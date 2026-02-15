namespace FormsApi.Repository.Query;

public abstract class QueryPart { }

public sealed class And : QueryPart
{
    public required IEnumerable<QueryPart> Parts { get; init; }
}

public sealed class Or : QueryPart
{
    public required IEnumerable<QueryPart> Parts { get; init; }
}

public sealed class Expression : QueryPart
{
    public required string Property { get; init; }
    public required QueryOperator Operator { get; init; }
    public required object Value { get; init; }
}
