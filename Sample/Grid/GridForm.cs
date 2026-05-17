using FormsApi.Contract.View;
using FormsApi.Forms;
using FormsApi.Metadata;
using FormsApi.Repository.Handlers;

namespace Sample.Grid;

public class GridForm : Form<GridFormModel>
{
    protected override BaseView<GridFormModel> View => new CombinedView<GridFormModel>()
{
    new CombinedView<GridFormModel>()
    {
        GridSettings(),
        TransactionGrid(),
    }.Unified(),
    TransactionGrid2(),
    UserGrid(),
};

    private FieldView<GridFormModel> GridSettings()
    {
        return new FieldView<GridFormModel>()
    {
        {x => x.AllowAdd, 3},
        {x => x.AllowEdit, 3},
        {x => x.AllowDelete, 3}
    };
    }

    private SubPropertyGridView<GridFormModel, Transaction> TransactionGrid()
    {
        return new SubPropertyGridView<GridFormModel, Transaction>(x => x.Transactions, t => t.Id)
        {
            {t => t.Date, 3},
            {t => t.Time, 3},
            {t => t.Amount, 3 },
            {t => t.IncludeInCostSplitting, 3},
            t => t.Description,
            t => t.Notes,
            t => t.Editable
        }
        .EnableSelection(t => t.Selected)
        .WithTitle("Transactions")
        .CanAddWhen(m => m.AllowAdd)
        .CanEditWhen(m => m.AllowEdit)
        .CanEditRowWhen(m => m.Editable)
        .CanDeleteWhen(m => m.AllowDelete);
    }

    private SubPropertyGridView<GridFormModel, Transaction> TransactionGrid2()
    {
        return new SubPropertyGridView<GridFormModel, Transaction>(x => x.Transactions2, t => t.Id)
        {
            t => t.Date,
            t => t.Time,
            t => t.Amount,
            t => t.Description,
        }
        .EnableSelection(t => t.Selected, GridSelectionType.Single)
        .EnableEdit().WithEditForm(new TransactionEditForm());
    }

    private SubPropertyGridView<GridFormModel, BasicUserModel> UserGrid()
    {
        return new SubPropertyGridView<GridFormModel, BasicUserModel>(x => x.Users, t => t.Id)
        {
            {u => u.Id, 1},
            u => u.UserName
        }
        .WithTitle("Users").EnableAdd().EnableEdit();
        // .WithEditForm(new UserEditForm());
    }
}

public class TransactionEditForm : Form<Transaction>
{
    protected override BaseView<Transaction> View => new FieldView<Transaction>
    {
        {t => t.Date, 6},
        {t => t.Time, 6},
        t => t.Description,
        {t => t.Amount, 6},
        {t => t.IncludeInCostSplitting, 6},
        t => t.Notes
    };
}

public class UserEditForm : Form<User>
{
    protected override BaseView<User> View => new FieldView<User>
    {
        u => u.Id ,
        u => u.UserName,
        u => u.Email,
        u => u.PhoneNumber,
        u => u.IsAdmin
    };
}

public class TransactionMetadata : Metadata<Transaction>
{
    public TransactionMetadata()
    {
        Currency(m => m.Amount);
    }
}

public class UserMetadata : Metadata<User>
{
    public UserMetadata()
    {
        Numeric(m => m.Id).Disabled();
    }
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
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
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
