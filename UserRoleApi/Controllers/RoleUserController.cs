using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult> AddNewRoleToUser(RoleUser roleUser)
        {
            try
            {
                bool userExists = await _context.users.AnyAsync(ru => roleUser.UserId == roleUser.UserId);
                if (!userExists)
                {
                    var roleuser = new RoleUser
                    {
                        UserId = roleUser.UserId,
                        RoleId = roleUser.RoleId,
                    };
                
                    await _context.roleuser.AddAsync(roleuser);
                    await _context.SaveChangesAsync();
                    return StatusCode(201, new { message = "Sikeres hozzáadás!", result = roleUser });
                }
                return BadRequest(new { message = "Sikertelen összerendelés!", result = "" });
            }
            catch (Exception ex)
            {

                return StatusCode(400, new { message = ex.Message });
            }
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteUseRoles(Guid userid)
        {

            try
            {
                var deltedUserRoles = await _context.roleuser
                    .Where(ru => ru.UserId == userid)
                    .ToListAsync();

                if (deltedUserRoles != null)
                {
                    foreach (var i in deltedUserRoles)
                    {
                        _context.roleuser.Remove(i);
                    }

                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Sikeres törlés.", result = deltedUserRoles });
                }

                return NotFound(new { message = "nincs ilyn id.", result = deltedUserRoles });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message, result = "" });
            }
        }
    }
}
