using CaravanDomain.Entities;
using CaravanDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Caravan.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MoneyController : ControllerBase {
        private readonly UserManager<PlayerUser> _userManager;

        public MoneyController(UserManager<PlayerUser> userManager) {
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task AddMoney(int amount, string playerId) {
            var user = await _userManager.FindByIdAsync(playerId);
            user.Money += amount;
            await _userManager.UpdateAsync(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task Remove(int amount, string playerId) {
            var user = await _userManager.FindByIdAsync(playerId);
            if (user.Money >= amount) {
                user.Money -= amount;
                await _userManager.UpdateAsync(user);
            }
            //error

        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult<int>> GetMoney() {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user == null) {
                return NotFound();
            }
            return Ok(user.Money);
        }
    }
}
