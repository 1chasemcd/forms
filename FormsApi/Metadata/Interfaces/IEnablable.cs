using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IEnablable<TModel>
{
    public FormValueRefBuilder<TModel, bool>? Enabled { get; set; }
}
public interface IEnablable<TThis, TModel> : IEnablable<TModel>;
