using Microsoft.AspNetCore.Mvc;
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

    }
}