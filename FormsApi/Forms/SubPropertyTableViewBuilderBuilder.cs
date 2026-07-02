using System.Collections;
using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract;
using FormsApi.Contract.View;

namespace FormsApi.Forms;

public interface ISubPropertyTableViewBuilder<TModel> : IViewBuilder<TModel>
{
    IReadOnlyList<FormFieldInfoContainer> FieldList { get; }
    string SubProperty { get; }
    string IdProperty { get; }
    FormValueRefBuilder<TModel, bool>? CanAdd { get; }
    FormValueRefBuilder<TModel, bool>? CanEdit { get; }
    IFormValueRefBuilder? CanEditRow { get; }
    FormValueRefBuilder<TModel, bool>? CanDelete { get; }
    IFormValueRefBuilder? CanDeleteRow { get; }
    IForm? EditForm { get; }
    string? SelectionProperty { get; }
    TableSelectionType SelectionType { get; }
}

public sealed class SubPropertyTableViewBuilder<TModel, TSub>(
    Expression<Func<TModel, IEnumerable<TSub>?>> subProperty, Expression<Func<TSub, object?>> idProperty)
    : ViewBuilder<TModel, SubPropertyTableViewBuilder<TModel, TSub>>, ISubPropertyTableViewBuilder<TModel>, IEnumerable
{
    public IReadOnlyList<FormFieldInfoContainer> FieldList => _fieldList;
    private readonly List<FormFieldInfoContainer> _fieldList = [];
    public string SubProperty { get; } = subProperty.GetPropertyName();
    public string IdProperty { get; } = idProperty.GetPropertyName();
    public FormValueRefBuilder<TModel, bool>? CanAdd { get; private set; }
    public FormValueRefBuilder<TModel, bool>? CanEdit { get; private set; }
    public IFormValueRefBuilder? CanEditRow { get; private set; }
    public FormValueRefBuilder<TModel, bool>? CanDelete { get; private set; }
    public IFormValueRefBuilder? CanDeleteRow { get; private set; }
    public IForm? EditForm { get; private set; }
    public string? SelectionProperty { get; private set; }
    public TableSelectionType SelectionType { get; private set; }
    public IEnumerator GetEnumerator() => (FieldList as IEnumerable).GetEnumerator();

    public void Add(Expression<Func<TSub, object?>> selector, int? width = null)
    {
        _fieldList.Add(new(selector.GetPropertyName(), width));
    }

    public SubPropertyTableViewBuilder<TModel, TSub> EnableAdd()
    {
        CanAdd = true;
        return this;
    }

    public SubPropertyTableViewBuilder<TModel, TSub> CanAddWhen(Expression<Func<TModel, bool>> selector)
    {
        CanAdd = selector;
        return this;
    }

    public SubPropertyTableViewBuilder<TModel, TSub> EnableEdit()
    {
        CanEdit = true;
        return this;
    }

    public SubPropertyTableViewBuilder<TModel, TSub> CanEditWhen(Expression<Func<TModel, bool>> selector)
    {
        CanEdit = selector;
        return this;
    }

    public SubPropertyTableViewBuilder<TModel, TSub> CanEditRowWhen(Expression<Func<TSub, bool>> selector)
    {
        if (CanEdit is null) EnableEdit();
        CanEditRow = new FormValueRefBuilder<TSub, bool>(selector);
        return this;
    }

    public SubPropertyTableViewBuilder<TModel, TSub> EnableDelete()
    {
        CanDelete = true;
        return this;
    }

    public SubPropertyTableViewBuilder<TModel, TSub> CanDeleteWhen(Expression<Func<TModel, bool>> selector)
    {
        CanDelete = selector;
        return this;
    }

    public SubPropertyTableViewBuilder<TModel, TSub> CanDeleteRowWhen(Expression<Func<TSub, bool>> selector)
    {
        if (CanDelete is null) EnableDelete();
        CanDeleteRow = new FormValueRefBuilder<TSub, bool>(selector);
        return this;
    }

    public SubPropertyTableViewBuilder<TModel, TSub> EnableSelection(Expression<Func<TSub, bool>> selectionProperty, TableSelectionType selectionType = TableSelectionType.Multiple)
    {
        SelectionProperty = selectionProperty.GetPropertyName();
        SelectionType = selectionType;
        return this;
    }

    public SubPropertyTableViewBuilder<TModel, TSub> WithEditForm(Form<TSub> editForm)
    {
        if (CanEdit is null) EnableEdit();
        EditForm = editForm;
        return this;
    }
}
