namespace FormsApi.Builder.Metadata;

public interface IPrecisionAndScalable<TModel>
{
    PropertyOrConstant<TModel, int>? Precision { get; set; }
    PropertyOrConstant<TModel, int>? Scale { get; set; }

}
public interface IPrecisionAndScalable<TThis, TModel> : IPrecisionAndScalable<TModel>;
