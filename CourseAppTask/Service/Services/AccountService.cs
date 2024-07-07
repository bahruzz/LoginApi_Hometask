using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.DTOs.Account;
using Service.Helpers.Account;
using Service.Helpers.Enums;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;

        public AccountService(UserManager<AppUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IMapper mapper,
                              IConfiguration configuration,
                              IOptions<JwtSettings> jwtSettings)
        {
            
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _jwtSettings = jwtSettings.Value; 
        }

        public async Task CreateRoleAsync()
        {
            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
        }

        public async Task<UserDto> GetUserByUsernameAsync(string userName)
        {
            var existUser = await _userManager.FindByNameAsync(userName);
            if (existUser == null) throw new NotFoundException("User not exist");
            return _mapper.Map<UserDto>(existUser);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
           return _mapper.Map<IEnumerable<UserDto>>(await _userManager.Users.ToListAsync());
        }

        public async Task<LoginResponse> SignInAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.EmailOrUsername);

            if (user is null)
             user = await _userManager.FindByNameAsync(model.EmailOrUsername);

            if (user == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Error = "Login failed",
                    Token = null
                };
            }

            bool result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return new LoginResponse
                {
                    Success = false,
                    Error = "Login failed",
                    Token = null
                };
            }
            var userRoles = await _userManager.GetRolesAsync(user);


            string token = GenerateJwtToken(user.UserName, (List<string>)userRoles);

            return new LoginResponse
            {
                Success = true,
                Error = null,
                Token = token
            };
        }

        public async Task<RegisterResponse> SignUp(RegisterDto model)
        {
            var user=_mapper.Map<AppUser>(model);
            IdentityResult result= await _userManager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
            {
                return new RegisterResponse()
                {
                    Success = false,
                    Errors = result.Errors.Select(m => m.Description)
                };
            }

            await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            return new RegisterResponse()
            {
                Success = true,
                Errors = null
            };
        }


        private string GenerateJwtToken(string username, List<string> roles)
        {
            var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, username),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.NameIdentifier, username)
        };

            roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.ExpireDays));

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
