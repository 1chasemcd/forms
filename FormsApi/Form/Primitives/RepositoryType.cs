namespace FormsApi.Form.Primitives;

public readonly record struct RepositoryType(string TypeId)
{
    public override string ToString() => TypeId;
    public static bool TryParse(string? value, out RepositoryType result)
    {
        if (!string.IsNullOrEmpty(value))
        {
            result = new RepositoryType(value);
            return true;
        }

        result = default;
        return false;
    }
}
