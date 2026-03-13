using System.ComponentModel.DataAnnotations;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.Field;

public sealed record class StaticTextField : BaseField
{
    private readonly string _id = Guid.NewGuid().ToString();
    [Required]
    public override string Id => _id;
}
