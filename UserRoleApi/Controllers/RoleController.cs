using Microsoft.AspNetCore.Mvc;
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
    }
}