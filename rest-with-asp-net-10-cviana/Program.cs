using rest_with_asp_net_10_cviana.Auth.Contract;
using rest_with_asp_net_10_cviana.Auth.Tools;
using rest_with_asp_net_10_cviana.Configurations;
using rest_with_asp_net_10_cviana.Files.Exporters.Factory;
using rest_with_asp_net_10_cviana.Files.Exporters.Impl;
using rest_with_asp_net_10_cviana.Files.Importers.Factory;
using rest_with_asp_net_10_cviana.Files.Importers.Impl;
using rest_with_asp_net_10_cviana.Hypermedia.Filters;
using rest_with_asp_net_10_cviana.Mail;
using rest_with_asp_net_10_cviana.Repositories;
using rest_with_asp_net_10_cviana.Repositories.BookRepository;
using rest_with_asp_net_10_cviana.Repositories.PersonRepository;
using rest_with_asp_net_10_cviana.Repositories.UsersRepository;
using rest_with_asp_net_10_cviana.Services;
using rest_with_asp_net_10_cviana.Services.Impl;


// --------------------------------------------------
// Setting up resources
var builder = WebApplication.CreateBuilder(args);

builder.AddSeriLogging();
// --------------------------------------------------

// --------------------------------------------------
// Adding services to the container.
builder.Services
    .AddControllers(options =>
        {
            options.Filters.Add<HypermediaFilter>();
        })
    .AddContentNegotiation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiConfig();

//SWAGGER CONFIGS
builder.Services.AddSwaggerConfig();
builder.Services.AddRouteConfig();

builder.Services.AddCorsConfiguration(builder.Configuration);

builder.Services.AddHateoasConfiguration();

builder.Services.AddEmailConfiguration(builder.Configuration);

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);

builder.Services.AddAuthConfiguration(builder.Configuration);

builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IPersonServices, PersonServices>();
builder.Services.AddScoped<IBookServices, BookServices>();

builder.Services.AddScoped<IEmailServices, EmailServices>();
builder.Services.AddScoped<EmailSender>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IFileServices, FileServices>();
builder.Services.AddScoped<CsvFileImporter>();
builder.Services.AddScoped<XlsxFileImporter>();
builder.Services.AddScoped<FileImporterFactory>();
builder.Services.AddScoped<CsvFileExporter>();
builder.Services.AddScoped<XlsxFileExporter>();
builder.Services.AddScoped<FileExporterFactory>();

builder.Services.AddScoped<IPasswordHasher, Sha256PasswordHasher>();
builder.Services.AddScoped<IUsersAuthServices, UsersAuthServices>();
builder.Services.AddScoped<ILoginServices, LoginServices>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();

builder.Services.AddOpenApi();
// --------------------------------------------------

// --------------------------------------------------
// Building all services set above
var app = builder.Build();

// --------------------------------------------------
// Configuring the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCorsConfiguration(builder.Configuration);

app.MapControllers();

app.UseHateoasRoutes();

app.UseSwaggerSpecification();
app.UseScalarSpecification();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://*:{port}");
