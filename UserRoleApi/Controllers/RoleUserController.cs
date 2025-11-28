using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserRoleApi.Models;
using UserRoleApi.Models.Dtos;

namespace UserRoleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleUserController : ControllerBase
    {
        private readonly UserRoleDbContext _context;
        public RoleUserController(UserRoleDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewRoleToUser(AddNewSwitchDto roleUser)
        {
            try
            {
                var roleuser = new RoleUser {
                    UserId = roleUser.UserId,
                    RoleId = roleUser.RoleId,
                };
                await _context.roleuser.AddAsync(roleuser);
                await _context.SaveChangesAsync();
                return StatusCode(201, new { message = "Sikeres hozzáadás!", result = roleUser });
            }
            catch (Exception ex)
            {

                return StatusCode(400, new { message = ex.Message });
            }
        }
    }
}
