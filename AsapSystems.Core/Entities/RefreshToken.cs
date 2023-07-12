namespace AsapSystems.Core.Entities
{
    public class RefreshToken : BaseEntity
    {
        public int PersonId { get; set; }
        public string Jti { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // nav props.

        public Person Person { get; set; }
    }
}