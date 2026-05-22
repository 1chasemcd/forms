using System.Diagnostics;
using FormsApi.Contract.View;

namespace FormsApi.Forms.Services;

public interface IFormBuilderService
{
    IReadOnlyList<BaseViewDto> BuildFormIntoViews<TModel>(Form<TModel> form);
}

internal sealed class FormBuilderService : IFormBuilderService
{

    public IReadOnlyList<BaseViewDto> BuildFormIntoViews<TModel>(Form<TModel> form)
    {
        var result = new List<BaseViewDto>();
        ProcessView(form.View, result);
        return result;
    }

    private int ProcessView<TModel>(IView<TModel> view, List<BaseViewDto> accumulatedResult)
    {
        int index = accumulatedResult.Count;
        accumulatedResult.Add(null!);

        BaseViewDto resultView = view switch
        {
            CombinedView<TModel> combined => ProcessCombinedView(combined, accumulatedResult),
            ControlView<TModel> control => ProcessControlView(control),
            ISubPropertyGridView<TModel> spGrid => ProcessSubPropertyGridView(spGrid, accumulatedResult),
            _ => throw new UnreachableException($"Unable to process view type: {view.GetType()}")
        };

        accumulatedResult[index] = resultView with
        {
            Title = view.Title,
            Width = view.Width,
            Visible = view.Visible
        };

        return index;
    }

    private CombinedViewDto ProcessCombinedView<TModel>(CombinedView<TModel> view, List<BaseViewDto> accumulatedResult)
    {
        var viewIds = new List<int>();
        foreach (IView<TModel> sub in view.Views)
        {
            viewIds.Add(ProcessView(sub, accumulatedResult));
        }

        var currentView = new CombinedViewDto
        {
            ViewIds = viewIds,
            Unify = view.IsUnified
        };
        return currentView;
    }

    private ControlViewDto ProcessControlView<TModel>(ControlView<TModel> view)
    {
        return new ControlViewDto
        {
            Controls = view.ControlList
        };
    }

    private SubPropertyGridViewDto ProcessSubPropertyGridView<TModel>(ISubPropertyGridView<TModel> view, List<BaseViewDto> accumulatedResult)
    {
        int? editViewId = null;
        if (view.EditForm is { } editForm)
        {
            editViewId = accumulatedResult.Count;
            IReadOnlyList<BaseViewDto> additionalViews = editForm.ProvideBuilder(this);
            accumulatedResult.AddRange(additionalViews);
        }

        GridSelectionOptions? selectionOptions = null;
        if (view.SelectionProperty is not null)
            selectionOptions = new()
            {
                SelectionProperty = view.SelectionProperty,
                SelectionType = view.SelectionType
            };

        return new SubPropertyGridViewDto
        {
            Controls = view.ControlList,
            SubProperty = view.SubProperty,
            IdProperty = view.IdProperty,
            CanAdd = view.CanAdd,
            CanEdit = view.CanEdit,
            CanEditRow = view.CanEditRow,
            CanDelete = view.CanDelete,
            CanDeleteRow = view.CanDeleteRow,
            EditViewId = editViewId,
            GridSelectionOptions = selectionOptions
        };
    }
}
