using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace PyroDB.Models.Database
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Chemical> OwnedChems { get; set; } = new List<Chemical>();

        public override string ToString() => UserName ?? "";
    }
}