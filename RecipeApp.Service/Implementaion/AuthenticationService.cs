using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Domain.Helpers;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeApp.Service.Implementaion
{
    internal class AuthenticationService : IAuthenticationService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IConfirmEmailSerivce _confirmEmailSerivce;

        public AuthenticationService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings, IConfirmEmailSerivce confirmEmailSerivce)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _confirmEmailSerivce = confirmEmailSerivce;
        }

        public async Task<ReturnBase<string>> LoginInAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return ReturnBaseHandler.Failed<string>("InvalidCredentials");

            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return ReturnBaseHandler.Failed<string>("InvalidEmailOrPassword");

            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordCorrect)
                return ReturnBaseHandler.Failed<string>("InvalidEmailOrPassword");

            string token = GenerateJwtToken(user.UserName!, user.Id);

            if (!user.EmailConfirmed)
            {
                ReturnBase<bool> sendConfirmationEmailResult = await _confirmEmailSerivce.SendConfirmationEmailAsync(user);
                if (sendConfirmationEmailResult.Succeeded)
                {
                    return ReturnBaseHandler.Success(token, $"Confirmation Email has been sent to {user.Email} Please, confirm your email");
                }
            }
            return ReturnBaseHandler.Success(token, "Logged in successfully");
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
                expires: DateTime.Now.AddMonths(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private List<Claim> GetClaims(string username, int userId)
        {
            return [
                new Claim(JwtRegisteredClaimNames.Name, username),
                new Claim(JwtRegisteredClaimNames.NameId, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];
        }
    }
}