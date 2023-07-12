namespace AsapSystems.Core.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }

        public int GenderId { get; set; }

        // nav props.
        public Gender Gender { get; set; }

        public ICollection<Address>? Addresses { get; set; }
    }
}