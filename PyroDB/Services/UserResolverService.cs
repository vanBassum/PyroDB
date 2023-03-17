using Microsoft.AspNetCore.Identity;
using PyroDB.Models.Database;

namespace PyroDB.Services
{
    public class UserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }


       //public ApplicationUser GetCurrentUser()
       //{
       //    
       //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
       //
       //    return _context.HttpContext?.User?.Identity
       //}

    }
}
