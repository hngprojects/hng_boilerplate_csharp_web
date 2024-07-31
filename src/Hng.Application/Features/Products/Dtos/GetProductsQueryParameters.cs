using System.Text.Json.Serialization;

namespace Hng.Application.Features.Products.Dtos
{
	public class GetProductsQueryParameters
	{
		[JsonPropertyName("page_size")]
		public int PageSize { get; set; }
		[JsonPropertyName("page_number")]
		public int PageNumber { get; set; }
	}
}
