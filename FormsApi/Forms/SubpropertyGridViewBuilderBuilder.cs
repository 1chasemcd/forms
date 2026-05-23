using System.Collections;
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract;
using FormsApi.Contract.View;

namespace FormsApi.Forms;

public interface ISubPropertyGridViewBuilder<TModel> : IViewBuilder<TModel>
{
    IReadOnlyList<FormControl> ControlList { get; }
    string SubProperty { get; }
    string IdProperty { get; }
    PropertyOrConstantBuilder<TModel, bool>? CanAdd { get; }
    PropertyOrConstantBuilder<TModel, bool>? CanEdit { get; }
    IPropertyOrConstantBuilder? CanEditRow { get; }
    PropertyOrConstantBuilder<TModel, bool>? CanDelete { get; }
    IPropertyOrConstantBuilder? CanDeleteRow { get; }
    IForm? EditForm { get; }
    string? SelectionProperty { get; }
    GridSelectionType SelectionType { get; }
}

public sealed class SubPropertyGridViewBuilder<TModel, TSub>(
    Expression<Func<TModel, IEnumerable<TSub>?>> subProperty, Expression<Func<TSub, object?>> idProperty)
    : ViewBuilder<TModel, SubPropertyGridViewBuilder<TModel, TSub>>, ISubPropertyGridViewBuilder<TModel>, IEnumerable
{
    public IReadOnlyList<FormControl> ControlList => _controlList;
    private readonly List<FormControl> _controlList = [];
    public string SubProperty { get; } = subProperty.GetPropertyName();
    public string IdProperty { get; } = idProperty.GetPropertyName();
    public PropertyOrConstantBuilder<TModel, bool>? CanAdd { get; private set; }
    public PropertyOrConstantBuilder<TModel, bool>? CanEdit { get; private set; }
    public IPropertyOrConstantBuilder? CanEditRow { get; private set; }
    public PropertyOrConstantBuilder<TModel, bool>? CanDelete { get; private set; }
    public IPropertyOrConstantBuilder? CanDeleteRow { get; private set; }
    public IForm? EditForm { get; private set; }
    public string? SelectionProperty { get; private set; }
    public GridSelectionType SelectionType { get; private set; }
    public IEnumerator GetEnumerator() => (ControlList as IEnumerable).GetEnumerator();

    public void Add(Expression<Func<TSub, object?>> selector, int? width = null)
    {
        _controlList.Add(new(selector.GetPropertyName(), width));
    }

    public SubPropertyGridViewBuilder<TModel, TSub> EnableAdd()
    {
        CanAdd = true;
        return this;
    }

    public SubPropertyGridViewBuilder<TModel, TSub> CanAddWhen(Expression<Func<TModel, bool>> selector)
    {
        CanAdd = selector;
        return this;
    }

    public SubPropertyGridViewBuilder<TModel, TSub> EnableEdit()
    {
        CanEdit = true;
        return this;
    }

    public SubPropertyGridViewBuilder<TModel, TSub> CanEditWhen(Expression<Func<TModel, bool>> selector)
    {
        CanEdit = selector;
        return this;
    }

    public SubPropertyGridViewBuilder<TModel, TSub> CanEditRowWhen(Expression<Func<TSub, bool>> selector)
    {
        if (CanEdit is null) EnableEdit();
        CanEditRow = new PropertyOrConstantBuilder<TSub, bool>(selector);
        return this;
    }

    public SubPropertyGridViewBuilder<TModel, TSub> EnableDelete()
    {
        CanDelete = true;
        return this;
    }

    public SubPropertyGridViewBuilder<TModel, TSub> CanDeleteWhen(Expression<Func<TModel, bool>> selector)
    {
        CanDelete = selector;
        return this;
    }

    public SubPropertyGridViewBuilder<TModel, TSub> CanDeleteRowWhen(Expression<Func<TSub, bool>> selector)
    {
        if (CanDelete is null) EnableDelete();
        CanDeleteRow = new PropertyOrConstantBuilder<TSub, bool>(selector);
        return this;
    }

    public SubPropertyGridViewBuilder<TModel, TSub> EnableSelection(Expression<Func<TSub, bool>> selectionProperty, GridSelectionType selectionType = GridSelectionType.Multiple)
    {
        SelectionProperty = selectionProperty.GetPropertyName();
        SelectionType = selectionType;
        return this;
    }

    public SubPropertyGridViewBuilder<TModel, TSub> WithEditForm(Form<TSub> editForm)
    {
        if (CanEdit is null) EnableEdit();
        EditForm = editForm;
        return this;
    }
}
