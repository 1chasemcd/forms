using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IMaxLengthable<TModel>
{
    FormValueRefBuilder<TModel, int>? MaxLength { get; set; }
}
public interface IMaxLengthable<TThis, TModel> : IMaxLengthable<TModel>;
