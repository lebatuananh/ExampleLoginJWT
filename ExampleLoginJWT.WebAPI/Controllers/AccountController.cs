using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExampleLoginJWT.Common.Constants;
using ExampleLoginJWT.Domain;
using ExampleLoginJWT.Domain.Entity;
using ExampleLoginJWT.WebAPI.Attributes;
using ExampleLoginJWT.WebAPI.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ExampleLoginJWT.WebAPI.Controllers
{
    public class AccountController : AdminV1Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ExampleLoginJwtDbContext _context;

        public AccountController(IConfiguration configuration, SignInManager<User> signInManager,
            UserManager<User> userManager, RoleManager<Role> roleManager, ExampleLoginJwtDbContext context)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [AllowAnonymous]
        [Route("login")]
        [ValidateModel]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, true);
                if (!result.Succeeded)
                    return BadRequest("Mật khẩu không đúng");
                var roles = await _userManager.GetRolesAsync(user);
                var permissionsInUser = from permission in _context.Permissions.ToList()
                    join function in _context.Functions.ToList() on permission.FunctionId equals function.Id
                    join role in _roleManager.Roles.ToList() on permission.RoleId equals role.Id
                    join command in _context.Commands.ToList() on permission.CommandId equals command.Id
                    join userRole in _context.UserRoles.ToList() on role.Id equals userRole.RoleId
                    where userRole.UserId == user.Id
                    select new
                    {
                        Id = function.Id + "_" + command.Id
                    };
                var permissions = new List<string>();
                var inUser = permissionsInUser.ToList();
                if (inUser.Any())
                {
                    permissions.AddRange(inUser.Select(item => item.Id));
                }


                var claims = new[]
                {
                    new Claim("Email", user.Email),
                    new Claim(SystemConstants.UserClaim.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(SystemConstants.UserClaim.Roles, string.Join(";", roles)),
                    new Claim(SystemConstants.UserClaim.Permissions, JsonConvert.SerializeObject(permissions)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                    _configuration["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddDays(2),
                    signingCredentials: creds);

                return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token)});
            }

            return NotFound($"Không tìm thấy tài khoản {model.UserName}");
        }
    }
}