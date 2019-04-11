using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AngularPerformanceSamples.Controllers
{
    [Route("api/[controller]")]
    public class HttpRedisController : Controller
    {
        private const string headerName = "numArray";
        [HttpPost("[action]")]
        public IActionResult NonRedisCheck()
        {
            var headersNumArray = GetNumListFromHeader(headerName);
            var randomNumberToFind = new Random().Next(headersNumArray.Count() * 10);
            var exists = headersNumArray.Contains(randomNumberToFind);
            return Ok(exists);
        }
        [HttpGet("[action]")]
        public IActionResult RedisCheck()
        {
            return Ok();
        }
        private IEnumerable<int> GetNumListFromHeader(string headerName)
        {
            if (Request.Headers.TryGetValue(headerName, out var numArray))
            {
                var nums = numArray.FirstOrDefault();
                return nums.Split(',').Select(i => int.Parse(i)).ToList();
            }
            return null;
        }
    }
}
