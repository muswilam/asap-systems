namespace AsapSystems.BLL.Dtos.Base
{
    public class Paged<T>
    {
        public T Items { get; set; }

        public int TotalCount { get; set; }
    }
}