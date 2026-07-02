using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IPrecisionAndScalable<TModel>
{
    FormValueRefBuilder<TModel, int>? Precision { get; set; }
    FormValueRefBuilder<TModel, int>? Scale { get; set; }

}
public interface IPrecisionAndScalable<TThis, TModel> : IPrecisionAndScalable<TModel>;
