using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Project.Models;
using POSAPI.Models;

namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivinePoolController : Controller
    {

        private readonly DivinePoolContext context;
        public DivinePoolController(DivinePoolContext context)
        {
            this.context = context;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<DivinePool>> Get(int id)
        {
            var item = await this.context.DivinePools.FindAsync(id);
            if (item == null)
                return BadRequest("Bad ID");
            return Ok(item);
        }

        [HttpPut]
        public async Task<ActionResult<DivinePool>> UpdateItem(DivinePool request)
        {
            var Divine = await this.context.DivinePools.FindAsync(request.id);
            if (Divine == null)
                return BadRequest("Bad ID");
            
            Divine.amount = Divine.amount + request.amount;
            
            await this.context.SaveChangesAsync();
            return Ok(await this.context.DivinePools.ToListAsync());
        }
    }
}
