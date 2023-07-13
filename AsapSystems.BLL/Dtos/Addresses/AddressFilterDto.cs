using AsapSystems.BLL.Dtos.Base;

namespace AsapSystems.BLL.Dtos.Addresses;

public class AddressFilterDto : BaseFilterDto
{
    public int PersonId { get; set; }

    public int? AddressTypeId { get; set; }
}
