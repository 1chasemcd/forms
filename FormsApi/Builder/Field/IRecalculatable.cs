using System;
using System.Linq.Expressions;
using FormsApi.Recalculate;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FormsApi.Builder.Field;

public interface IRecalculatable;

public interface IRecalculatable<TModel> : IRecalculatable
{
    IRecalculateEventBuilder<TModel>? RecalculateEvent { get; }
    void AddRecalc<TService>(Expression<Func<TService, Func<TModel, PostRecalculateEvent?>>> method);
}