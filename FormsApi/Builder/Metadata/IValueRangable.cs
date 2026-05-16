using System.Numerics;

namespace FormsApi.Builder.Metadata;

public interface IValueRangable<TModel, TField>
{
    PropertyOrConstant<TModel, TField>? MaxValue { get; set; }
    PropertyOrConstant<TModel, TField>? MinValue { get; set; }
}

public interface IValueRangable<TThis, TModel, TField> : IValueRangable<TModel, TField>;
