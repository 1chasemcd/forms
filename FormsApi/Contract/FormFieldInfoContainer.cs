using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract;

public sealed record FormFieldInfoContainer
{
    public FormFieldInfoContainer(string identifier, int? width = null)
    {
        Identifier = identifier;
        Width = width;
    }
    [Required]
    public string Identifier { get; init; }
    public int? Width { get; init; }
}
