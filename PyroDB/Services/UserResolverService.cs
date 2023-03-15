namespace PyroDB.Services
{
    public class UserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string? GetCurrentUser()
        {
            return _context.HttpContext?.User?.Identity?.Name;
        }
    }
}
