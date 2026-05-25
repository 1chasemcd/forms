using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract;

public sealed record FormControlInfoContainer
{
    public FormControlInfoContainer(string propertyName, int? width = null)
    {
        PropertyName = propertyName;
        Width = width;
    }
    [Required]
    public string PropertyName { get; init; }
    public int? Width { get; init; }
}
