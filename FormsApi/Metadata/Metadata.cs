using System.Linq.Expressions;
using System.Numerics;
using FormsApi.Common;
using FormsApi.FormService;
using FormsApi.Metadata.Builders;

namespace FormsApi.Metadata;

public abstract class Metadata<TModel>
{
    internal Dictionary<string, IMetadataBuilder<TModel>> MetadataBuilders { get; } = [];
    protected ButtonMetadataBuilder<TModel> Button(Expression<Func<TModel, object?>> selector) =>
        Add<ButtonMetadataBuilder<TModel>, object>(selector);

    protected CheckBoxMetadataBuilder<TModel> CheckBox(Expression<Func<TModel, bool>> selector) =>
        Add<CheckBoxMetadataBuilder<TModel>, bool>(selector);

    protected CurrencyMetadataBuilder<TModel> Currency(Expression<Func<TModel, decimal>> selector) =>
        Add<CurrencyMetadataBuilder<TModel>, decimal>(selector);

    protected DateMetadataBuilder<TModel> Date(Expression<Func<TModel, DateOnly>> selector) =>
        Add<DateMetadataBuilder<TModel>, DateOnly>(selector);

    protected LabelValueMetadataBuilder<TModel> LabelValue(Expression<Func<TModel, string?>> selector) =>
        Add<LabelValueMetadataBuilder<TModel>, string>(selector);

    protected NumericMetadataBuilder<TModel, TNumber> Numeric<TNumber>(Expression<Func<TModel, TNumber?>> selector)
        where TNumber : INumber<TNumber> =>
        Add<NumericMetadataBuilder<TModel, TNumber>, TNumber>(selector);

    protected TextAreaMetadataBuilder<TModel> TextArea(Expression<Func<TModel, string?>> selector) =>
        Add<TextAreaMetadataBuilder<TModel>, string>(selector);

    protected TextMetadataBuilder<TModel> Text(Expression<Func<TModel, string?>> selector) =>
        Add<TextMetadataBuilder<TModel>, string>(selector);

    protected TimeMetadataBuilder<TModel> Time(Expression<Func<TModel, TimeOnly>> selector) =>
        Add<TimeMetadataBuilder<TModel>, TimeOnly>(selector);

    private TMetadata Add<TMetadata, TProperty>(Expression<Func<TModel, TProperty?>> selector) where TMetadata : IMetadataBuilder<TModel>, new()
    {
        string propertyName = new ModelMember<TModel, TProperty>(selector).Build();
        if (MetadataBuilders.TryGetValue(propertyName, out IMetadataBuilder<TModel>? builder))
        {
            if (builder is TMetadata tBuilder) return tBuilder;
            else throw new InvalidOperationException($"Property {propertyName} was already assigned a different input type");
        }

        builder = new TMetadata();
        MetadataBuilders.Add(propertyName, builder);
        return (TMetadata)builder;
    }


    protected FormServiceMethod<TModel, TService> Recalculate<TService>(
        Expression<Func<TService, Func<TModel, FormServicePostAction?>>> selector)
    {
        return new FormServiceMethod<TModel, TService>(selector);
    }
}
