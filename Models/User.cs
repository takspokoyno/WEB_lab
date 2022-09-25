using Microsoft.AspNetCore.Identity;

namespace Labka1.Models
{
    public class User : IdentityUser
    {
        public string Nickname { get; set; }
    }
}
