namespace Hng.Application.Shared.Dtos
{
    public class PaginatedResponseDto<T>
    {
        public string Message { get; set; } = "Request completed successfully.";
        public PagedListMetadataDto Metadata { get; set; }
        public T Data { get; set; }
    }
}
