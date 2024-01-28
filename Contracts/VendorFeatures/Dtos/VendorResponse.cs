﻿namespace Contracts.VendorFeatures.Dtos
{
    public record VendorResponse(
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
        string? vatRegNumber);
}
