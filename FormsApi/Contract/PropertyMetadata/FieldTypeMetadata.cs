using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.PropertyMetadata;

public enum FieldType
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


public sealed record FieldTypeMetadata : PropertyMetadata
{
    [Required]
    public FieldType Value { get; init; }
}
