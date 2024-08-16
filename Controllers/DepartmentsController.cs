using DalDocBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DepartmentsController(AppDbContext database) : ControllerBase
    {
        private readonly AppDbContext db = database;

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await db.Departments.ToListAsync();
            return Ok(departments);
        }
    }
}
