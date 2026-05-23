using FormsApi.Contract;
using FormsApi.Contract.View;
using FormsApi.Forms;

namespace Tests.Forms;

[TestFixture]
public class SubPropertyGridViewTests
{
    private sealed class TestModel
    {
        public bool BoolProperty1 { get; set; }
        public bool BoolProperty2 { get; set; }
        public bool BoolProperty3 { get; set; }

        public IEnumerable<TestGridRowModel> Rows { get; set; } = [];
    }
    private sealed class TestGridRowModel
    {
        public int Id { get; set; }
        public bool GridRowBoolProperty1 { get; set; }
        public bool GridRowBoolProperty2 { get; set; }
        public bool GridRowBoolProperty3 { get; set; }
    }

    private static SubPropertyGridViewBuilder<TestModel, TestGridRowModel> BasicGrid => new(x => x.Rows, x => x.Id);

    [Test]
    public void WithFields_SetsFields()
    {
        var view = new SubPropertyGridViewBuilder<TestModel, TestGridRowModel>(x => x.Rows, x => x.Id)
        {
            {m => m.GridRowBoolProperty1, 6}
        };

        Assert.That(view.ControlList.Single(),
            Has.Property(nameof(FormControl.PropertyName))
            .EqualTo(nameof(TestGridRowModel.GridRowBoolProperty1))
            .And.Property(nameof(FormControl.Width))
            .EqualTo(6));
    }

    [Test]
    public void EnableAdd_SetsCanAddToTrue()
    {
        SubPropertyGridViewBuilder<TestModel, TestGridRowModel> grid = BasicGrid.EnableAdd();
        Assert.That(grid.CanAdd.InnerValue(), Is.True);
    }

    [Test]
    public void CanAddWhen_SetsCanAdd()
    {
        SubPropertyGridViewBuilder<TestModel, TestGridRowModel> grid = BasicGrid.CanAddWhen(m => m.BoolProperty1);
        Assert.That(grid.CanAdd.InnerValue(), Is.EqualTo(nameof(TestModel.BoolProperty1)));
    }

    [Test]
    public void EnableEdit_SetsCanEditToTrue()
    {
        SubPropertyGridViewBuilder<TestModel, TestGridRowModel> grid = BasicGrid.EnableEdit();
        Assert.That(grid.CanEdit.InnerValue(), Is.True);
    }

    [Test]
    public void CanEditWhen_SetsCanEdit()
    {
        SubPropertyGridViewBuilder<TestModel, TestGridRowModel> grid = BasicGrid.CanEditWhen(m => m.BoolProperty1);
        Assert.That(grid.CanEdit.InnerValue(), Is.EqualTo(nameof(TestModel.BoolProperty1)));
    }

    [Test]
    public void EnableEditRow_SetsCanEditRowToTrue()
    {
        SubPropertyGridViewBuilder<TestModel, TestGridRowModel> grid = BasicGrid.CanEditRowWhen(m => m.GridRowBoolProperty1);
        Assert.That(grid.CanEditRow.InnerValue(), Is.EqualTo(nameof(TestGridRowModel.GridRowBoolProperty1)));
    }

    [Test]
    public void EnableDelete_SetsCanDeleteToTrue()
    {
        SubPropertyGridViewBuilder<TestModel, TestGridRowModel> grid = BasicGrid.EnableDelete();
        Assert.That(grid.CanDelete.InnerValue(), Is.True);
    }

    [Test]
    public void CanDeleteWhen_SetsCanDelete()
    {
        SubPropertyGridViewBuilder<TestModel, TestGridRowModel> grid = BasicGrid.CanDeleteWhen(m => m.BoolProperty1);
        Assert.That(grid.CanDelete.InnerValue(), Is.EqualTo(nameof(TestModel.BoolProperty1)));
    }

    [Test]
    public void EnableDeleteRow_SetsCanDeleteRowToTrue()
    {
        SubPropertyGridViewBuilder<TestModel, TestGridRowModel> grid = BasicGrid.CanDeleteRowWhen(m => m.GridRowBoolProperty1);
        Assert.That(grid.CanDeleteRow.InnerValue(), Is.EqualTo(nameof(TestGridRowModel.GridRowBoolProperty1)));
    }

    [Test]
    public void EnableSelection_SetsSelectionProperties()
    {
        SubPropertyGridViewBuilder<TestModel, TestGridRowModel> grid = BasicGrid
            .EnableSelection(m => m.GridRowBoolProperty1, GridSelectionType.Single);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(grid.SelectionProperty, Is.EqualTo(nameof(TestGridRowModel.GridRowBoolProperty1)));
            Assert.That(grid.SelectionType, Is.EqualTo(GridSelectionType.Single));
        }

    }
}
