namespace AsapSystems.Core.Entities
{
    public class Address : BaseEntity
    {
        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string BuildingNumber { get; set; } = string.Empty;

        public string ApartmentNumber { get; set; } = string.Empty;
    }
}