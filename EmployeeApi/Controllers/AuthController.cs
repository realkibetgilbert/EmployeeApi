﻿using EmployeeApi.Dtos;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();

        private readonly IConfiguration _configuration;

        public AuthController( IConfiguration configuration) 
        {
          _configuration= configuration;
        }

        [HttpPost("register-user")]

        public ActionResult<User> Register(UserDto userDto) 
        {
          string passwordHash=BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            user.UserName=userDto.UserName;

            user.PasswordHash=passwordHash; 
            
            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto userDto)
        {
            if(user.UserName != userDto.UserName)
            {
                return BadRequest("User Not Found");
            }
            if(!BCrypt.Net.BCrypt.Verify(userDto.Password,user.PasswordHash))
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateToken(user);

            return Ok(token);
        }
        [NonAction]
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Name,"Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }
    }
}
