using System.Diagnostics;
using FormsApi.Contract.View;

namespace FormsApi.Forms.Services;

public interface IFormBuilderService
{
    IReadOnlyList<View> BuildFormIntoViews<TModel>(Form<TModel> form);
}

internal sealed class FormBuilderService : IFormBuilderService
{

    public IReadOnlyList<View> BuildFormIntoViews<TModel>(Form<TModel> form)
    {
        var result = new List<View>();
        ProcessView(form.View, result);
        return result;
    }

    private int ProcessView<TModel>(IViewBuilder<TModel> view, List<View> accumulatedResult)
    {
        int index = accumulatedResult.Count;
        accumulatedResult.Add(null!);

        View resultView = view switch
        {
            CombinedViewBuilder<TModel> combined => ProcessCombinedView(combined, accumulatedResult),
            ControlViewBuilder<TModel> control => ProcessControlView(control),
            ISubPropertyGridViewBuilder<TModel> spGrid => ProcessSubPropertyGridView(spGrid, accumulatedResult),
            _ => throw new UnreachableException($"Unable to process view type: {view.GetType()}")
        };

        accumulatedResult[index] = resultView with
        {
            Title = view.Title?.Build(),
            Width = view.Width,
            Visible = view.Visible?.Build()
        };

        return index;
    }

    private CombinedView ProcessCombinedView<TModel>(CombinedViewBuilder<TModel> view, List<View> accumulatedResult)
    {
        var viewIds = new List<int>();
        foreach (IViewBuilder<TModel> sub in view.Views)
        {
            viewIds.Add(ProcessView(sub, accumulatedResult));
        }

        var currentView = new CombinedView
        {
            ViewIds = viewIds,
            Unify = view.IsUnified
        };
        return currentView;
    }

    private ControlView ProcessControlView<TModel>(ControlViewBuilder<TModel> view)
    {
        return new ControlView
        {
            Controls = view.ControlList
        };
    }

    private SubPropertyGridView ProcessSubPropertyGridView<TModel>(ISubPropertyGridViewBuilder<TModel> view, List<View> accumulatedResult)
    {
        int? editViewId = null;
        if (view.EditForm is { } editForm)
        {
            editViewId = accumulatedResult.Count;
            IReadOnlyList<View> additionalViews = editForm.ProvideBuilder(this);
            accumulatedResult.AddRange(additionalViews);
        }

        GridSelectionOptions? selectionOptions = null;
        if (view.SelectionProperty is not null)
            selectionOptions = new()
            {
                SelectionProperty = view.SelectionProperty,
                SelectionType = view.SelectionType
            };

        return new SubPropertyGridView
        {
            Controls = view.ControlList,
            SubProperty = view.SubProperty,
            IdProperty = view.IdProperty,
            CanAdd = view.CanAdd?.Build(),
            CanEdit = view.CanEdit?.Build(),
            CanEditRow = view.CanEditRow?.Build(),
            CanDelete = view.CanDelete?.Build(),
            CanDeleteRow = view.CanDeleteRow?.Build(),
            EditViewId = editViewId,
            GridSelectionOptions = selectionOptions
        };
    }
}
