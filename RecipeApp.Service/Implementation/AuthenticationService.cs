﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Domain.Helpers;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RecipeApp.Service.Implementation
{
    internal class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IConfirmEmailService _confirmEmailService;
        private readonly ISendEmailService _emailService;
        private readonly ISendPasswordChangeNotificationEmailService
            _sendPasswordChangeNotificationEmailService;
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructors
        public AuthenticationService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings, IConfirmEmailService confirmEmailService, AppDbContext dbContext, ISendPasswordChangeNotificationEmailService
            sendPasswordChangeNotificationEmailService, SignInManager<ApplicationUser> signInManager, ISendEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _confirmEmailService = confirmEmailService;
            _dbContext = dbContext;
            _sendPasswordChangeNotificationEmailService = sendPasswordChangeNotificationEmailService;
            _signInManager = signInManager;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Authentication Handle Functions
        public async Task<ReturnBase<string>> LoginInAsync(string email, string password, string ipAddress)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return ReturnBaseHandler.Failed<string>("InvalidCredentials");

            await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(email);
                if (user is null)
                    return ReturnBaseHandler.Failed<string>("InvalidEmailOrPassword");

                LoginAttempt? loginAttempt = await _dbContext.LoginAttempt
                    .Where(a => a.UserId == user.Id)
                    .OrderByDescending(a => a.LastAttemptTime)
                    .FirstOrDefaultAsync();

                bool isBlocked = loginAttempt?.IsBlocked ?? false;
                DateTime? lastAttemptTime = loginAttempt?.LastAttemptTime;

                if (isBlocked && lastAttemptTime.HasValue)
                {
                    TimeSpan blockDuration = DateTime.UtcNow - lastAttemptTime.Value;
                    if (blockDuration < TimeSpan.FromMinutes(30))
                    {
                        return ReturnBaseHandler.Failed<string>(
                            $"Too many failed attempts. Try again after {30 - blockDuration.TotalMinutes:F0} minutes.");
                    }

                    loginAttempt.IsBlocked = false;
                    loginAttempt.AttemptCount = 0;
                    await _dbContext.SaveChangesAsync();
                }

                bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);

                if (!isPasswordCorrect)
                {
                    if (loginAttempt is not null)
                    {
                        loginAttempt.AttemptCount++;

                        if (loginAttempt.AttemptCount >= 3)
                        {
                            loginAttempt.IsBlocked = true;
                        }

                        loginAttempt.LastAttemptTime = DateTime.UtcNow;
                    }
                    else
                    {
                        loginAttempt = new LoginAttempt
                        {
                            UserId = user.Id,
                            IpAddress = ipAddress,
                            AttemptCount = 1,
                            LastAttemptTime = DateTime.UtcNow,
                            IsBlocked = false
                        };

                        await _dbContext.LoginAttempt.AddAsync(loginAttempt);
                    }

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return ReturnBaseHandler.Failed<string>("InvalidEmailOrPassword");
                }

                if (loginAttempt is not null)
                {
                    loginAttempt.AttemptCount = 0;
                    loginAttempt.IsBlocked = false;
                    loginAttempt.LastAttemptTime = DateTime.UtcNow;
                }



                string jwtId = Guid.NewGuid().ToString();
                string token = await GenerateJwtToken(user, jwtId);


                ApplicationUserRefreshToken newRefreshToken = new()
                {
                    UserId = user.Id,
                    RefreshToken = GenerateRefreshToken(),
                    JwtId = jwtId,
                    IsUsed = false,
                    IsRevoked = false,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenExpireDate)
                };

                ApplicationUserRefreshToken? existingRefreshTokenRecord = await _dbContext.UserRefreshToken
                    .FirstOrDefaultAsync(rt => rt.UserId == user.Id);

                if (existingRefreshTokenRecord is null)
                {
                    await _dbContext.UserRefreshToken.AddAsync(newRefreshToken);
                }
                else
                {
                    existingRefreshTokenRecord.RefreshToken = GenerateRefreshToken();
                    existingRefreshTokenRecord.IsUsed = true;
                    existingRefreshTokenRecord.CreatedAt = DateTime.UtcNow;
                    existingRefreshTokenRecord.ExpiresAt = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenExpireDate);

                    _dbContext.UserRefreshToken.Update(existingRefreshTokenRecord);
                }

                if (!user.EmailConfirmed)
                {
                    ReturnBase<bool> sendConfirmationEmailResult = await _confirmEmailService.SendConfirmationEmailAsync(user);
                    if (sendConfirmationEmailResult.Succeeded)
                    {
                        await transaction.CommitAsync();
                        await transaction.DisposeAsync();
                        return ReturnBaseHandler.BadRequest<string>($"A Confirmation Email has been sent to {user.Email}. Please confirm your email first and then log in.");
                    }
                }

                var checkIfUserHasUserRole = await _userManager.IsInRoleAsync(user, "User");
                var checkIfUserHasAdminRole = await _userManager.IsInRoleAsync(user, "Admin");

                if (!checkIfUserHasUserRole && !checkIfUserHasAdminRole)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                await transaction.DisposeAsync();
                return ReturnBaseHandler.Success(token, "Logged in successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ReturnBaseHandler.Failed<string>(ex.Message);
            }
        }
        public async Task<ReturnBase<string>> RegisterAsync(ApplicationUser appUser, string password)
        {
            try
            {
                IdentityResult createUserResult = await _userManager.CreateAsync(appUser, password);

                if (createUserResult.Succeeded)
                {
                    ReturnBase<bool> sendConfirmationEmailResult = await _confirmEmailService.SendConfirmationEmailAsync(appUser);

                    await _userManager.AddToRoleAsync(appUser, "User");

                    if (sendConfirmationEmailResult.Data)
                    {
                        return ReturnBaseHandler.Created("", $"Confirmation Email has been sent to {appUser.Email} Please, confirm your email");
                    }
                    return ReturnBaseHandler.Created("", "We could not send a confirmation email to you, please log in to confirm your email!");
                }
                return ReturnBaseHandler.Failed<string>(createUserResult.Errors.FirstOrDefault().Code);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<string>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(currentPassword))
                    return ReturnBaseHandler.Failed<bool>("PasswordsDoNotProvided");

                ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
                if (user is null)
                {
                    await transaction.RollbackAsync();
                    return ReturnBaseHandler.Failed<bool>("InvalidUserId");
                }

                IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (changePasswordResult.Succeeded)
                {
                    var sendEmailResult = await _sendPasswordChangeNotificationEmailService.SendPasswordChangeNotificationAsync(user);
                    if (!sendEmailResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return ReturnBaseHandler.Failed<bool>("FailedToSendChangePasswordEmail");
                    }

                    await _signInManager.SignOutAsync();
                    await transaction.CommitAsync();
                    return ReturnBaseHandler.Success(true, "Password has been changed successfully");
                }

                await transaction.RollbackAsync();
                return ReturnBaseHandler.Failed<bool>("Failed To Change Password");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> IsEmailAlreadyRegisteredAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return ReturnBaseHandler.BadRequest<bool>("Email is required.");
                }

                ApplicationUser? user = await _dbContext.ApplicationUser.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();

                if (user is not null)
                {
                    return ReturnBaseHandler.Success(true, "Email is already registered.");
                }
                return ReturnBaseHandler.Failed<bool>();
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> IsUserNameAlreadyExistAsync(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    return ReturnBaseHandler.BadRequest<bool>("User Name is required.");
                }

                ApplicationUser? user = await _userManager.FindByNameAsync(userName);
                if (user is not null)
                {
                    return ReturnBaseHandler.Success(true, "User Name already used.");
                }
                return ReturnBaseHandler.Failed<bool>();
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> SendResetPasswordEmailAsync(string email)
        {
            try
            {
                if (email is null)
                    return ReturnBaseHandler.Failed<bool>("Email is required");

                ApplicationUser? user = await _userManager.FindByEmailAsync(email);

                if (user is null)
                    return ReturnBaseHandler.Failed<bool>("User Not Found");

                string resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                string encodedToken = WebUtility.UrlEncode(resetPasswordToken);
                HttpRequest requestAccessor = _httpContextAccessor.HttpContext.Request;

                UriBuilder uriBuilder = new()
                {
                    Scheme = requestAccessor.Scheme,
                    Host = requestAccessor.Host.Host,
                    Port = requestAccessor.Host.Port ?? -1,
                    Path = "api/authentication/ResetPassword",
                    Query = $"email={Uri.EscapeDataString(email)}&token={encodedToken}"
                };

                string returnUrl = uriBuilder.ToString();

                string message = $"To Reset Your Password Click This Link: <a href=\"{returnUrl}\">Reset Password</a>";

                ReturnBase<string> sendEmailResult = await _emailService.SendEmailAsync(email, message, "Reset Password Link", "text/html");

                if (sendEmailResult.Succeeded)
                    return ReturnBaseHandler.Success(true, "Reset password email send successfully");

                return ReturnBaseHandler.Failed<bool>(sendEmailResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> ResetPasswordAsync(string resetPasswordToken, string newPassword, string email)
        {
            try
            {
                if (string.IsNullOrEmpty(resetPasswordToken))
                    return ReturnBaseHandler.Failed<bool>("Invalid Token");

                ApplicationUser? user = await _userManager.FindByEmailAsync(email);

                if (user is null)
                    return ReturnBaseHandler.Failed<bool>("User Not Found");

                string decodedToken = WebUtility.UrlDecode(resetPasswordToken);

                IdentityResult resetPasswordResult = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

                if (resetPasswordResult.Succeeded)
                    return ReturnBaseHandler.Success(true, "Password has been reset successfully");

                return ReturnBaseHandler.Failed<bool>(resetPasswordResult.Errors.FirstOrDefault().Description);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        #endregion

        #region Token Handle Functions
        private async Task<string> GenerateJwtToken(ApplicationUser user, string jwtId)
        {
            List<Claim> claims = await GetClaimsAsync(user, jwtId);

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user, string jwtId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims =
            [
                new Claim("UserId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            ];
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            return claims;
        }
        private bool IsAccessTokenExpired(string accessToken)
        {
            try
            {

                JwtSecurityTokenHandler tokenHandler = new();
                if (tokenHandler.ReadToken(accessToken) is not JwtSecurityToken token)
                    return true;

                DateTimeOffset expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value));

                return expirationTime.UtcDateTime <= DateTime.UtcNow;
            }
            catch
            {
                return true;
            }
        }
        public async Task<ReturnBase<string>> RefreshTokenAsync(string accessToken)
        {
            try
            {
                if (!IsAccessTokenExpired(accessToken))
                    return ReturnBaseHandler.Success("", "Access Token Is Valid");

                string? userId = GetUserIdFromToken(accessToken);
                string? jwtId = GetJwtIdFromToken(accessToken);

                if (jwtId is null || userId is null)
                    return ReturnBaseHandler.Failed<string>("InvalidAccessToken");

                ApplicationUserRefreshToken? storedRefreshToken = await _dbContext.UserRefreshToken
                    .FirstOrDefaultAsync(rt => rt.UserId.ToString() == userId && rt.JwtId == jwtId);

                if (storedRefreshToken is null || storedRefreshToken.IsRevoked)
                    return ReturnBaseHandler.Failed<string>("Your session has expired. please log in again.");

                if (storedRefreshToken.ExpiresAt < DateTime.UtcNow)
                {
                    storedRefreshToken.IsRevoked = true;
                    _dbContext.UserRefreshToken.Update(storedRefreshToken);
                    await _dbContext.SaveChangesAsync();
                    return ReturnBaseHandler.Failed<string>("Your session has expired. please log in again.");
                }

                if (!storedRefreshToken.IsUsed)
                {
                    storedRefreshToken.IsUsed = true;
                    _dbContext.UserRefreshToken.Update(storedRefreshToken);
                }

                ApplicationUser? user = await _userManager.FindByIdAsync(userId);

                if (user is null)
                    return ReturnBaseHandler.Failed<string>("InvalidAccessToken");


                string newJwtId = Guid.NewGuid().ToString();
                string newAccessToken = await GenerateJwtToken(user, newJwtId);

                storedRefreshToken.JwtId = newJwtId;

                await _dbContext.SaveChangesAsync();

                if (newAccessToken is null)
                    return ReturnBaseHandler.Failed<string>("FailedToGenerateNewAccessToken");

                return ReturnBaseHandler.Success(newAccessToken, "New Access Token Created");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<string>(ex.Message);
            }
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private string? GetJwtIdFromToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

            return jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        }
        private string? GetUserIdFromToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

            return jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value.ToString();
        }
        #endregion
    }
}