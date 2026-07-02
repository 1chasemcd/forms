using FormsApi.Contract;
using FormsApi.Contract.View;
using FormsApi.Forms;

namespace Tests.Forms;

[TestFixture]
public class SubPropertyTableViewTests
{
    private sealed class TestModel
    {
        public bool BoolProperty1 { get; set; }
        public bool BoolProperty2 { get; set; }
        public bool BoolProperty3 { get; set; }

        public IEnumerable<TestTableRowModel> Rows { get; set; } = [];
    }
    private sealed class TestTableRowModel
    {
        public int Id { get; set; }
        public bool TableRowBoolProperty1 { get; set; }
        public bool TableRowBoolProperty2 { get; set; }
        public bool TableRowBoolProperty3 { get; set; }
    }

    private static SubPropertyTableViewBuilder<TestModel, TestTableRowModel> BasicTable => new(x => x.Rows, x => x.Id);

    [Test]
    public void WithFields_SetsFields()
    {
        var view = new SubPropertyTableViewBuilder<TestModel, TestTableRowModel>(x => x.Rows, x => x.Id)
        {
            {m => m.TableRowBoolProperty1, 6}
        };

        Assert.That(view.FieldList.Single(),
            Has.Property(nameof(FormFieldInfoContainer.Identifier))
            .EqualTo(nameof(TestTableRowModel.TableRowBoolProperty1))
            .And.Property(nameof(FormFieldInfoContainer.Width))
            .EqualTo(6));
    }

    [Test]
    public void EnableAdd_SetsCanAddToTrue()
    {
        SubPropertyTableViewBuilder<TestModel, TestTableRowModel> table = BasicTable.EnableAdd();
        Assert.That(table.CanAdd.InnerValue(), Is.True);
    }

    [Test]
    public void CanAddWhen_SetsCanAdd()
    {
        SubPropertyTableViewBuilder<TestModel, TestTableRowModel> table = BasicTable.CanAddWhen(m => m.BoolProperty1);
        Assert.That(table.CanAdd.InnerValue(), Is.EqualTo(nameof(TestModel.BoolProperty1)));
    }

    [Test]
    public void EnableEdit_SetsCanEditToTrue()
    {
        SubPropertyTableViewBuilder<TestModel, TestTableRowModel> table = BasicTable.EnableEdit();
        Assert.That(table.CanEdit.InnerValue(), Is.True);
    }

    [Test]
    public void CanEditWhen_SetsCanEdit()
    {
        SubPropertyTableViewBuilder<TestModel, TestTableRowModel> table = BasicTable.CanEditWhen(m => m.BoolProperty1);
        Assert.That(table.CanEdit.InnerValue(), Is.EqualTo(nameof(TestModel.BoolProperty1)));
    }

    [Test]
    public void EnableEditRow_SetsCanEditRowToTrue()
    {
        SubPropertyTableViewBuilder<TestModel, TestTableRowModel> table = BasicTable.CanEditRowWhen(m => m.TableRowBoolProperty1);
        Assert.That(table.CanEditRow.InnerValue(), Is.EqualTo(nameof(TestTableRowModel.TableRowBoolProperty1)));
    }

    [Test]
    public void EnableDelete_SetsCanDeleteToTrue()
    {
        SubPropertyTableViewBuilder<TestModel, TestTableRowModel> table = BasicTable.EnableDelete();
        Assert.That(table.CanDelete.InnerValue(), Is.True);
    }

    [Test]
    public void CanDeleteWhen_SetsCanDelete()
    {
        SubPropertyTableViewBuilder<TestModel, TestTableRowModel> table = BasicTable.CanDeleteWhen(m => m.BoolProperty1);
        Assert.That(table.CanDelete.InnerValue(), Is.EqualTo(nameof(TestModel.BoolProperty1)));
    }

    [Test]
    public void EnableDeleteRow_SetsCanDeleteRowToTrue()
    {
        SubPropertyTableViewBuilder<TestModel, TestTableRowModel> table = BasicTable.CanDeleteRowWhen(m => m.TableRowBoolProperty1);
        Assert.That(table.CanDeleteRow.InnerValue(), Is.EqualTo(nameof(TestTableRowModel.TableRowBoolProperty1)));
    }

    [Test]
    public void EnableSelection_SetsSelectionProperties()
    {
        SubPropertyTableViewBuilder<TestModel, TestTableRowModel> table = BasicTable
            .EnableSelection(m => m.TableRowBoolProperty1, TableSelectionType.Single);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(table.SelectionProperty, Is.EqualTo(nameof(TestTableRowModel.TableRowBoolProperty1)));
            Assert.That(table.SelectionType, Is.EqualTo(TableSelectionType.Single));
        }

    }
}
