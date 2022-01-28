using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FrwkQuickWait.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [ValidateNever]
        public string? Role { get; set; }

    }
}
