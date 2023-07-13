namespace AsapSystems.BLL.Dtos.Addresses;

public class AddressDetailsDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string BuildingNumber { get; set; }

    public string ApartmentNumber { get; set; }

    public string AddressTypeName { get; set; }
}
