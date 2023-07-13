namespace AsapSystems.BLL.Dtos.Addresses;

public class NewAddressDto
{
    public int PersonId { get; set; }

    public int AddressTypeId { get; set; }

    public string Name { get; set; }

    public string Country { get; set; }

    public string City { get; set; }
    
    public string Street { get; set; }

    public string? BuildingNumber { get; set; }

    public string? ApartmentNumber { get; set; }
}
