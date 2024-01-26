using Contracts.PurchaseOrderFeatures;
using Infrastructure.DbContexts;

namespace Features.PurchaseOrderFeatures;

public class PurchaseOrderServices : IPurchaseOrderServices
{
    private readonly ApplicationDbContext _context;

    public PurchaseOrderServices(ApplicationDbContext context)
    {
        _context = context;
    }
}
