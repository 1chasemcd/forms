using System.Collections;
using System.Linq.Expressions;
using FormsApi.Builder.Field;
using FormsApi.Definition;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace FormsApi.Builder.View;

public class SubPropertyGridViewBuilder<TModel, TSub>(
    Expression<Func<TModel, IEnumerable<TSub>?>> subProperty, Expression<Func<TSub, object?>> idProperty)
    : ViewBuilder<TModel>, IFieldCollection<TSub>
{
    public ModelMemberBuilder<TModel, IEnumerable<TSub>?> SubProperty { get; } = subProperty;
    public ModelMemberBuilder<TSub, object?> IdProperty { get; } = idProperty;
    public PropertyOrConstantBuilder<TModel, bool>? CanAdd { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? CanEdit { get; set; }
    public PropertyOrConstantBuilder<TSub, bool>? CanEditRow { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? CanDelete { get; set; }
    public PropertyOrConstantBuilder<TSub, bool>? CanDeleteRow { get; set; }
    public IBuildable<FormDefinition>? EditForm { get; set; }
    public IList<IFieldBuilder<TSub>> Fields { get; } = [];

    private ModelMemberBuilder<TSub, bool>? _selectionProperty;
    private GridSelectionType _selectionType;
    public SubPropertyGridViewBuilder<TModel, TSub> EnableSelection(Expression<Func<TSub, bool>> selectionProperty, GridSelectionType selectionType = GridSelectionType.Multiple)
    {
        _selectionProperty = selectionProperty;
        _selectionType = selectionType;
        return this;
    }

    public IEnumerator GetEnumerator() => Fields.GetEnumerator();
    protected override BaseViewDefinition BuildImpl()
    {
        return new SubPropertyGridViewDefinition()
        {
            IdProperty = IdProperty.Build(),
            Fields = Fields.Select(x => x.Build()),
            SubPropertyName = SubProperty.Build(),
            CanAdd = CanAdd?.Build(),
            CanEdit = CanEdit?.Build(),
            CanEditRow = CanEditRow?.Build(),
            CanDelete = CanDelete?.Build(),
            CanDeleteRow = CanDeleteRow?.Build(),
            EditForm = EditForm?.Build(),
            SelectionOptions = _selectionProperty != null ? new GridSelectionOptions
            {
                SelectionProperty = _selectionProperty.Build(),
                SelectionType = _selectionType
            } : null
        };
    }
}
