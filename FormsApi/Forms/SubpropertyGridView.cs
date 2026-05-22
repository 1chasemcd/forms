using System.Collections;
using System.Linq.Expressions;
using FormsApi.Contract;
using FormsApi.Contract.View;

namespace FormsApi.Forms;

public interface ISubPropertyGridView<TModel> : IView<TModel>
{
    IReadOnlyList<FormControlLayoutDto> ControlList { get; }
    string SubProperty { get; }
    string IdProperty { get; }
    PropertyOrConstantDto? CanAdd { get; }
    PropertyOrConstantDto? CanEdit { get; }
    PropertyOrConstantDto? CanEditRow { get; }
    PropertyOrConstantDto? CanDelete { get; }
    PropertyOrConstantDto? CanDeleteRow { get; }
    IForm? EditForm { get; }
    string? SelectionProperty { get; }
    GridSelectionType SelectionType { get; }
}

public sealed class SubPropertyGridView<TModel, TSub>(
    Expression<Func<TModel, IEnumerable<TSub>?>> subProperty, Expression<Func<TSub, object?>> idProperty)
    : View<TModel, SubPropertyGridView<TModel, TSub>>, ISubPropertyGridView<TModel>, IEnumerable
{
    public IReadOnlyList<FormControlLayoutDto> ControlList => _controlList;
    private readonly List<FormControlLayoutDto> _controlList = [];
    public string SubProperty { get; } = subProperty.GetPropertyName();
    public string IdProperty { get; } = idProperty.GetPropertyName();
    public PropertyOrConstantDto? CanAdd { get; private set; }
    public PropertyOrConstantDto? CanEdit { get; private set; }
    public PropertyOrConstantDto? CanEditRow { get; private set; }
    public PropertyOrConstantDto? CanDelete { get; private set; }
    public PropertyOrConstantDto? CanDeleteRow { get; private set; }
    public IForm? EditForm { get; private set; }
    public string? SelectionProperty { get; private set; }
    public GridSelectionType SelectionType { get; private set; }
    public IEnumerator GetEnumerator() => (ControlList as IEnumerable).GetEnumerator();

    public void Add(Expression<Func<TSub, object?>> selector, int? width = null)
    {
        _controlList.Add(new(selector.GetPropertyName(), width));
    }

    public SubPropertyGridView<TModel, TSub> EnableAdd()
    {
        CanAdd = new ConstantDto(true);
        return this;
    }

    public SubPropertyGridView<TModel, TSub> CanAddWhen(Expression<Func<TModel, bool>> selector)
    {
        CanAdd = new PropertyDto(selector.GetPropertyName());
        return this;
    }

    public SubPropertyGridView<TModel, TSub> EnableEdit()
    {
        CanEdit = new ConstantDto(true);
        return this;
    }

    public SubPropertyGridView<TModel, TSub> CanEditWhen(Expression<Func<TModel, bool>> selector)
    {
        CanEdit = new PropertyDto(selector.GetPropertyName());
        return this;
    }

    public SubPropertyGridView<TModel, TSub> CanEditRowWhen(Expression<Func<TSub, bool>> selector)
    {
        if (CanEdit is null) EnableEdit();
        CanEditRow = new PropertyDto(selector.GetPropertyName());
        return this;
    }

    public SubPropertyGridView<TModel, TSub> EnableDelete()
    {
        CanDelete = new ConstantDto(true);
        return this;
    }

    public SubPropertyGridView<TModel, TSub> CanDeleteWhen(Expression<Func<TModel, bool>> selector)
    {
        CanDelete = new PropertyDto(selector.GetPropertyName());
        return this;
    }

    public SubPropertyGridView<TModel, TSub> CanDeleteRowWhen(Expression<Func<TSub, bool>> selector)
    {
        if (CanDelete is null) EnableDelete();
        CanDeleteRow = new PropertyDto(selector.GetPropertyName());
        return this;
    }

    public SubPropertyGridView<TModel, TSub> EnableSelection(Expression<Func<TSub, bool>> selectionProperty, GridSelectionType selectionType = GridSelectionType.Multiple)
    {
        SelectionProperty = selectionProperty.GetPropertyName();
        SelectionType = selectionType;
        return this;
    }

    public SubPropertyGridView<TModel, TSub> WithEditForm(Form<TSub> editForm)
    {
        if (CanEdit is null) EnableEdit();
        EditForm = editForm;
        return this;
    }
}
