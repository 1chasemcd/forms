using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ControlMetadata;

public enum ControlType
{
    Button,
    CheckBox,
    Currency,
    Date,
    Time,
    Numeric,
    TextArea,
    Text,
    LabelValue,
}


public sealed record ControlTypeMetadataDto : BaseControlMetadataDto
{
    [Required]
    public ControlType Value { get; init; }
}
