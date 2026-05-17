using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ControlMetadata;

public enum InputType
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


public sealed class InputTypeMetadataDto : IInputMetadataDto
{
    [Required]
    public InputType Value { get; init; }
}
