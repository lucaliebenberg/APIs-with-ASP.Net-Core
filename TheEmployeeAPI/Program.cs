using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var employees = new List<Employee>()
{
    new Employee { Id = 1, FirstName = "John", LastName = "Luke" },
    new Employee { Id = 2, FirstName = "James", LastName = "Thomas" }
};

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var employeeRoute = app.MapGroup("employees");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

employeeRoute.MapGet(string.Empty, () =>
{
    return employees;
});

employeeRoute.MapGet("{id:int}", (int id) =>
{
    var employee = employees.SingleOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(employee);
});

employeeRoute.MapPost(string.Empty, ([FromBody] Employee employee, HttpContext context) =>
{
    employee.Id = employees.Max(e => e.Id) + 1; // not using a DB
    employees.Add(employee);
    return Results.Created($"/employees/{employee.Id}", employee);
});

app.Run();

