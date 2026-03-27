using FormsApi.Form.Field;

namespace FormsApi.Form.View;

public interface IFieldView
{
    IEnumerable<BaseField> Fields { get; init; }
}
