using Microsoft.EntityFrameworkCore;
using Phase1_Layer2_BlazorChat.Components;
using Phase1_Layer2_BlazorChat.Data;
using Phase1_Layer2_BlazorChat.Services;

var builder = WebApplication.CreateBuilder(args);
//----------------------
//recipie: configuration
//-----------------------

//Read the database connection string from the configuration (appsettings.json)
//this keeps environment-specific settings out of the code and allows for easier management of configuration values.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=chatapp.db"; // Fallback to a default connection string if not found in configuration

//--------------------------
//Recepie: Services
//--------------------------
//Add blazor ui services
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();
//add EF core using SQLite as the database provider
//we use a dbcontext factory to create instances of the AppDbContext as needed 
//because Blazor Server apps are long-lived and we want to ensure that we create a new dbcontext instance for each operation
//thus to avoid issues with concurrency and stale data.
builder.Services.AddDbContextFactory<AppDbContext>(options =>
   options.UseSqlite(connectionString));
//register our app service .
//later this  service will coordinate chat logic, AI interactions, and database operations related to chat messages.
builder.Services.AddHttpClient<ChatService>();

var app = builder.Build();

//--------------------------
//recipie: Middleware/pipeline
//--------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();


//---------------------------
//Recipie: EndPoints/component mapping
//-------------------------------

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
//create teh SQlite ab automatically if it doesnot exist yet.
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var db = factory.CreateDbContext();
    db.Database.EnsureCreated();
}
    app.Run();

