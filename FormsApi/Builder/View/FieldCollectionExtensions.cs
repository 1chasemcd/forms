using System.Linq.Expressions;
using System.Numerics;
using FormsApi.Builder.Field;
using FormsApi.Common.Types;

namespace FormsApi.Builder.View;

public static class FieldCollectionExtensions
{
    public static void Add<TModel>(this IFieldCollection<TModel> fieldCollection, Expression<Func<TModel, Action>> selector, Action<ButtonBuilder<TModel>>? augment = null)
    {
        var field = new ButtonBuilder<TModel>(selector);
        augment?.Invoke(field);
        fieldCollection.Fields.Add(field);
    }

    public static void Add<TModel>(this IFieldCollection<TModel> fieldCollection, Expression<Func<TModel, bool?>> selector, Action<CheckBoxInputBuilder<TModel>>? augment = null)
    {
        var field = new CheckBoxInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        fieldCollection.Fields.Add(field);
    }

    public static void Add<TModel>(this IFieldCollection<TModel> fieldCollection, Expression<Func<TModel, Currency?>> selector, Action<CurrencyInputBuilder<TModel>>? augment = null)
    {
        var field = new CurrencyInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        fieldCollection.Fields.Add(field);
    }

    public static void Add<TModel>(this IFieldCollection<TModel> fieldCollection, Expression<Func<TModel, DateOnly?>> selector, Action<DateInputBuilder<TModel>>? augment = null)
    {
        var field = new DateInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        fieldCollection.Fields.Add(field);
    }

    public static void Add<TModel, TMember>(this IFieldCollection<TModel> fieldCollection, Expression<Func<TModel, TMember?>> selector, Action<NumericInputBuilder<TModel, TMember>>? augment = null)
        where TMember : INumber<TMember>
    {
        var field = new NumericInputBuilder<TModel, TMember>(selector);
        augment?.Invoke(field);
        fieldCollection.Fields.Add(field);
    }

    public static void Add<TModel>(this IFieldCollection<TModel> fieldCollection, string text, Action<StaticTextBuilder<TModel>>? augment = null)
    {
        var field = new StaticTextBuilder<TModel>(text);
        augment?.Invoke(field);
        fieldCollection.Fields.Add(field);
    }

    public static void Add<TModel>(this IFieldCollection<TModel> fieldCollection, Expression<Func<TModel, string?>> selector, Action<TextInputBuilder<TModel>>? augment = null)
    {
        var field = new TextInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        fieldCollection.Fields.Add(field);
    }

    public static void Add<TModel>(this IFieldCollection<TModel> fieldCollection, Expression<Func<TModel, IEnumerable<string>?>> selector, Action<TextAreaInputBuilder<TModel>>? augment = null)
    {
        var field = new TextAreaInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        fieldCollection.Fields.Add(field);
    }

    public static void Add<TModel>(this IFieldCollection<TModel> fieldCollection, Expression<Func<TModel, TimeOnly?>> selector, Action<TimeInputBuilder<TModel>>? augment = null)
    {
        var field = new TimeInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        fieldCollection.Fields.Add(field);
    }
}
