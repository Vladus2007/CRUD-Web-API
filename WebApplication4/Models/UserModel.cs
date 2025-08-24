using Microsoft.AspNetCore.Identity;
namespace WebApplication4.Models
{
    public class UserModel:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }

        
       public string Email { get; set; }

    }
}
