using System;
using System.Linq;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Services.Foundation.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
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
            try
            {
                User addedUser = await this.userService
                    .AddUserAsync(user);
                
                return Created(addedUser);
            }
            catch (UserValidationException userValidationException)
            {
                return BadRequest(userValidationException.InnerException);
            }
            catch (UserDependencyValidationException userDependencyValidationException)
                when (userDependencyValidationException.InnerException is AlreadyExistsUserException)
            {
                return Conflict(userDependencyValidationException.InnerException);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                return InternalServerError(userServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<User>> GetAllUsers()
        {
            try
            {
                IQueryable<User> users = this.userService
                    .RetrieveAllUsers();
                
                return Ok(users);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                return InternalServerError(userServiceException);
            }
        }

        [HttpGet("{id}")]
        public async ValueTask<ActionResult<User>> GetUserById(Guid id)
        {
            try
            {
                User user = await this.userService
                    .RetrieveUserByIdAsync(id);
                
                return Ok(user);
            }
            catch (UserValidationException userValidationException)
                when(userValidationException.InnerException is NotFoundUserException)
            {
                return NotFound(userValidationException.InnerException);
            }
            catch (UserValidationException userValidationException)
            {
                return BadRequest(userValidationException.InnerException);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                return InternalServerError(userServiceException);
            }
        }

        [HttpDelete("{id}")]
        public async ValueTask<ActionResult<User>> DeleteUserById(Guid id)
        {
            try
            {
                User user = await this.userService
                    .RemoveUserAsync(id);
                
                return Ok(user);
            }
            catch (UserValidationException userValidationException)
                when (userValidationException.InnerException is NotFoundUserException)
            {
                return NotFound(userValidationException.InnerException);
            }
            catch (UserValidationException userValidationException)
            {
                return BadRequest(userValidationException.InnerException);
            }
            catch (UserDependencyValidationException userDependencyValidationException)
            {
                return BadRequest(userDependencyValidationException.InnerException);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                return InternalServerError(userServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<User>> UpdateUser(User user)
        {
            try
            {
                User updatedUser = await this.userService
                    .ModifyUserAsync(user);
                
                return Ok(updatedUser);
            }
            catch (UserValidationException userValidationException)
                when(userValidationException.InnerException is NotFoundUserException)
            {
                return NotFound(userValidationException.InnerException);
            }
            catch (UserValidationException userValidationException)
            {
                return BadRequest(userValidationException.InnerException);
            }
            catch (UserDependencyValidationException userDependencyValidationException)
                when (userDependencyValidationException.InnerException is AlreadyExistsUserException)
            {
                return Conflict(userDependencyValidationException.InnerException);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                return InternalServerError(userServiceException);
            }
        }
    }
}
