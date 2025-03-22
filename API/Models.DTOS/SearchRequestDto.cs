namespace Wego.API.Models.DTOS
{
    public class SearchRequestDto
    {
        public string Query { get; set; } = string.Empty;
        public string SortBy { get; set; } = "Id";
        public string SortDir { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
