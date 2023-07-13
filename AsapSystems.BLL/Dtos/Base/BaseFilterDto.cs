namespace AsapSystems.BLL.Dtos.Base
{
    public class BaseFilterDto
    {
        public string? Search { get; set; }

        /// <summary>
        /// Page Number. Default 0.
        /// </summary>
        public int PageNumber { get; set; } = 0;

        /// <summary>
        /// Number of items in page. Default 10.
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}