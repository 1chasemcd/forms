using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Numerics;
using FormsApi.Builder.Metadata;
using FormsApi.Recalculate;

namespace FormsApi.Builder;

public abstract class Metadata<TModel>
{
    private readonly Dictionary<string, IMetadataBuilder<TModel>> _metadataBuilders = [];
    internal ReadOnlyDictionary<string, IMetadataBuilder<TModel>> MetadataBuilders => _metadataBuilders.AsReadOnly();
    protected ButtonInputMetadataBuilder<TModel> Button(Expression<Func<TModel, object?>> selector) =>
        Add<ButtonInputMetadataBuilder<TModel>, object>(selector);

    protected CheckBoxInputMetadataBuilder<TModel> CheckBox(Expression<Func<TModel, bool>> selector) =>
        Add<CheckBoxInputMetadataBuilder<TModel>, bool>(selector);

    protected CurrencyInputMetadataBuilder<TModel> Currency(Expression<Func<TModel, decimal>> selector) =>
        Add<CurrencyInputMetadataBuilder<TModel>, decimal>(selector);

    protected DateInputMetadataBuilder<TModel> Date(Expression<Func<TModel, DateOnly>> selector) =>
        Add<DateInputMetadataBuilder<TModel>, DateOnly>(selector);

    protected LabelValueMetadataBuilder<TModel> LabelValue(Expression<Func<TModel, string?>> selector) =>
        Add<LabelValueMetadataBuilder<TModel>, string>(selector);

    protected NumericInputMetadataBuilder<TModel, TNumber> Numeric<TNumber>(Expression<Func<TModel, TNumber?>> selector)
        where TNumber : INumber<TNumber> =>
        Add<NumericInputMetadataBuilder<TModel, TNumber>, TNumber>(selector);

    protected TextAreaInputMetadataBuilder<TModel> TextArea(Expression<Func<TModel, string?>> selector) =>
        Add<TextAreaInputMetadataBuilder<TModel>, string>(selector);

    protected TextInputMetadataBuilder<TModel> Text(Expression<Func<TModel, string?>> selector) =>
        Add<TextInputMetadataBuilder<TModel>, string>(selector);

    protected TimeInputMetadataBuilder<TModel> Time(Expression<Func<TModel, TimeOnly>> selector) =>
        Add<TimeInputMetadataBuilder<TModel>, TimeOnly>(selector);

    private TMetadata Add<TMetadata, TProperty>(Expression<Func<TModel, TProperty?>> selector) where TMetadata : IMetadataBuilder<TModel>, new()
    {
        string propertyName = new ModelMember<TModel, TProperty>(selector).Build();
        if (_metadataBuilders.TryGetValue(propertyName, out IMetadataBuilder<TModel>? builder))
        {
            if (builder is TMetadata tBuilder) return tBuilder;
            else throw new InvalidOperationException($"Property {propertyName} was already assigned a different input type");
        }

        builder = new TMetadata();
        _metadataBuilders.Add(propertyName, builder);
        return (TMetadata)builder;
    }


    protected RecalculateEvent<TModel, TService> Recalculate<TService>(
        Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> selector)
    {
        return new RecalculateEvent<TModel, TService>(selector);
    }
}
