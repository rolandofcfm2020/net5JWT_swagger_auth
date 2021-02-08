using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiNet5.NorthwndDataAccess;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using ApiNet5.Backend;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ApiNet5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly NORTHWNDContext _context =  new NORTHWNDContext();
        public IConfiguration _config;
        public UsersController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Login(string userName, string password)
        {
            IActionResult response = Unauthorized();
            var user = login(userName, password);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }



        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserName))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (UserExists(user.UserName))
            {
                return Conflict();
            }
          

            user.Salt = Guid.NewGuid().ToString();
            user.PasswordHash = CryptographySC.Encrypt(user.PasswordHash, user.Salt);
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                
            }

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }



        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string name)
        {
            return _context.Users.Any(e => e.UserName == name);
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:PrivateKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("valid", "1"));
            permClaims.Add(new Claim("userid", userInfo.UserId.ToString()));
            permClaims.Add(new Claim("name", userInfo.Email));


            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"], claims: permClaims,
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);



            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User login(string userName, string password)
        {
            var user = _context.Users.Where(w => w.UserName == userName).FirstOrDefault();

            var salt = user.Salt;
            var decryptedPassword = CryptographySC.Decrypt(user.PasswordHash, salt);

            if (decryptedPassword == password)
                return user;

            else
                return null;
        }


    }
}
