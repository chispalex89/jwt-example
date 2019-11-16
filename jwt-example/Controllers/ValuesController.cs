using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace jwt_example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("{key}")]
        public string Post([FromBody] Dictionary<string, object> value, string key)
        {
            var handler = new JwtSecurityTokenHandler();

            //create symmetrickey
            var buffer = key.PadRight(64, ' ')
                .ToCharArray()
                .Select(x => Convert.ToByte(x))
                .ToArray();

            var claims = value.Select(x => new Claim(x.Key, x.Value.ToString()));

            //create jwt
            var description = new SecurityTokenDescriptor {
                Issuer = "nosotros :D",
                Audience = "audience",
                Expires =  DateTime.UtcNow.AddSeconds(10),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(buffer), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha512Digest)
            };
            var token = handler.CreateToken(description);
            //validate jwt
            var tokenString = handler.WriteToken(token);

            return tokenString;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
