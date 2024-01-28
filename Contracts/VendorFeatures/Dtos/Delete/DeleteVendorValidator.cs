using FluentValidation;

namespace Contracts.VendorFeatures.Dtos.Delete;

public class DeleteVendorValidator : BaseValidator<DeleteVendorRequest>
{
    public DeleteVendorValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .Length(10);
    }
}
