namespace Hng.Application.Features.ApiStatuses.Dtos.Requests
{
    public class ApiStatusWrapper
    {
        public Dictionary<string, List<ApiStatusModel>> Collection { get; set; }
    }
}