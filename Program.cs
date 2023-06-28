using Microsoft.EntityFrameworkCore;
using TearApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TearList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors();
builder.Services.AddSession(options=>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // session expires after 30 minutes of inactivity
});

var app = builder.Build();
app.MapControllers();

//Configure CORS
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});
app.UseSession();

var UserItemsController = app.MapGroup("/useritems");
var MatchController = app.MapGroup("/match");
var OppositeItemsController = app.MapGroup("/oppositeitems");

UserItemsController.MapGet("/", GetAllUsers);
UserItemsController.MapPost("/", CreateUser);
UserItemsController.MapDelete("/{id}", DeleteUser);
UserItemsController.MapPut("/{id}", UpdateUser);

MatchController.MapGet("/", GetMatch);

OppositeItemsController.MapGet("/", GetAllOpposites);
OppositeItemsController.MapPost("/", CreateOpposite);


app.Run();



    static async Task<IResult> GetMatch(int ownUserId, AppDbContext db)
{
    var user = await db.Users.FindAsync(ownUserId);

    if (user == null)
    {
        return TypedResults.NotFound($"User with UserId {ownUserId} not found.");
    }

    var connectedUser = await db.Users.FirstOrDefaultAsync(u => u.OppositeId == user.OppositeId && u.IsAgreeing != user.IsAgreeing);

    if (connectedUser == null)
    {
        return TypedResults.NotFound("Connected user not found.");
    }

    return TypedResults.Ok(connectedUser.UserId);
}


static async Task<IResult> GetAllOpposites(AppDbContext db)
{
    return TypedResults.Ok(await db.Opposites.ToArrayAsync());
}


static async Task<IResult> CreateOpposite(List<Opposite> opposites, AppDbContext db)
{
    db.Opposites.AddRange(opposites);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/oppositeitems", opposites);
}

static async Task<IResult> GetAllUsers(AppDbContext db)
{
    return TypedResults.Ok(await db.Users.ToArrayAsync());
}
static async Task<IResult> CreateUser(User user, AppDbContext db)
{
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/userItems/{user.UserId}", user);
}

static async Task<IResult> DeleteUser(int id, AppDbContext db)
{
    if (await db.Users.FindAsync(id) is User user)
    {
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return TypedResults.Ok(user);
    }

    return TypedResults.NotFound();
}

static async Task<IResult> UpdateUser(int id, User inputUser, AppDbContext db)
{
    var user = await db.Users.FindAsync(id);

    if (user is null) return TypedResults.NotFound();

    user.OppositeId = inputUser.OppositeId;
    user.IsAgreeing = inputUser.IsAgreeing;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}