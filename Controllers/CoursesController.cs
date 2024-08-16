
using DalDocBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class CoursesController(AppDbContext database) : ControllerBase
    {
        private readonly AppDbContext db = database;

        [HttpGet]
        public async Task<IActionResult> GetCourses([FromQuery] string department)
        {
            var courseCodes = await db.Courses
                    .Join(db.Departments,
                          c => c.department_id,
                          d => d.id,
                          (c, d) => new { c, d })
                    .Where(cd => cd.d.Name == department)
                    .Select(cd => cd.c.Code)
                    .ToListAsync();

            return Ok(courseCodes);
        }


    }
}