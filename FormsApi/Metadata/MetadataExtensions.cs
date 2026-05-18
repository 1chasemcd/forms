using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata;

public static class MetadataExtensions
{
    public static TThis Disabled<TThis, TModel>(this IEnablable<TThis, TModel> self)
    {
        self.Enabled = false;
        return (TThis)self;
    }
    public static TThis EnabledWhen<TThis, TModel>(this IEnablable<TThis, TModel> self, Expression<Func<TModel, bool>> selector)
    {
        self.Enabled = selector;
        return (TThis)self;
    }
    public static TThis WithLabel<TThis, TModel>(this ILabelable<TThis, TModel> self, string label)
    {
        self.Label = label;
        return (TThis)self;
    }
    public static TThis WithLabel<TThis, TModel>(this ILabelable<TThis, TModel> self,
        Expression<Func<TModel, string?>> selector)
    {
        self.Label = selector;
        return (TThis)self;
    }
    public static TThis WithMaxLength<TThis, TModel>(this IMaxLengthable<TThis, TModel> self, int maxLength)
    {
        self.MaxLength = maxLength;
        return (TThis)self;
    }
    public static TThis WithMaxLength<TThis, TModel>(this IMaxLengthable<TThis, TModel> self,
        Expression<Func<TModel, int>> selector)
    {
        self.MaxLength = selector;
        return (TThis)self;
    }
    public static TThis WithPrecision<TThis, TModel>(this IPrecisionAndScalable<TThis, TModel> self, int precision)
    {
        self.Precision = precision;
        return (TThis)self;
    }
    public static TThis WithPrecision<TThis, TModel>(this IPrecisionAndScalable<TThis, TModel> self,
        Expression<Func<TModel, int>> selector)
    {
        self.Precision = selector;
        return (TThis)self;
    }
    public static TThis WithScale<TThis, TModel>(this IPrecisionAndScalable<TThis, TModel> self, int scale)
    {
        self.Scale = scale;
        return (TThis)self;
    }
    public static TThis WithScale<TThis, TModel>(this IPrecisionAndScalable<TThis, TModel> self,
        Expression<Func<TModel, int>> selector)
    {
        self.Scale = selector;
        return (TThis)self;
    }
    public static TThis OnChange<TThis, TModel>(this IRecalculatable<TThis, TModel> self, IFormServiceMethod<TModel> recalc)
    {
        self.FormServiceMethod = recalc;
        return (TThis)self;
    }
    public static TThis Required<TThis, TModel>(this IRequirable<TThis, TModel> self)
    {
        self.Required = true;
        return (TThis)self;
    }
    public static TThis RequiredWhen<TThis, TModel>(this IRequirable<TThis, TModel> self, Expression<Func<TModel, bool>> selector)
    {
        self.Required = selector;
        return (TThis)self;
    }
    public static TThis WithMinValue<TThis, TModel, TField>(this IValueRangable<TThis, TModel, TField> self, TField minValue)
    {
        self.MinValue = minValue;
        return (TThis)self;
    }
    public static TThis WithMinValue<TThis, TModel, TField>(this IValueRangable<TThis, TModel, TField> self,
        Expression<Func<TModel, TField?>> minValue)
    {
        self.MinValue = minValue;
        return (TThis)self;
    }
    public static TThis WithMaxValue<TThis, TModel, TField>(this IValueRangable<TThis, TModel, TField> self, TField maxValue)
    {
        self.MaxValue = maxValue;
        return (TThis)self;
    }
    public static TThis WithMaxValue<TThis, TModel, TField>(this IValueRangable<TThis, TModel, TField> self,
        Expression<Func<TModel, TField?>> maxValue)
    {
        self.MaxValue = maxValue;
        return (TThis)self;
    }
    public static TThis Hidden<TThis, TModel>(this IVisible<TThis, TModel> self)
    {
        self.Visible = false;
        return (TThis)self;
    }

    public static TThis VisibleWhen<TThis, TModel>(this IVisible<TThis, TModel> self, Expression<Func<TModel, bool>> selector)
    {
        self.Visible = selector;
        return (TThis)self;
    }
}
