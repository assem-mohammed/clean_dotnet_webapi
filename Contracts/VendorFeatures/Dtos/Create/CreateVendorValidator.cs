using FluentValidation;

namespace Contracts.VendorFeatures.Dtos.Create;

public class CreateVendorValidator : BaseValidator<CreateVendorRequest>
{
    public CreateVendorValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .Length(10);

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress()
            .MaximumLength(241);

        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .MaximumLength(35);

        RuleFor(x => x.FirstSearchTerm)
            .NotEmpty()
            .NotNull()
            .MaximumLength(10);

        RuleFor(x => x.Language)
            .NotEmpty()
            .NotNull()
            .Length(2);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .NotNull()
            .MaximumLength(16);
    }
}
