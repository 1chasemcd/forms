
namespace FormsApi.Repository.Query;

public sealed class QueryCriteria
{
    public QueryPart? Filter { get; init; }
    public IEnumerable<string>? OrderBy { get; set; }
}
