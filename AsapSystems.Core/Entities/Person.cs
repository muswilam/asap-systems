namespace AsapSystems.Core.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int GenderId { get; set; }

        public DateTime CreateDate { get; set; }

        // nav props.
        public Gender Gender { get; set; }

        public ICollection<Address>? Addresses { get; set; }
    }
}