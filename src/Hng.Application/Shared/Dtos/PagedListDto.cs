namespace Hng.Application.Shared.Dtos
{
    public class PagedListDto<T> : List<T>
    {
        public PagedListMetadataDto MetaData { get; set; }
        public PagedListDto(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new PagedListMetadataDto
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            AddRange(items);
        }

        public static PagedListDto<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count(); var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedListDto<T>(items, count, pageNumber, pageSize);
        }
    }
}
