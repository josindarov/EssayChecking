using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;

namespace EssayChecker.API.Services.Foundation.Users;

public partial class UserService
{
    private static void ValidateUserNotNull(User user)
    {
        if (user is null)
        {
            throw new UserNullException();
        }
    }
}