using System.Diagnostics;
using FormsApi.Contract;
using FormsApi.Contract.View;
using FormsApi.Forms;
using FormsApi.Forms.Services;
using Moq;
using NUnit.Framework;

namespace Tests.Forms;

[TestFixture]
public class FormBuilderServiceTests
{
    public sealed class TestModel
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public List<GridRowModel> SubRows { get; set; } = [];
    }

    public sealed class GridRowModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool Selected { get; set; }
    }

    private sealed class TestForm<TModel>(IView<TModel> view) : Form<TModel>
    {
        protected internal override IView<TModel> View => view;
    }

    private FormBuilderService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _sut = new FormBuilderService();
    }

    [Test]
    public void BuildFormIntoViews_WithControlView_MapsCorrectly()
    {
        ControlView<TestModel> controlView = new ControlView<TestModel>()
            .WithTitle("Basic Control Info")
            .WithWidth(12)
            .VisibleWhen(m => m.IsActive);

        controlView.Add(m => m.Name, 6);
        controlView.Add(m => m.Age, 6);

        var form = new TestForm<TestModel>(controlView);
        IReadOnlyList<BaseViewDto> result = _sut.BuildFormIntoViews(form);
        Assert.That(result, Has.Count.EqualTo(1));
        var dto = result[0] as ControlViewDto;
        Assert.That(dto, Is.Not.Null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(dto.Width, Is.EqualTo(12));
            Assert.That(dto.Title.InnerValue(), Is.EqualTo("Basic Control Info"));
            Assert.That(dto.Visible.InnerValue(), Is.EqualTo(nameof(TestModel.IsActive)));
            Assert.That(dto.Controls, Has.Count.EqualTo(2));
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(dto.Controls[0].PropertyName, Is.EqualTo(nameof(TestModel.Name)));
            Assert.That(dto.Controls[0].Width, Is.EqualTo(6));
            Assert.That(dto.Controls[1].PropertyName, Is.EqualTo(nameof(TestModel.Age)));
            Assert.That(dto.Controls[1].Width, Is.EqualTo(6));
        }

    }

    [Test]
    public void BuildFormIntoViews_WithCombinedView_FlattensAndMaintainsIndices()
    {
        ControlView<TestModel> subView1 = new ControlView<TestModel>().WithTitle("Sub 1");
        subView1.Add(m => m.Name);

        ControlView<TestModel> subView2 = new ControlView<TestModel>().WithTitle("Sub 2");
        subView2.Add(m => m.Age);

        CombinedView<TestModel> combinedView = new CombinedView<TestModel>()
            .WithTitle("Main Combined")
            .Unify();
        combinedView.Add(subView1);
        combinedView.Add(subView2);

        var form = new TestForm<TestModel>(combinedView);
        IReadOnlyList<BaseViewDto> result = _sut.BuildFormIntoViews(form);
        Assert.That(result, Has.Count.EqualTo(3));

        var combinedDto = result[0] as CombinedViewDto;
        Assert.That(combinedDto, Is.Not.Null);
        Assert.That(combinedDto.Title.InnerValue(), Is.EqualTo("Main Combined"));
        Assert.That(combinedDto.Unify, Is.True);
        Assert.That(combinedDto.ViewIds, Is.EquivalentTo(new[] { 1, 2 }));

        var sub1Dto = result[1] as ControlViewDto;
        Assert.That(sub1Dto, Is.Not.Null);
        Assert.That(sub1Dto.Title.InnerValue(), Is.EqualTo("Sub 1"));

        var sub2Dto = result[2] as ControlViewDto;
        Assert.That(sub2Dto, Is.Not.Null);
        Assert.That(sub2Dto.Title.InnerValue(), Is.EqualTo("Sub 2"));
    }

    [Test]
    public void BuildFormIntoViews_WithSubPropertyGridView_MapsProperties()
    {
        SubPropertyGridView<TestModel, GridRowModel> gridView = new SubPropertyGridView<TestModel, GridRowModel>(m => m.SubRows, r => r.Id)
            .WithTitle("Sub-Rows Grid")
            .EnableAdd()
            .CanEditWhen(m => m.IsActive)
            .CanEditRowWhen(r => r.Selected)
            .CanDeleteWhen(m => m.IsActive)
            .CanDeleteRowWhen(r => r.Selected);

        gridView.Add(r => r.Description);

        var form = new TestForm<TestModel>(gridView);
        IReadOnlyList<BaseViewDto> result = _sut.BuildFormIntoViews(form);
        Assert.That(result, Has.Count.EqualTo(1));
        var gridDto = result[0] as SubPropertyGridViewDto;
        Assert.That(gridDto, Is.Not.Null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridDto.Title.InnerValue(), Is.EqualTo("Sub-Rows Grid"));
            Assert.That(gridDto.SubProperty, Is.EqualTo(nameof(TestModel.SubRows)));
            Assert.That(gridDto.IdProperty, Is.EqualTo(nameof(GridRowModel.Id)));

            Assert.That(gridDto.CanAdd.InnerValue(), Is.True);
            Assert.That(gridDto.CanEdit.InnerValue(), Is.EqualTo(nameof(TestModel.IsActive)));
            Assert.That(gridDto.CanEditRow.InnerValue(), Is.EqualTo(nameof(GridRowModel.Selected)));
            Assert.That(gridDto.CanDelete.InnerValue(), Is.EqualTo(nameof(TestModel.IsActive)));
            Assert.That(gridDto.CanDeleteRow.InnerValue(), Is.EqualTo(nameof(GridRowModel.Selected)));

            Assert.That(gridDto.Controls, Has.Count.EqualTo(1));
        }

        Assert.That(gridDto.Controls[0].PropertyName, Is.EqualTo(nameof(GridRowModel.Description)));
    }

    [Test]
    public void BuildFormIntoViews_WithSubPropertyGridView_MapsSelectionOptions()
    {
        SubPropertyGridView<TestModel, GridRowModel> gridView =
            new SubPropertyGridView<TestModel, GridRowModel>(m => m.SubRows, r => r.Id)
            .EnableSelection(r => r.Selected, GridSelectionType.Single);

        var form = new TestForm<TestModel>(gridView);
        IReadOnlyList<BaseViewDto> result = _sut.BuildFormIntoViews(form);
        Assert.That(result, Has.Count.EqualTo(1));
        var gridDto = result[0] as SubPropertyGridViewDto;
        Assert.That(gridDto, Is.Not.Null);
        Assert.That(gridDto.GridSelectionOptions, Is.Not.Null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridDto.GridSelectionOptions.SelectionProperty, Is.EqualTo(nameof(GridRowModel.Selected)));
            Assert.That(gridDto.GridSelectionOptions.SelectionType, Is.EqualTo(GridSelectionType.Single));
        }

    }

    [Test]
    public void BuildFormIntoViews_WithSubPropertyGridView_AndEditForm_FlattensEditFormViewsRecursively()
    {
        ControlView<GridRowModel> editFormView = new ControlView<GridRowModel>().WithTitle("Edit Form Row View");
        editFormView.Add(r => r.Description);
        var editForm = new TestForm<GridRowModel>(editFormView);

        SubPropertyGridView<TestModel, GridRowModel> gridView =
            new SubPropertyGridView<TestModel, GridRowModel>(m => m.SubRows, r => r.Id)
            .WithTitle("Main Grid")
            .WithEditForm(editForm);

        var form = new TestForm<TestModel>(gridView);
        IReadOnlyList<BaseViewDto> result = _sut.BuildFormIntoViews(form);
        Assert.That(result, Has.Count.EqualTo(2));

        var gridDto = result[0] as SubPropertyGridViewDto;
        Assert.That(gridDto, Is.Not.Null);
        Assert.That(gridDto.EditViewId, Is.EqualTo(1)); // Points to index 1

        var editFormDto = result[1] as ControlViewDto;
        Assert.That(editFormDto, Is.Not.Null);
        Assert.That(editFormDto.Title.InnerValue(), Is.EqualTo("Edit Form Row View"));
        Assert.That(editFormDto.Controls.Single().PropertyName, Is.EqualTo(nameof(GridRowModel.Description)));
    }

    [Test]
    public void BuildFormIntoViews_WithUnsupportedViewType_ThrowsUnreachableException()
    {
        var mockUnsupportedView = new Mock<IView<TestModel>>();
        var form = new TestForm<TestModel>(mockUnsupportedView.Object);
        Assert.Throws<UnreachableException>(() => _sut.BuildFormIntoViews(form));
    }
}
