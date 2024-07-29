namespace Hng.Application.Shared.Dtos
{
    public class PagedListMetadataDto
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool? HasNext => CurrentPage < TotalPages;
    }
}
