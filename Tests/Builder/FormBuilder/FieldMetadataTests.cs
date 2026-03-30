using System.Threading.RateLimiting;
using FormsApi.Definition;
using FormsApi.Definition.Field;
using FormsApi.Definition.Metadata;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace Tests.Builder.FormBuilder;

public class FieldMetadataTests
{
    private readonly FormDefinition _form = new TestFormBuilder().Build();

    [Test]
    public void FieldMetadatas_AreAppliedCorrectly()
    {
        List<FieldDefinition> fields = ((CombinedViewDefinition)_form.View).Views
            .Select(x => x as FieldViewDefinition).Last(x => x != null)?.Fields.ToList()!;

        AssertMetadataHasValue(fields, nameof(TestModel.BoolProperty), typeof(WidthMetadata), nameof(WidthMetadata.Width), new NumericSize(6));
        AssertMetadataHasValue(fields, nameof(TestModel.CurrencyProperty), typeof(EnabledMetadata), nameof(EnabledMetadata.Enabled), new Constant(false));
        AssertMetadataHasValue(fields, nameof(TestModel.DateProperty), typeof(ValueRangeMetadata), nameof(ValueRangeMetadata.MaxValue), new Constant(new DateOnly(2025, 01, 01)));
        AssertMetadataHasValue(fields, nameof(TestModel.DecimalProperty), typeof(PrecisionScaleMetadata), nameof(PrecisionScaleMetadata.Precision), new Constant(4));
        AssertMetadataHasValue(fields, nameof(TestModel.IntProperty), typeof(ValueRangeMetadata), nameof(ValueRangeMetadata.MinValue), new Property(nameof(TestModel.MinValueProperty)));
        AssertMetadataHasValue(fields, nameof(TestModel.TextAreaProperty), typeof(VisibleMetadata), nameof(VisibleMetadata.Visible), new Constant(false));
        AssertMetadataHasValue(fields, nameof(TestModel.StringProperty), typeof(LabelMetadata), nameof(LabelMetadata.Label), new Constant("Test Label"));
    }

    private static void AssertMetadataHasValue(List<FieldDefinition> fields, string inputName, Type metadataType, string metadataPropertyName, object value)
    {
        FieldDefinition? field = fields.SingleOrDefault(x => x.Property == inputName);
        BaseMetadataDefinition? metadata = field?.FieldMetadatas?.SingleOrDefault(x => x.GetType() == metadataType);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(field, Is.Not.Null);
            Assert.That(metadata, Is.Not.Null);
            Assert.That(metadata, Has.Property(metadataPropertyName).EqualTo(value));
        }

    }

}
