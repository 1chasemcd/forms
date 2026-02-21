namespace FormsApi.Repository;

public readonly record struct RepositoryType(string TypeId)
{
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
