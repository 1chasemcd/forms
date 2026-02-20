using FormsApi.Builder;
using FormsApi.Builder.View;
using FormsApi.Common.Types;
using FormsApi.Form;
using FormsApi.Form.Field;
using FormsApi.Form.View;
using FormsApi.Repository;
using NUnit.Framework;

namespace Tests.Builder;

public class FormBuilderTests
{
    private FormModel? _form;
    [OneTimeSetUp]
    public void BuildForm()
    {
        _form = new TestFormBuilder().Build();
    }

    [Test]
    public void Form_Type()
    {
        var expectedType = new RepositoryType(typeof(TestModel));
        Assert.That(_form.Type, Is.EqualTo(expectedType));
    }

    [Test]
    public void Form_View()
    {
        Assert.That(_form.View, Is.InstanceOf<CombinedView>());
    }

    [Test]
    public void View_Title()
    {
        Assert.That(_form.View.Title, Is.EqualTo(new Constant<string>("Title")));
    }

    public void DataView_MaintainsFieldOrder(string propertyName, int expectedIndex)
    {
        var fields = ((CombinedView)_form.View).Views
                    .Select(x => x as DataView).Where(x => x != null).ToList()[0]!.Fields.ToList();

        Assert.That(fields, Is.Not.Null);
        Assert.That(fields, Has.ItemAt(expectedIndex)
            .With.Property(nameof(BaseInput.Property)).EqualTo(propertyName));
    }

    [TestCase(nameof(TestModel.BoolProperty), typeof(CheckBoxInput))]
    [TestCase(nameof(TestModel.CurrencyProperty), typeof(CurrencyInput))]
    [TestCase(nameof(TestModel.DateProperty), typeof(DateInput))]
    [TestCase(nameof(TestModel.DecimalProperty), typeof(NumericInput))]
    [TestCase(nameof(TestModel.IntProperty), typeof(NumericInput))]
    [TestCase(nameof(TestModel.StringListProperty), typeof(TextAreaInput))]
    [TestCase(nameof(TestModel.StringProperty), typeof(TextInput))]
    [TestCase(nameof(TestModel.TimeProperty), typeof(TimeInput))]
    public void DataView_InputFieldTypesMappedCorrectly(string inputName, Type expectedInputType)
    {
        var fields = ((CombinedView)_form.View).Views
            .Select(x => x as DataView).Where(x => x != null).ToList()[0]?.Fields.ToList()!;

        Assert.That(fields, Has.One.With.InstanceOf(expectedInputType)
            .With.Property(nameof(BaseInput.Property)).EqualTo(inputName));
    }

    [Test]
    public void DataView_ButtonField()
    {
        var fields = ((CombinedView)_form.View).Views
            .Select(x => x as DataView).First(x => x != null)?.Fields.ToList()!;


        Assert.That(fields, Has.One.With.InstanceOf<Button>()
            .With.Property(nameof(Button.MethodToRunOnChange))
            .EqualTo(nameof(TestModel.ButtonAction)));
    }

    [Test]
    public void DataView_FieldAugments()
    {
        var fields = ((CombinedView)_form.View).Views
            .Select(x => x as DataView).Last(x => x != null)?.Fields.ToList()!;

        AssertAugmentHasValue(nameof(TestModel.BoolProperty), nameof(CheckBoxInput.Width), new FormElementSize(50));
        AssertAugmentHasValue(nameof(TestModel.CurrencyProperty), nameof(CurrencyInput.Disabled), new Constant<bool>(true));
        AssertAugmentHasValue(nameof(TestModel.DateProperty), nameof(DateInput.MaxValue), new Constant<DateOnly>(new DateOnly(2025, 01, 01)));
        AssertAugmentHasValue(nameof(TestModel.DecimalProperty), nameof(NumericInput.Precision), new Constant<int>(4));
        AssertAugmentHasValue(nameof(TestModel.IntProperty), nameof(NumericInput.MinValue), new Property<int>(nameof(TestModel.MinValueProperty)));
        AssertAugmentHasValue(nameof(TestModel.StringListProperty), nameof(TextAreaInput.Hidden), new Constant<bool>(true));
        AssertAugmentHasValue(nameof(TestModel.StringProperty), nameof(TextInput.Label), new Constant<string>("Test Label"));

        Assert.That(fields, Has.One.With
            .Property(nameof(BaseInput.Property)).EqualTo(nameof(TestModel.TimeProperty))
            .And.With.Property(nameof(TimeInput.PropertiesToUpdateOnChange)).EquivalentTo(new List<string>() { nameof(TestModel.BoolProperty) }));

        void AssertAugmentHasValue(string inputName, string augmentName, object value) => Assert.That(fields, Has.One.With
            .Property(nameof(BaseInput.Property)).EqualTo(inputName)
            .And.With.Property(augmentName).EqualTo(value));
    }

    private class TestFormBuilder : FormBuilder<TestModel>
    {
        protected override ViewBuilder<TestModel> View => new CombinedViewBuilder<TestModel>("Title", 100)
        {
            DataView(),
            DataViewWithAugments()
        };

        private DataViewBuilder<TestModel> DataView()
        {
            return new DataViewBuilder<TestModel>() {
                { m => m.BoolProperty },
                { m => m.CurrencyProperty },
                { m => m.DateProperty },
                { m => m.DecimalProperty },
                { m => m.IntProperty },
                { m => m.StringListProperty },
                { m => m.StringProperty },
                { m => m.TimeProperty },

                { m => m.ButtonAction },
                { "Static Text" },
            };
        }

        private DataViewBuilder<TestModel> DataViewWithAugments()
        {
            return new DataViewBuilder<TestModel>() {
                { m => m.BoolProperty, p => p.Width = 50 },
                { m => m.CurrencyProperty, p => p.Disabled = true },
                { m => m.DateProperty, p => p.MaxValue = new DateOnly(2025, 01, 01)},
                { m => m.DecimalProperty, p => p.Precision = 4 },
                { m => m.IntProperty, p => p.MinValue = new PropertyOrConstantBuilder<TestModel, int>(x => x.MinValueProperty) },
                { m => m.StringListProperty, p => p.Hidden = true },
                { m => m.StringProperty, p => p.Label = "Test Label" },
                { m => m.TimeProperty, p => p.PropertiesToUpdateOnChange = [ new ModelMemberBuilder<TestModel, object>(x => x.BoolProperty)] }
            };
        }
    }

    private class TestModel
    {
        public string StringProperty { get; set; } = "";
        public required IList<string> StringListProperty { get; set; }
        public required Currency CurrencyProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public int IntProperty { get; set; }
        public bool BoolProperty { get; set; }
        public DateOnly DateProperty { get; set; }
        public TimeOnly TimeProperty { get; set; }
        public int DisabledProperty { get; }
        internal int InternalProperty { get; set; }
        private int _noGetterField;
        public int NoGetterProperty { set => _noGetterField = value; }
        public int IntField;
        public int MinValueProperty => 20;
        public void ButtonAction() => IntField = 0;
        public IEnumerable<TestModelChild> EnumerableProperty { get; set; } = [];
        public class TestModelChild
        {
            public int Property1 { get; set; }
            public int Property2 { get; set; }
        }
    }
}
