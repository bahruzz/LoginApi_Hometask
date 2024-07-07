using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace App.Controllers.Admin
{
    //[Authorize]
    public class AccountController : BaseController
    {

        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _accountService.GetUsersAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            return Ok(await _accountService.GetUserByUsernameAsync(username));
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoles()
        {
            await _accountService.CreateRoleAsync();
            return Ok();
        }
    }
}
