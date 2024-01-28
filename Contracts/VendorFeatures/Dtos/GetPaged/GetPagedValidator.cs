using FluentValidation;

namespace Contracts.VendorFeatures.Dtos.GetPaged;

public class GetPagedValidator : BaseValidator<GetVendorsPagedRequest>
{
    public GetPagedValidator()
    {
        RuleFor(x => x.Length)
            .NotEmpty()
            .GreaterThan(0);
    }
}
