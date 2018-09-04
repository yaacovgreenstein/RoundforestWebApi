using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace roundforestwebapi.Controllers
{
    [Route("api/[controller]")]
    public class MiningController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody]UrlToGet url)
        {
            IMineProduct mwm = new MineWallMart();
            string productNumber = await mwm.MineProduct(url.Url);
        }
    }
}
