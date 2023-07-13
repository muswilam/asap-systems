namespace AsapSystems.Core.Entities
{
    public class Address : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string? BuildingNumber { get; set; }

        public string? ApartmentNumber { get; set; }

        public int AddressTypeId { get; set; }

        public int PersonId { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // nav props.
        public AddressType AddressType { get; set; }

        public Person Person { get; set; }
    }
}