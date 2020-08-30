using JWT_Test.Data;
using JWT_Test.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Test.Services
{
    public class AuthenticationService
    {
        UserManager<User> UserManager { get; set; }
        SignInManager<User> SignInManager { get; set; }
        IConfiguration Config { get; set; }
        public AuthenticationService(UserManager<User> userManager,SignInManager<User> signInManager,IConfiguration config) {
            UserManager = userManager;
            SignInManager = signInManager;
            Config = config;
        }
        public async Task<UserModel> Authenticate(string username, string password) {
            var user = await UserManager.FindByNameAsync(username);

            if (user == null) return null;

            var sResult = await SignInManager.CheckPasswordSignInAsync(user, password, false);

            if (!sResult.Succeeded) return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Config["AppSettings:JWTSecret"]);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] { 
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Role,"arole")
                }),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new UserModel {
                Username = user.UserName,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
