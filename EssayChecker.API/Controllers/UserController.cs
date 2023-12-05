using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Services.Foundation.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RESTFulSense.Controllers;

namespace EssayChecker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : RESTFulController
    {
        private IUserService userService;
        public UserController(IUserService userService) =>
            this.userService = userService;
        
        [HttpPost]
        public async ValueTask<ActionResult<User>> PostUserAsync(User user)
        {
            User addedUser = await this.userService.AddUserAsync(user);
            return Created(addedUser);
        }

        [HttpGet]
        public ActionResult<IQueryable<User>> GetAllUsers()
        {
            IQueryable<User> users = this.userService.RetrieveAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async ValueTask<ActionResult<User>> GetUserById(Guid id)
        {
            User user = await this.userService.RetrieveUserByIdAsync(id);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async ValueTask<ActionResult<User>> DeleteUserById(Guid id)
        {
            User user = await this.userService.RemoveUserAsync(id);
            return Ok(user);
        }

        [HttpPut]
        public async ValueTask<ActionResult<User>> UpdateUser(User user)
        {
            User updatedUser = await this.userService.ModifyUserAsync(user);
            return Ok(updatedUser);
        }
    }
}
