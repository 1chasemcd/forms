using FormsApi.Builder;
using FormsApi.Common.Types;
using FormsApi.Recalculate;

namespace Tests.Builder;

public class TestFormBuilder : Form<TestModel>
{
    protected override ViewBuilder<TestModel> View => new CombinedViewBuilder<TestModel>("Title", 100)
    {
        DataView(),
        DataViewWithAugments(),
        SubPropertyGridView()
    };

    private static FieldViewBuilder<TestModel> DataView()
    {
        return new FieldViewBuilder<TestModel>() {
            { m => m.BoolProperty },
            { m => m.CurrencyProperty },
            { m => m.DateProperty },
            { m => m.DecimalProperty },
            { m => m.IntProperty },
            { m => m.TextAreaProperty },
            { m => m.StringProperty },
            { m => m.TimeProperty },

            { m => m.Button, p => p.AddRecalc<TestService>(s => s.PerformAction) },
            { m => m.StaticText }
        };
    }

    private static FieldViewBuilder<TestModel> DataViewWithAugments()
    {
        return new FieldViewBuilder<TestModel>() {
            { m => m.BoolProperty, p => p.Width = 6 },
            { m => m.CurrencyProperty, p => p.Enabled = false },
            { m => m.DateProperty, p => p.MaxValue = new DateOnly(2025, 01, 01) },

            { m => m.DecimalProperty, p => p.Precision = 4 },
            { m => m.IntProperty, p => p.MinValue = Property(m => m.MinValueProperty) },
            { m => m.TextAreaProperty, p => p.Visible = false },
            { m => m.StringProperty, p => p.Label = "Test Label" },
        };
    }

    private static SubPropertyGridViewBuilder<TestModel, TestModel.TestModelChild> SubPropertyGridView()
    {
        return new SubPropertyGridViewBuilder<TestModel, TestModel.TestModelChild>(m => m.EnumerableProperty, r => r.Property1)
        {
            m => m.Property1,
            m => m.Property2
        };
    }
}

public class TestModel
{
    public string StringProperty { get; set; } = "";
    public required TextArea TextAreaProperty { get; set; }
    public required Currency CurrencyProperty { get; set; }
    public decimal DecimalProperty { get; set; }
    public int IntProperty { get; set; }
    public bool BoolProperty { get; set; }
    public DateOnly DateProperty { get; set; }
    public TimeOnly TimeProperty { get; set; }
    public Button? Button { get; set; }
    public LabelValue StaticText => "Static Text";
    public int DisabledProperty { get; }
    internal int InternalProperty { get; set; }
    private int _noGetterField;
    public int NoGetterProperty { set => _noGetterField = value; }
    public int IntField;
    public int MinValueProperty { get; }
    public IEnumerable<TestModelChild> EnumerableProperty { get; set; } = [];
    public class TestModelChild
    {
        public int Property1 { get; set; }
        public int Property2 { get; set; }
    }
}

public class TestService
{
    public PostRecalculateEvent? PerformAction(TestModel model) => null;

}
