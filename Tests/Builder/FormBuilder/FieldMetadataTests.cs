using FormsApi.Contract;
using FormsApi.Contract.View;

namespace Tests.Builder.FormBuilder;

public class FieldMetadataTests
{
    private readonly FormDto _form = new TestFormBuilder().Build();

    [Test]
    public void FieldMetadatas_AreAppliedCorrectly()
    {
        List<FormControlLayoutDto> fields = ((CombinedViewDto)_form.View).Views
            .Select(x => x as FieldViewDto).Last(x => x != null)?.Fields.ToList()!;

        using (Assert.EnterMultipleScope())
        {
            AssertMetadataHasValue(fields, nameof(TestModel.BoolProperty), MetadataType.Width, 6);
            AssertMetadataHasValue(fields, nameof(TestModel.CurrencyProperty), MetadataType.Enabled, new ConstantDto(false));
            AssertMetadataHasValue(fields, nameof(TestModel.DateProperty), MetadataType.MaxValue, new ConstantDto(new DateOnly(2025, 01, 01)));
            AssertMetadataHasValue(fields, nameof(TestModel.DecimalProperty), MetadataType.Precision, new ConstantDto(4));
            AssertMetadataHasValue(fields, nameof(TestModel.IntProperty), MetadataType.MinValue, new PropertyDto(nameof(TestModel.MinValueProperty)));
            AssertMetadataHasValue(fields, nameof(TestModel.TextAreaProperty), MetadataType.Visible, new ConstantDto(false));
            AssertMetadataHasValue(fields, nameof(TestModel.StringProperty), MetadataType.Label, new ConstantDto("Test Label"));
        }
    }

    private static void AssertMetadataHasValue(List<FormControlLayoutDto> fields, string inputName, MetadataType metadataType, object value)
    {
        FormControlLayoutDto? field = fields.SingleOrDefault(x => x.Property == inputName);
        MetadataDefinition? metadata = field?.FieldMetadatas?.SingleOrDefault(x => x.Type == metadataType);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(field, Is.Not.Null);
            Assert.That(metadata, Is.Not.Null);
            Assert.That(metadata?.Value, Is.EqualTo(value));
        }

    }

}
