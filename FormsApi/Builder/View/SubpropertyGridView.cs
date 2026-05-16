using System.Collections;
using System.Linq.Expressions;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace FormsApi.Builder.View;

public sealed class SubPropertyGridView<TModel, TSub>(
    Expression<Func<TModel, IEnumerable<TSub>?>> subProperty, Expression<Func<TSub, object?>> idProperty)
    : BaseView<TModel, SubPropertyGridView<TModel, TSub>>, IEnumerable
{
    public FieldList<TSub> Fields { get; set; } = [];
    public ModelMember<TModel, IEnumerable<TSub>?> SubProperty { get; } = subProperty;
    public ModelMember<TSub, object?> IdProperty { get; } = idProperty;
    public PropertyOrConstant<TModel, bool>? CanAdd { get; set; }
    public PropertyOrConstant<TModel, bool>? CanEdit { get; set; }
    public PropertyOrConstant<TSub, bool>? CanEditRow { get; set; }
    public PropertyOrConstant<TModel, bool>? CanDelete { get; set; }
    public PropertyOrConstant<TSub, bool>? CanDeleteRow { get; set; }
    public Form<TSub>? EditForm { get; set; }
    private ModelMember<TSub, bool>? _selectionProperty;
    private GridSelectionType _selectionType;
    public IEnumerator GetEnumerator() => Fields.GetEnumerator();
    protected override BaseViewDto BuildImpl()
    {
        return new SubPropertyGridViewDefinition()
        {
            IdProperty = IdProperty.Build(),
            Fields = Fields.Fields,
            SubProperty = SubProperty.Build(),
            CanAdd = CanAdd?.Build(),
            CanEdit = CanEdit?.Build(),
            CanEditRow = CanEditRow?.Build(),
            CanDelete = CanDelete?.Build(),
            CanDeleteRow = CanDeleteRow?.Build(),
            // EditForm = EditForm?.Build(),
            GridSelectionOptions = _selectionProperty != null ? new GridSelectionOptions
            {
                SelectionProperty = _selectionProperty.Build(),
                SelectionType = _selectionType
            } : null
        };
    }

    public void Add(Expression<Func<TSub, object?>> selector, int? width = null)
    {
        Fields.Add(selector, width);
    }

    public SubPropertyGridView<TModel, TSub> EnableAdd()
    {
        CanAdd = true;
        return this;
    }

    public SubPropertyGridView<TModel, TSub> CanAddWhen(Expression<Func<TModel, bool>> selector)
    {
        CanAdd = selector;
        return this;
    }

    public SubPropertyGridView<TModel, TSub> EnableEdit()
    {
        CanEdit = true;
        return this;
    }

    public SubPropertyGridView<TModel, TSub> CanEditWhen(Expression<Func<TModel, bool>> selector)
    {
        CanEdit = selector;
        return this;
    }

    public SubPropertyGridView<TModel, TSub> CanEditRowWhen(Expression<Func<TSub, bool>> selector)
    {
        if (CanEdit is null) EnableEdit();
        CanEditRow = selector;
        return this;
    }

    public SubPropertyGridView<TModel, TSub> EnableDelete()
    {
        CanDelete = true;
        return this;
    }

    public SubPropertyGridView<TModel, TSub> CanDeleteWhen(Expression<Func<TModel, bool>> selector)
    {
        CanDelete = selector;
        return this;
    }

    public SubPropertyGridView<TModel, TSub> CanDeleteRowWhen(Expression<Func<TSub, bool>> selector)
    {
        if (CanDelete is null) EnableDelete();
        CanDeleteRow = selector;
        return this;
    }

    public SubPropertyGridView<TModel, TSub> EnableSelection(Expression<Func<TSub, bool>> selectionProperty, GridSelectionType selectionType = GridSelectionType.Multiple)
    {
        _selectionProperty = selectionProperty;
        _selectionType = selectionType;
        return this;
    }

    public SubPropertyGridView<TModel, TSub> WithEditForm(Form<TSub> editForm)
    {
        if (CanEdit is null) EnableEdit();
        EditForm = editForm;
        return this;
    }
}
