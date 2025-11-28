using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserRoleApi.Models;
using UserRoleApi.Models.Dtos;

namespace UserRoleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRoleDbContext _context;
        public UserController(UserRoleDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewUser(AddUserDto addUserDto)
        {
            try
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = addUserDto.Name,
                    Email = addUserDto.Email,
                    Password = addUserDto.Password
                };
                if (user != null)
                {
                    await _context.users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    return StatusCode(201, new { message = "Sikeres hozzáadás!", result = user} );
                }
                return StatusCode(404, new { message = "Sikertelen hozzáadás!", result = user });
            }
            catch (Exception ex)
            {

                return StatusCode(400, new { message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                return Ok(new { message = "Sikeres lekérdezés!", result = await _context.users.ToListAsync() });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }
        [HttpDelete("DeleteById")]
        public async Task<ActionResult> DeleteUserById(Guid id)
        {
            try
            {
                var user = _context.users.FirstOrDefault(x => x.Id == id);
                if (user != null)
                {
                    _context.users.Remove(user);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Sikeres törlés!", result = user });
                }
                return NotFound(new { message = "Nincs ilyen id!", result = user });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(Guid id, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _context.users.FirstOrDefaultAsync(x => x.Id == id);
                if (user != null)
                {
                    user.Name = updateUserDto.Name;
                    user.Email = updateUserDto.Email;
                    user.Password = updateUserDto.Password;
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Sikeres módosítás!", result = user });
                }
                return NotFound(new { message = "Nincs ilyen id!", result = user });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }
    }
}