using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IValueRangable<TModel, TField>
{
    FormValueRefBuilder<TModel, TField>? MaxValue { get; set; }
    FormValueRefBuilder<TModel, TField>? MinValue { get; set; }
}

public interface IValueRangable<TThis, TModel, TField> : IValueRangable<TModel, TField>;
