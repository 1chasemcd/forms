using System.Diagnostics;
using FormsApi.Contract.View;
using FormsApi.Forms;
using FormsApi.Forms.Services;
using Moq;

namespace Tests.Forms;

[TestFixture]
public class FormBuilderServiceTests
{
    public sealed class TestModel
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public List<TableRowModel> SubRows { get; set; } = [];
    }

    public sealed class TableRowModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool Selected { get; set; }
    }

    private sealed class TestForm<TModel>(IViewBuilder<TModel> view) : Form<TModel>
    {
        protected internal override IViewBuilder<TModel> View => view;
    }

    private FormBuilderService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _sut = new FormBuilderService();
    }

    [Test]
    public void BuildFormIntoViews_WithFieldView_MapsCorrectly()
    {
        FieldViewBuilder<TestModel> fieldView = new FieldViewBuilder<TestModel>()
            .WithTitle("Basic Field Info")
            .WithWidth(12)
            .VisibleWhen(m => m.IsActive);

        fieldView.Add(m => m.Name, 6);
        fieldView.Add(m => m.Age, 6);

        var form = new TestForm<TestModel>(fieldView);
        IReadOnlyList<View> result = _sut.BuildFormIntoViews(form);
        Assert.That(result, Has.Count.EqualTo(1));
        var dto = result[0] as FieldView;
        Assert.That(dto, Is.Not.Null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(dto.Width, Is.EqualTo(12));
            Assert.That(dto.Title.InnerValue(), Is.EqualTo("Basic Field Info"));
            Assert.That(dto.Visible.InnerValue(), Is.EqualTo(nameof(TestModel.IsActive)));
            Assert.That(dto.Fields, Has.Count.EqualTo(2));
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(dto.Fields[0].Identifier, Is.EqualTo(nameof(TestModel.Name)));
            Assert.That(dto.Fields[0].Width, Is.EqualTo(6));
            Assert.That(dto.Fields[1].Identifier, Is.EqualTo(nameof(TestModel.Age)));
            Assert.That(dto.Fields[1].Width, Is.EqualTo(6));
        }

    }

    [Test]
    public void BuildFormIntoViews_WithCombinedView_FlattensAndMaintainsIndices()
    {
        FieldViewBuilder<TestModel> subView1 = new FieldViewBuilder<TestModel>().WithTitle("Sub 1");
        subView1.Add(m => m.Name);

        FieldViewBuilder<TestModel> subView2 = new FieldViewBuilder<TestModel>().WithTitle("Sub 2");
        subView2.Add(m => m.Age);

        CombinedViewBuilder<TestModel> combinedView = new CombinedViewBuilder<TestModel>()
            .WithTitle("Main Combined")
            .Unify();
        combinedView.Add(subView1);
        combinedView.Add(subView2);

        var form = new TestForm<TestModel>(combinedView);
        IReadOnlyList<View> result = _sut.BuildFormIntoViews(form);
        Assert.That(result, Has.Count.EqualTo(3));

        var combinedDto = result[0] as CombinedView;
        Assert.That(combinedDto, Is.Not.Null);
        Assert.That(combinedDto.Title.InnerValue(), Is.EqualTo("Main Combined"));
        Assert.That(combinedDto.Unify, Is.True);
        Assert.That(combinedDto.ViewIds, Is.EquivalentTo(new[] { 1, 2 }));

        var sub1Dto = result[1] as FieldView;
        Assert.That(sub1Dto, Is.Not.Null);
        Assert.That(sub1Dto.Title.InnerValue(), Is.EqualTo("Sub 1"));

        var sub2Dto = result[2] as FieldView;
        Assert.That(sub2Dto, Is.Not.Null);
        Assert.That(sub2Dto.Title.InnerValue(), Is.EqualTo("Sub 2"));
    }

    [Test]
    public void BuildFormIntoViews_WithSubPropertyTableView_MapsProperties()
    {
        SubPropertyTableViewBuilder<TestModel, TableRowModel> tableView = new SubPropertyTableViewBuilder<TestModel, TableRowModel>(m => m.SubRows, r => r.Id)
            .WithTitle("Sub-Rows Table")
            .EnableAdd()
            .CanEditWhen(m => m.IsActive)
            .CanEditRowWhen(r => r.Selected)
            .CanDeleteWhen(m => m.IsActive)
            .CanDeleteRowWhen(r => r.Selected);

        tableView.Add(r => r.Description);

        var form = new TestForm<TestModel>(tableView);
        IReadOnlyList<View> result = _sut.BuildFormIntoViews(form);
        Assert.That(result, Has.Count.EqualTo(1));
        var tableDto = result[0] as SubPropertyTableView;
        Assert.That(tableDto, Is.Not.Null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(tableDto.Title.InnerValue(), Is.EqualTo("Sub-Rows Table"));
            Assert.That(tableDto.SubProperty, Is.EqualTo(nameof(TestModel.SubRows)));
            Assert.That(tableDto.IdProperty, Is.EqualTo(nameof(TableRowModel.Id)));

            Assert.That(tableDto.CanAdd.InnerValue(), Is.True);
            Assert.That(tableDto.CanEdit.InnerValue(), Is.EqualTo(nameof(TestModel.IsActive)));
            Assert.That(tableDto.CanEditRow.InnerValue(), Is.EqualTo(nameof(TableRowModel.Selected)));
            Assert.That(tableDto.CanDelete.InnerValue(), Is.EqualTo(nameof(TestModel.IsActive)));
            Assert.That(tableDto.CanDeleteRow.InnerValue(), Is.EqualTo(nameof(TableRowModel.Selected)));

            Assert.That(tableDto.Fields, Has.Count.EqualTo(1));
        }

        Assert.That(tableDto.Fields[0].Identifier, Is.EqualTo(nameof(TableRowModel.Description)));
    }

    [Test]
    public void BuildFormIntoViews_WithSubPropertyTableView_MapsSelectionOptions()
    {
        SubPropertyTableViewBuilder<TestModel, TableRowModel> tableView =
            new SubPropertyTableViewBuilder<TestModel, TableRowModel>(m => m.SubRows, r => r.Id)
            .EnableSelection(r => r.Selected, TableSelectionType.Single);

        var form = new TestForm<TestModel>(tableView);
        IReadOnlyList<View> result = _sut.BuildFormIntoViews(form);
        Assert.That(result, Has.Count.EqualTo(1));
        var table = result[0] as SubPropertyTableView;
        Assert.That(table, Is.Not.Null);
        Assert.That(table.TableSelectionOptions, Is.Not.Null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(table.TableSelectionOptions.SelectionProperty, Is.EqualTo(nameof(TableRowModel.Selected)));
            Assert.That(table.TableSelectionOptions.SelectionType, Is.EqualTo(TableSelectionType.Single));
        }

    }

    [Test]
    public void BuildFormIntoViews_WithSubPropertyTableView_AndEditForm_FlattensEditFormViewsRecursively()
    {
        FieldViewBuilder<TableRowModel> editFormView = new FieldViewBuilder<TableRowModel>().WithTitle("Edit Form Row View");
        editFormView.Add(r => r.Description);
        var editForm = new TestForm<TableRowModel>(editFormView);

        SubPropertyTableViewBuilder<TestModel, TableRowModel> tableView =
            new SubPropertyTableViewBuilder<TestModel, TableRowModel>(m => m.SubRows, r => r.Id)
            .WithTitle("Main Table")
            .WithEditForm(editForm);

        var form = new TestForm<TestModel>(tableView);
        IReadOnlyList<View> result = _sut.BuildFormIntoViews(form);
        Assert.That(result, Has.Count.EqualTo(2));

        var table = result[0] as SubPropertyTableView;
        Assert.That(table, Is.Not.Null);
        Assert.That(table.EditViewId, Is.EqualTo(1)); // Points to index 1

        var editFormDto = result[1] as FieldView;
        Assert.That(editFormDto, Is.Not.Null);
        Assert.That(editFormDto.Title.InnerValue(), Is.EqualTo("Edit Form Row View"));
        Assert.That(editFormDto.Fields.Single().Identifier, Is.EqualTo(nameof(TableRowModel.Description)));
    }

    [Test]
    public void BuildFormIntoViews_WithUnsupportedViewType_ThrowsUnreachableException()
    {
        var mockUnsupportedView = new Mock<IViewBuilder<TestModel>>();
        var form = new TestForm<TestModel>(mockUnsupportedView.Object);
        Assert.Throws<UnreachableException>(() => _sut.BuildFormIntoViews(form));
    }
}
