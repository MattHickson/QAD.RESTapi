using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSAPI.Models;
using FuzzyString;
using Microsoft.Extensions.Options;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonsterModController : ControllerBase
    {


        private readonly MonsterModContext context;
        public MonsterModController(MonsterModContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<MonsterMod>>> Get()
        {
            return Ok(await this.context.MonsterMods.ToListAsync());
        }
        [HttpGet("Name")]
        public async Task<ActionResult<MonsterMod>> Get(string name)
        {
            FuzzyStringComparisonTolerance tolerance = FuzzyStringComparisonTolerance.Normal;
            name = name.ToLower();
            var placeholder = "";
            MonsterMod payload = new MonsterMod();
            var list = await this.context.MonsterMods.ToListAsync();
            List<FuzzyStringComparisonOptions> options = new List<FuzzyStringComparisonOptions>();
            options.Add(FuzzyStringComparisonOptions.UseOverlapCoefficient);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);

            for (int i = 0; i < list.Count; i++)
            {
                if(placeholder == "")
                {
                    if (list[i].ModName.ApproximatelyEquals(name, tolerance, options[0]))
                    {
                        placeholder = list[i].ModName;
                        payload = list[i];
                    }
                }
                if(placeholder.OverlapCoefficient(name) < list[i].ModName.OverlapCoefficient(name) && list[i].ModName.ApproximatelyEquals(name, tolerance, options[0]))
                {
                    placeholder = list[i].ModName;
                    payload = list[i];
                }
                if (i == list.Count-1 && placeholder != "")
                {
                   
                    return Ok(payload);
                }
            }
            return BadRequest("Bad name");
         
        }
        [HttpPost]
        public async Task<ActionResult<List<MonsterMod>>> AddItem(MonsterMod addedItem)
        {
            this.context.MonsterMods.Add(addedItem);
            await this.context.SaveChangesAsync();
            return Ok(await this.context.MonsterMods.ToListAsync());
        }
        [HttpDelete]
        public async Task<ActionResult<List<MonsterMod>>> DeleteItem(int id)
        {
            var item = await this.context.MonsterMods.FindAsync(id);
            if (item == null)
                return BadRequest("Bad ID");
            this.context.MonsterMods.Remove(item);
            await this.context.SaveChangesAsync();
            return Ok(await this.context.MonsterMods.ToListAsync());
        }

    }
}
