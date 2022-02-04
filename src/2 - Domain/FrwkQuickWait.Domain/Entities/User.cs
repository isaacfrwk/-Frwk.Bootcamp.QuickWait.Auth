using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FrwkQuickWait.Domain.Entities
{
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        [ValidateNever]
        public string? Role { get; set; }
        public string? Email { get; set; }
        public string? CPF { get; set; }
        public string? CNPJ { get; set; }
    }
}
