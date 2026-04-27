using FormsApi.Builder;
using FormsApi.Builder.View;
using FormsApi.Common.Types;
using FormsApi.Definition.Primitives;
using FormsApi.Repository.Handler;

namespace Sample;

public class GridForm : FormBuilder<GridFormModel>
{
    protected override ViewBuilder<GridFormModel> View => new CombinedViewBuilder<GridFormModel>
    {
        new CombinedViewBuilder<GridFormModel>(unify: true)
        {
            GridSettings(),
            TransactionGrid(),
        },
        TransactionGrid2(),
        UserGrid(),
    };

    private FieldViewBuilder<GridFormModel> GridSettings()
    {
        return new FieldViewBuilder<GridFormModel>()
        {
            {x => x.AllowAdd, x => x.Width = 3},
            {x => x.AllowEdit, x => x.Width = 3},
            {x => x.AllowDelete, x => x.Width = 3}
        };
    }

    private SubPropertyGridViewBuilder<GridFormModel, Transaction> TransactionGrid()
    {
        var grid = new SubPropertyGridViewBuilder<GridFormModel, Transaction>(x => x.Transactions, t => t.Id)
        {
            {t => t.Date, x => x.Width = 3},
            {t => t.Time, x => x.Width = 3},
            {t => t.Amount, x => x.Width = 3 },
            {t => t.IncludeInCostSplitting, x => x.Width = 3},
            t => t.Description,
            t => t.Notes,
            t => t.Editable
        }.EnableSelection(t => t.Selected);

        grid.Title = "Transactions";
        grid.CanAdd = new PropertyOrConstantBuilder<GridFormModel, bool>(x => x.AllowAdd);
        grid.CanEdit = new PropertyOrConstantBuilder<GridFormModel, bool>(x => x.AllowEdit);
        grid.CanDelete = new PropertyOrConstantBuilder<GridFormModel, bool>(x => x.AllowDelete);
        grid.CanEditRow = new PropertyOrConstantBuilder<Transaction, bool>(t => t.Editable);
        return grid;
    }

    private SubPropertyGridViewBuilder<GridFormModel, Transaction> TransactionGrid2()
    {
        var grid = new SubPropertyGridViewBuilder<GridFormModel, Transaction>(x => x.Transactions2, t => t.Id)
        {
            t => t.Date,
            t => t.Time,
            t => t.Amount,
            t => t.Description,
        }.EnableSelection(t => t.Selected, GridSelectionType.Single);

        grid.CanEdit = true;
        grid.EditForm = new TransactionEditForm();

        return grid;
    }

    private SubPropertyGridViewBuilder<GridFormModel, BasicUserModel> UserGrid()
    {
        var grid = new SubPropertyGridViewBuilder<GridFormModel, BasicUserModel>(x => x.Users, t => t.Id)
        {
            {u => u.Id, x => x.Width = 1},
            u => u.UserName
        };

        grid.Title = "Users";
        grid.EditForm = new UserEditForm();
        grid.CanEdit = true;
        grid.CanAdd = true;

        return grid;
    }
}

public class TransactionEditForm : FormBuilder<Transaction>
{
    protected override ViewBuilder<Transaction> View => new FieldViewBuilder<Transaction>
    {
        {t => t.Date, x => x.Width = 6},
        {t => t.Time, x => x.Width = 6},
        t => t.Description,
        {t => t.Amount, x => x.Width = 6},
        {t => t.IncludeInCostSplitting, x => x.Width = 6},
        t => t.Notes
    };
}

public class UserEditForm : FormBuilder<User>
{
    protected override ViewBuilder<User> View => new FieldViewBuilder<User>
    {
        {u => u.Id, x => x.Enabled = false },
        u => u.UserName,
        u => u.Email,
        u => u.PhoneNumber,
        u => u.IsAdmin
    };
}

public class GridFormModel
{
    public bool AllowAdd { get; set; } = true;
    public bool AllowEdit { get; set; } = true;
    public bool AllowDelete { get; set; } = true;
    public IList<Transaction> Transactions { get; set; } = [
        new Transaction {
            Id = 1,
            Date = new DateOnly(2026, 01, 17),
            Time = new TimeOnly(14, 30),
            Amount = 115.01m,
            Description = "Amazon.com transaction",
            Notes = "Bought earrings\nthey are ugly"
        },
        new Transaction {
            Id = 2,
            Date = new DateOnly(2026, 01, 26),
            Time = new TimeOnly(9, 45),
            Amount = 96.24m,
            Description = "City market",
            Editable = true
        }
    ];

    public IList<Transaction> Transactions2 { get; set; } = [
        new Transaction {
            Id = 3,
            Date = new DateOnly(2026, 02, 17),
            Time = new TimeOnly(14, 30),
            Amount = 1900.00m,
            Description = "Rent payment",
        },
        new Transaction {
            Id = 4,
            Date = new DateOnly(2026, 02, 26),
            Time = new TimeOnly(11, 45),
            Amount = 16.24m,
            Description = "Ragged Mountain Sports",
        }
    ];

    public IList<BasicUserModel> Users { get; set; } = [.. UserRepository.users.Select(x => new BasicUserModel() { Id = x.Id, UserName = x.UserName })];

}

public class Transaction
{
    public int Id { get; set; }
    public bool Selected { get; set; }
    public bool IncludeInCostSplitting { get; set; }
    public Currency Amount { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Description { get; set; } = string.Empty;
    public TextArea Notes { get; set; } = string.Empty;
    public bool Editable { get; set; }
}

public class BasicUserModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
}

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public string? Email { get; set; }
    public int PhoneNumber { get; set; }
}

public class UserRepository : IRepositoryQueryHandler<User>
{
    internal static IList<User> users = [
        new User() {
            Id = 12,
            UserName = "Heinz Doofenshmirtz",
            IsAdmin = true,
            Email = "heinz@doof.com",
            PhoneNumber = 1234567890,
        },
        new User() {
            Id = 17,
            UserName = "Big Bird",
        },
        new User() {
            Id = 24,
            UserName = "Mickey Mouse",
        },
    ];

    public async Task<IEnumerable<User>> GetAllAsync() => users;
    public async Task<User?> GetAsync(string id)
    {
        if (!int.TryParse(id, out int result))
            return null;
        return users.SingleOrDefault(x => x.Id == result);
    }

}
