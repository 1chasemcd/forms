using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IPrecisionAndScalable<TModel>
{
    PropertyOrConstantBuilder<TModel, int>? Precision { get; set; }
    PropertyOrConstantBuilder<TModel, int>? Scale { get; set; }

}
public interface IPrecisionAndScalable<TThis, TModel> : IPrecisionAndScalable<TModel>;
