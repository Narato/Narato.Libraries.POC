using System;
using Microsoft.AspNetCore.Mvc;
using Narato.Correlations.Http.Interfaces;
using Narato.ResponseMiddleware.Models.Models;

namespace POC.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        private readonly IHttpClientFactory _factory;
        public ValuesController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        // GET api/values
        [HttpGet]
        public Paged<string> Get()
        {
            //System.Threading.Thread.Sleep(1000);
            return new Paged<string>(new string[] { "meep", "moop" }, 2, 10, 12);
        }

        // GET api/values/5
        [HttpGet("{id}", Name ="getbyid")]
        public string Get(int id)
        {
            var client = _factory.Create();
            var response = client.GetAsync("http://www.google.com").Result;

            return "value";
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            return new CreatedAtRouteResult("getbyid", new { Id = "ooh..." }, "meeeeeeeeeeep");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new Exception("gjigdj");
        }
    }
}
