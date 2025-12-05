using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using UserRoleApi.Models;
using UserRoleApi.Models.Dtos;

namespace UserRoleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly UserRoleDbContext _context;
        public RoleController(UserRoleDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewRole(AddRoleDto addRoleDto)
        {
            try
            {
                var role = new Role
                {
                    Id = Guid.NewGuid(),
                    RoleName = addRoleDto.RoleName
                };
                if (role != null)
                {
                    await _context.roles.AddAsync(role);
                    await _context.SaveChangesAsync();
                    return StatusCode(201, new
                    {
                        message = "Sikeres hozzáadás!",
                        result = role
                    });
                }
                return StatusCode(404, new
                {
                    message = "Sikertelen hozzáadás!",
                    result = role
                });
            }
            catch (Exception ex)
            {

                return StatusCode(400, new
                {
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetAllRoles()
        {
            try
            {
                return Ok(new
                {
                    message = "Sikeres lekérdezés!",
                    result = await _context.roles.ToListAsync()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    message = ex.Message
                });
            }
        }
        [HttpDelete("DeleteById")]
        public async Task<ActionResult> DeleteRoleById(Guid id)
        {
            try
            {
                var role = _context.roles.FirstOrDefault(x => x.Id == id);
                if (role != null)
                {
                    _context.roles.Remove(role);
                    await _context.SaveChangesAsync();
                    return Ok(new
                    {
                        message = "Sikeres törlés!",
                        result = role
                    });
                }
                return NotFound(new
                {
                    message = "Nincs ilyen id!",
                    result = role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateRole(Guid id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                var role = await _context.roles.FirstOrDefaultAsync(x => x.Id == id);
                if (role != null)
                {
                    role.RoleName = updateRoleDto.RoleName;
                    await _context.SaveChangesAsync();
                    return Ok(new
                    {
                        message = "Sikeres módosítás!",
                        result = role
                    });
                }
                return NotFound(new
                {
                    message = "Nincs ilyen id!",
                    result = role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    message = ex.Message
                });
            }
        }
        [HttpGet("RoleWithUsers")]
        public async Task<ActionResult> GetRolesWithUsers(Guid id)
        {
            try
            {
                var rolesWithUsers =  await _context.roleuser
                    .Where(ru => ru.RoleId == id)
                    .Include(ru => ru.User)
                    .Select(ru => ru.User.Name)
                    .ToListAsync();

                if (rolesWithUsers != null)
                {
                    return StatusCode(200, new { message = "Sikeres lekérdezés!", result  = rolesWithUsers });
                }
                return NotFound(new { message = "Nincs ilyen id!", result = rolesWithUsers });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    message = ex.Message
                });
            }
        }
    }
}