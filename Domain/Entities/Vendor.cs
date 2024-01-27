using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Vendor : BaseEntity<string>
    {
        //[MaxLength(ModelConstants.LengthOf20)]
        public string? LegacyVendorCode { get; set; }

        //[MaxLength(ModelConstants.LengthOf4)]
        public string? Group { get; set; }

        //[MaxLength(ModelConstants.LengthOf15)]
        public string? Title { get; set; }

        //[Required, MaxLength(ModelConstants.LengthOf35)]
        public string Name { get; set; } = default!;

        //[MaxLength(ModelConstants.LengthOf35)]
        public string? Name2 { get; set; }

        //[MaxLength(ModelConstants.LengthOf35)]
        public string? Name3 { get; set; }

        //[MaxLength(ModelConstants.LengthOf35)]
        public string? Name4 { get; set; }

        //[Required, MaxLength(ModelConstants.LengthOf10)]
        public string FirstSearchTerm { get; set; } = default!;

        //[MaxLength(ModelConstants.LengthOf20)]
        public string? SecondSearchTerm { get; set; }

        //[Required, MaxLength(ModelConstants.LengthOf2)]
        public string Language { get; set; } = default!;

        //[MaxLength(ModelConstants.LengthOf16)]
        public string? Telephone { get; set; }

        //[Required, MaxLength(ModelConstants.LengthOf16)]
        public string Phone { get; set; } = default!;

        //[MaxLength(ModelConstants.LengthOf31)]
        public string? FaxNumber { get; set; }

        //[Required, MaxLength(ModelConstants.LengthOf241)]
        public string Email { get; set; } = default!;

        //[MaxLength(ModelConstants.LengthOf20)]
        public string? VatRegNumber { get; set; }

        //[MaxLength(ModelConstants.LengthOf4)]
        public string? IndustryKey { get; set; }

        //[MaxLength(ModelConstants.LengthOf1)]
        public string? CentralBlock { get; set; }

        //[MaxLength(ModelConstants.LengthOf50)]
        public string? SSOUserId { get; set; }
    }
}
