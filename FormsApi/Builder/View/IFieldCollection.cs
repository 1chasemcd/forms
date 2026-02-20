using System.Collections;
using System.Linq.Expressions;
using System.Numerics;
using FormsApi.Builder.Field;
using FormsApi.Common.Types;

namespace FormsApi.Builder.View;

public interface IFieldCollection<TModel> : IEnumerable
{
    public void Add<T>(T fieldBuilder, Action<T>? augment = null)
        where T : BaseFieldBuilder<TModel>;
    public void Add(Expression<Func<TModel, Action>> selector, Action<ButtonBuilder<TModel>>? augment = null);
    public void Add(Expression<Func<TModel, bool>> selector, Action<CheckBoxInputBuilder<TModel>>? augment = null);
    public void Add(Expression<Func<TModel, Currency>> selector, Action<CurrencyInputBuilder<TModel>>? augment = null);
    public void Add(Expression<Func<TModel, DateOnly>> selector, Action<DateInputBuilder<TModel>>? augment = null);
    public void Add<TMember>(Expression<Func<TModel, TMember>> selector, Action<NumericInputBuilder<TModel, TMember>>? augment = null)
        where TMember : INumber<TMember>;
    public void Add(string text, Action<StaticTextBuilder<TModel>>? augment = null);
    public void Add(Expression<Func<TModel, string>> selector, Action<TextInputBuilder<TModel>>? augment = null);
    public void Add(Expression<Func<TModel, IEnumerable<string>>> selector, Action<TextAreaInputBuilder<TModel>>? augment = null);
    public void Add(Expression<Func<TModel, TimeOnly>> selector, Action<TimeInputBuilder<TModel>>? augment = null);
}
