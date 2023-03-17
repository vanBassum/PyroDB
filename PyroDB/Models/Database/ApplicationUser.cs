using Microsoft.AspNetCore.Identity;

namespace PyroDB.Models.Database
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Chemical> OwnedChems { get; set; } = new List<Chemical>();
    }
}