using FluentValidation;
using FrwkQuickWait.Domain.Entities;

namespace FrwkQuickWait.Service.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("valor não pode está vazio.")
                .NotNull().WithMessage("Valor não pode ser nulo.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("valor não pode está vazio.")
                .NotNull().WithMessage("Valor não pode ser nulo.");
        }
    }
}
