namespace FormsApi.Builder.Metadata;

public interface IMaxLengthable<TModel>
{
    PropertyOrConstant<TModel, int>? MaxLength { get; set; }
}
public interface IMaxLengthable<TThis, TModel> : IMaxLengthable<TModel>;
