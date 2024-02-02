namespace Contracts.VendorFeatures.Dtos;

public class VendorResponse
{
	public VendorResponse(
    string id,
    string name,
    string email,
    string phone,
    string language,
    string firstSearchTerm,
    DateTime dateCreated,
    string modifiedBy,
    string? centralBlock,
    DateTime? dateRemoved,
    DateTime? dateUpdated,
    string? faxNumber,
    string? secondSearchTerm,
    string? group,
    string? industryKey,
    bool? isActive,
    bool? isDeleted,
    string? legacyVendorCode,
    string? name2,
    string? name3,
    string? name4,
    string? sSOUserId,
    string? telephone,
    string? title,
    string? vatRegNumber)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        Language = language;
        FirstSearchTerm = firstSearchTerm;
        DateCreated = dateCreated;
        ModifiedBy = modifiedBy;
        CentralBlock = centralBlock;
        DateRemoved = dateRemoved;
        DateUpdated = dateUpdated;
        FaxNumber = faxNumber;
        SecondSearchTerm = secondSearchTerm;
        Group = group;
        IndustryKey = industryKey;
        IsActive = isActive;
        IsDeleted = isDeleted;
        LegacyVendorCode = legacyVendorCode;
        Name2 = name2;
        Name3 = name3;
        Name4 = name4;
        SSOUserId = sSOUserId;
        Telephone = telephone;
        Title = title;
        VatRegNumber = vatRegNumber;
    }

    public string Id { get; }
    public string Name { get; }
    public string Email { get; }
    public string Phone { get; }
    public string Language { get; }
    public string FirstSearchTerm { get; }
    public DateTime DateCreated { get; }
    public string ModifiedBy { get; }
    public string? CentralBlock { get; }
    public DateTime? DateRemoved { get; }
    public DateTime? DateUpdated { get; }
    public string? FaxNumber { get; }
    public string? SecondSearchTerm { get; }
    public string? Group { get; }
    public string? IndustryKey { get; }
    public bool? IsActive { get; }
    public bool? IsDeleted { get; }
    public string? LegacyVendorCode { get; }
    public string? Name2 { get; }
    public string? Name3 { get; }
    public string? Name4 { get; }
    public string? SSOUserId { get; }
    public string? Telephone { get; }
    public string? Title { get; }
    public string? VatRegNumber { get; }
}
