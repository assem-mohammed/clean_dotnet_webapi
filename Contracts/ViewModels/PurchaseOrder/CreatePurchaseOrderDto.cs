using FluentValidation;

namespace Contracts.ViewModels.PurchaseOrder;

public class CreatePurchaseOrderDto
{
    public string Id { get; set; } = default!;
    public string VendorCode { get; set; } = default!;
    public int Index { get; set; }
}

public class CreatePurchaseOrderDtoValidator : BaseValidator<CreatePurchaseOrderDto>
{
    public CreatePurchaseOrderDtoValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.VendorCode).NotNull().NotEmpty();
        RuleFor(x => x.Index).NotNull().GreaterThan(0);
    }
}
