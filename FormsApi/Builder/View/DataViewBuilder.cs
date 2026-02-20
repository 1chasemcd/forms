using System.Collections;
using System.Linq.Expressions;
using System.Numerics;
using FormsApi.Builder.Field;
using FormsApi.Common.Types;
using FormsApi.Form;
using FormsApi.Form.View;

namespace FormsApi.Builder.View;

public sealed class DataViewBuilder<TModel> : ViewBuilder<TModel>, IFieldCollection<TModel>
{
    public DataViewBuilder(PropertyOrConstantBuilder<TModel, string>? title = null, FormElementSize width = default)
    {
        Title = title;
        Width = width;
    }

    private readonly IList<BaseFieldBuilder<TModel>> _fields = [];
    protected override DataView BuildImpl()
    {
        var view = new DataView
        {
            Fields = _fields.Select(x => x.Build())
        };

        return view;
    }
    public IEnumerator GetEnumerator() => _fields.GetEnumerator();

    public void Add(Expression<Func<TModel, Action>> selector, Action<ButtonBuilder<TModel>>? augment = null)
    {
        var field = new ButtonBuilder<TModel>(selector);
        augment?.Invoke(field);
        _fields.Add(field);
    }

    public void Add<T>(T fieldBuilder, Action<T>? augment = null)
            where T : BaseFieldBuilder<TModel>
    {
        augment?.Invoke(fieldBuilder);
        _fields.Add(fieldBuilder);
    }
    public void Add(Expression<Func<TModel, bool>> selector, Action<CheckBoxInputBuilder<TModel>>? augment = null)
    {
        var field = new CheckBoxInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        _fields.Add(field);
    }

    public void Add(Expression<Func<TModel, Currency>> selector, Action<CurrencyInputBuilder<TModel>>? augment = null)
    {
        var field = new CurrencyInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        _fields.Add(field);
    }

    public void Add(Expression<Func<TModel, DateOnly>> selector, Action<DateInputBuilder<TModel>>? augment = null)
    {
        var field = new DateInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        _fields.Add(field);
    }

    public void Add<TMember>(Expression<Func<TModel, TMember>> selector, Action<NumericInputBuilder<TModel, TMember>>? augment = null)
        where TMember : INumber<TMember>
    {
        var field = new NumericInputBuilder<TModel, TMember>(selector);
        augment?.Invoke(field);
        _fields.Add(field);
    }

    public void Add(string text, Action<StaticTextBuilder<TModel>>? augment = null)
    {
        var field = new StaticTextBuilder<TModel>(text);
        augment?.Invoke(field);
        _fields.Add(field);
    }

    public void Add(Expression<Func<TModel, string>> selector, Action<TextInputBuilder<TModel>>? augment = null)
    {
        var field = new TextInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        _fields.Add(field);
    }

    public void Add(Expression<Func<TModel, IEnumerable<string>>> selector, Action<TextAreaInputBuilder<TModel>>? augment = null)
    {
        var field = new TextAreaInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        _fields.Add(field);
    }

    public void Add(Expression<Func<TModel, TimeOnly>> selector, Action<TimeInputBuilder<TModel>>? augment = null)
    {
        var field = new TimeInputBuilder<TModel>(selector);
        augment?.Invoke(field);
        _fields.Add(field);
    }
}
