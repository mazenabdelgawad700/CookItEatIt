﻿using Microsoft.AspNetCore.Identity;
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
using System.Security.Claims;
using System.Text;

namespace RecipeApp.Service.Implementation
{
    internal class AuthenticationService : IAuthenticationService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IConfirmEmailSerivce _confirmEmailSerivce;
        private readonly ISendPasswordChangeNotificationEmailService
            _sendPasswordChangeNotificationEmailService;
        private readonly AppDbContext _dbContext;

        public AuthenticationService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings, IConfirmEmailSerivce confirmEmailSerivce, AppDbContext dbContext, ISendPasswordChangeNotificationEmailService
            sendPasswordChangeNotificationEmailService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _confirmEmailSerivce = confirmEmailSerivce;
            _dbContext = dbContext;
            _sendPasswordChangeNotificationEmailService = sendPasswordChangeNotificationEmailService;
            _signInManager = signInManager;
        }

        public async Task<ReturnBase<string>> LoginInAsync(string email, string password, string ipAddress)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return ReturnBaseHandler.Failed<string>("InvalidCredentials");

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

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
                    if (loginAttempt != null)
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

                if (loginAttempt != null)
                {
                    loginAttempt.AttemptCount = 0;
                    loginAttempt.IsBlocked = false;
                    loginAttempt.LastAttemptTime = DateTime.UtcNow;
                }

                await _dbContext.SaveChangesAsync();


                string token = GenerateJwtToken(user.UserName!, user.Id);

                if (!user.EmailConfirmed)
                {
                    ReturnBase<bool> sendConfirmationEmailResult = await _confirmEmailSerivce.SendConfirmationEmailAsync(user);
                    if (sendConfirmationEmailResult.Succeeded)
                    {
                        await transaction.CommitAsync();
                        await transaction.DisposeAsync();
                        return ReturnBaseHandler.Success(token, $"Confirmation Email has been sent to {user.Email}. Please confirm your email.");
                    }
                }

                await transaction.CommitAsync();
                await transaction.DisposeAsync();
                return ReturnBaseHandler.Success(token, "Logged in successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.Message);
                return ReturnBaseHandler.Failed<string>("An error occurred during login.");
            }
        }

        public async Task<ReturnBase<string>> RegisterAsync(ApplicationUser appUser, string password)
        {
            try
            {
                IdentityResult createUserResult = await _userManager.CreateAsync(appUser, password);

                if (createUserResult.Succeeded)
                {
                    ReturnBase<bool> sendConfirmationEmailResult = await _confirmEmailSerivce.SendConfirmationEmailAsync(appUser);
                    if (sendConfirmationEmailResult.Data)
                    {
                        return ReturnBaseHandler.Created("", $"Confirmation Email has been sent to {appUser.Email} Please, confirm your email");
                    }
                    return ReturnBaseHandler.Created("", "We could not send a confirmation email to you, please log in to confirm your email!");
                }
                return ReturnBaseHandler.Failed<string>("An error occurred while creating user.");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<string>($"{ex.Message}");
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

                ApplicationUser? user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    return ReturnBaseHandler.Success(true, "Email is already registered.");
                }
                return ReturnBaseHandler.Success(false, "Email is available.");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>($"An error occurred: {ex.Message}");
            }
        }
        private string GenerateJwtToken(string username, int userId)
        {
            List<Claim> claims = GetClaims(username, userId);

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
        private List<Claim> GetClaims(string username, int userId)
        {
            return [
                new Claim(JwtRegisteredClaimNames.Name, username),
                new Claim("UserId", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];
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
    }
}