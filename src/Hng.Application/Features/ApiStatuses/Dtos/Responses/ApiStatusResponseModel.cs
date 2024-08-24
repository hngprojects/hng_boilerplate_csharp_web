using System.IO;

namespace Hng.Application.Features.ApiStatuses.Dtos.Responses
{
    public record ApiStatusResponseModel
    {
        public Collection collection { get; set; }
        public Environment environment { get; set; }
        public Global globals { get; set; }
        public Run run { get; set; }
    }

    public record Collection
    {
        public List<CollectionItem> item { get; set; } = [];
    }

    public record Environment
    {
    }

    public record Global
    {
    }

    public record Run
    {
        public Stat stats { get; set; }
        public Timing timings { get; set; }
        public List<Execution> executions { get; set; } = [];
        public Transfer transfers { get; set; }
        public List<Failure> failures { get; set; } = [];
        public string error { get; set; }
    }

    public record Stat
    {
    }

    public record Timing
    {
    }

    public record Execution
    {
        public Cursor cursor { get; set; }
        public Item item { get; set; }
        public Request request { get; set; }
        public Response response { get; set; }
        public string id { get; set; }
        public List<Assertion> assertions { get; set; }
    }

    public record Transfer
    {
    }

    public record Failure
    {
        public FailureError error { get; set; }

        public FailureParent parent { get; set; }
    }

    public record Cursor
    {
    }

    public record Item
    {
        public string id { get; set; }
        public string name { get; set; }
        public ItemRequest request { get; set; }
    }

    public record CollectionItem
    {
        public string id { get; set; }
        public string name { get; set; } //group with name API Group
        public List<ItemItem> item { get; set; } = [];
    }

    public record ItemItem
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public record FailureParent
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public record Request
    {
    }

    public record Url
    {
        public List<string> path { get; set; }
    }

    public record ItemRequest
    {
        public Url url { get; set; }
    }

    public record Response
    {
        public string id { get; set; }
        public string status { get; set; }
        public long code { get; set; }
        public long responseTime { get; set; }
    }

    public record Assertion
    {
    }

    public record FailureError
    {
    }
}
