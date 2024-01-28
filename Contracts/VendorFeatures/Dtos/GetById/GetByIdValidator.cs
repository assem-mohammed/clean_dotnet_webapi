using FluentValidation;

namespace Contracts.VendorFeatures.Dtos.GetById;

public class GetByIdValidator : BaseValidator<GetVendorByIdRequest>
{
    public GetByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .Length(10);
    }
}
