using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Services.Foundation.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EssayChecker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<User>> PostUserAsync(User user)
        {
            return await userService.AddUserAsync(user);
        }
    }
}
