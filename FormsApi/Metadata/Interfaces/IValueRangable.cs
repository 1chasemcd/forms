using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IValueRangable<TModel, TField>
{
    PropertyOrConstantBuilder<TModel, TField>? MaxValue { get; set; }
    PropertyOrConstantBuilder<TModel, TField>? MinValue { get; set; }
}

public interface IValueRangable<TThis, TModel, TField> : IValueRangable<TModel, TField>;
