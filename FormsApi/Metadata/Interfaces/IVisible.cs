using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IVisible<TModel>
{
    public FormValueRefBuilder<TModel, bool>? Visible { get; set; }

}
public interface IVisible<TThis, TModel> : IVisible<TModel>;
