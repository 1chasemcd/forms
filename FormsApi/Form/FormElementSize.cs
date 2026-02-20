namespace FormsApi.Form;

public readonly record struct FormElementSize(int Size = 100)
{
    public static implicit operator FormElementSize(int size)
        => new(size);

    public static readonly FormElementSize Default = new();
}
