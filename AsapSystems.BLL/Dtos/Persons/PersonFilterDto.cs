using AsapSystems.BLL.Dtos.Base;

namespace AsapSystems.BLL.Dtos.Persons
{
    public class PersonFilterDto : BaseFilterDto
    {
        public int? GenderId { get; set; }
    }
}