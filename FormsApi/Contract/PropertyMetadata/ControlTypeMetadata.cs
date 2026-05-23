using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.PropertyMetadata;

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


public sealed record ControlTypeMetadata : PropertyMetadata
{
    [Required]
    public ControlType Value { get; init; }
}
