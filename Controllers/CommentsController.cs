using DalDocBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class CommentsController(AppDbContext database) : ControllerBase
    {
        private readonly AppDbContext db = database;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(string department)
        {
            return await db.Comments
                .Where(c => c.Department == department)
                .OrderBy(c => c.Timestamp)
                .ToListAsync();
        }
    }
}