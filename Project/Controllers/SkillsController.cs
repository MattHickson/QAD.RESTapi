using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Project.Models;
using POSAPI.Models;

namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : Controller
    {

        private readonly SkillsContext context;
        public SkillsController(SkillsContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Skills>>> Get(string target)
        {
            var splittarget = target.Split(";");
            var skills = await context.Skills.ToListAsync();
            List<Skills> targetskills = new List<Skills>();
            if(target == "")
            {
                return Ok(await this.context.Skills.ToListAsync());
            }
            else
            {
                
               for(int y = 0; y < skills.Count; y++)
               {
                    bool skillpass = true;
                    for (int x = 0; x < splittarget.Length; x++)
                        if (skillpass && skills[y].Tags.ToLower().Contains(splittarget[x].ToLower()))
                        {
                        
                        }
                    else
                    {
                        skillpass = false;
                    }
                    if (skillpass)
                    {
                        targetskills.Add(skills[y]);

                    }
                }
            }
            return Ok(targetskills);
        }
        [HttpPost]
        public async Task<ActionResult<List<Skills>>> AddItem(Skills addedItem)
        {
            this.context.Skills.Add(addedItem);
            await this.context.SaveChangesAsync();
            return Ok(await this.context.Skills.ToListAsync());
        }

    }
}
