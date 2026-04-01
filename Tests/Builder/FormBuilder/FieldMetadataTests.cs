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

        using (Assert.EnterMultipleScope())
        {
            AssertMetadataHasValue(fields, nameof(TestModel.BoolProperty), MetadataType.Width, new NumericSize(6));
            AssertMetadataHasValue(fields, nameof(TestModel.CurrencyProperty), MetadataType.Enabled, new Constant(false));
            AssertMetadataHasValue(fields, nameof(TestModel.DateProperty), MetadataType.MaxValue, new Constant(new DateOnly(2025, 01, 01)));
            AssertMetadataHasValue(fields, nameof(TestModel.DecimalProperty), MetadataType.Precision, new Constant(4));
            AssertMetadataHasValue(fields, nameof(TestModel.IntProperty), MetadataType.MinValue, new Property(nameof(TestModel.MinValueProperty)));
            AssertMetadataHasValue(fields, nameof(TestModel.TextAreaProperty), MetadataType.Visible, new Constant(false));
            AssertMetadataHasValue(fields, nameof(TestModel.StringProperty), MetadataType.Label, new Constant("Test Label"));
        }
    }

    private static void AssertMetadataHasValue(List<FieldDefinition> fields, string inputName, MetadataType metadataType, object value)
    {
        FieldDefinition? field = fields.SingleOrDefault(x => x.Property == inputName);
        MetadataDefinition? metadata = field?.FieldMetadatas?.SingleOrDefault(x => x.Type == metadataType);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(field, Is.Not.Null);
            Assert.That(metadata, Is.Not.Null);
            Assert.That(metadata?.Value, Is.EqualTo(value));
        }

    }

}
