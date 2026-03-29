using System.Collections;
using System.Linq.Expressions;
using FormsApi.Builder.Field;
using FormsApi.Form.Field;
using FormsApi.Form.View;

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
    public FormBuilder<TSub>? EditForm { get; set; }
    public IList<BaseFieldBuilder<TSub>> Fields { get; } = [];

    public IEnumerator GetEnumerator() => Fields.GetEnumerator();
    protected override BaseView BuildImpl()
    {
        return new SubPropertyGridView()
        {
            IdProperty = IdProperty.Build(),
            // TODO this will need fixing sometime
            Fields = Fields.Select(x => x.Build()).Where(x => x is BaseInput).Cast<BaseInput>(),
            SubPropertyName = SubProperty.Build(),
            CanAdd = CanAdd?.Build(),
            CanEdit = CanEdit?.Build(),
            CanEditRow = CanEditRow?.Build(),
            CanDelete = CanDelete?.Build(),
            CanDeleteRow = CanDeleteRow?.Build(),
            EditForm = EditForm?.Build()
        };
    }
}
