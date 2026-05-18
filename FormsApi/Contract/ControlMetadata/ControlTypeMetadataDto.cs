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


public sealed class ControlTypeMetadataDto : IControlMetadataDto
{
    [Required]
    public ControlType Value { get; init; }
}
