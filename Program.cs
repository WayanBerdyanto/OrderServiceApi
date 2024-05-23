using OrderApi;
using OrderApi.DAL;
using OrderApi.DAL.Interface;
using OrderApi.DTO;
using OrderApi.Models;
using OrderApi.Services;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomer, CustomerDAL>();
builder.Services.AddScoped<IOrderHeader, OrderHeaderDAL>();
builder.Services.AddScoped<IOrderDetail, OrderDetailDAL>();

//register HttpClient
builder.Services.AddHttpClient<IProductService, ProductService>().AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(6000)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/customers", (ICustomer customer) => 
{
    return Results.Ok(customer.GetAll());
});


app.MapGet("/customers/{id}", (ICustomer customer, int id) =>
{
    var cust = customer.GetById(id);
    if (cust == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(cust);
});

app.MapPost("/customers", (ICustomer customer, Customer obj) =>
{
    try
    {
        var cust = customer.Insert(obj);
        return Results.Created($"/customer{obj.CustomerID}", cust);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/orderHeaders", (IOrderHeader orderHeader) =>
{
    return Results.Ok(orderHeader.GetAll());
});

app.MapGet("/orderHeaders/{id}", (IOrderHeader orderHeader, int id) =>
{

    var order = orderHeader.GetById(id);
    if (order == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(order);
});

app.MapPost("/orderHeaders", (IOrderHeader orderHeader, OrderHeader obj) =>
{
    try
    {
        var order = orderHeader.Insert(obj);
        return Results.Created($"/orderHeaders/{obj.OrderHeaderID}", order);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/orderDetails", (IOrderDetail orderDetail) =>
{
    return Results.Ok(orderDetail.GetAll());
});

app.MapGet("/orderDetails/{id}", (IOrderDetail orderDetail, int id) =>
{
    var order = orderDetail.GetById(id);
    if (order == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(order);
});

app.MapPost("/orderDetails", async (IOrderDetail orderDetail, IProductService productService, OrderDetail obj)=>
{
    try
    {
        var product = await productService.GetProductById(obj.ProductId);
        if (product == null)
        {
            return Results.BadRequest("Product not found");
        }
        if (product.quantity < obj.Quantity)
        {
            return Results.BadRequest("Stock not enough");
        }
        obj.Price = product.price;
        var order = orderDetail.Insert(obj);
        var productUpdateStockDto = new ProductUpdateStockDto
        {
            ProductID = obj.ProductId,
            Quantity = obj.Quantity
        };
        // return Results.Ok(productUpdateStockDto);
        await productService.UpdateProductStock(productUpdateStockDto);
        return Results.Created($"/orderDetails/{obj.OrderDetailId}", order);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/orderDetails", async (IOrderDetail orderDetail, IProductService productService, OrderDetail obj) =>
{
    try
    {
        var product = await productService.GetProductById(obj.ProductId);
        if (product == null)
        {
            return Results.BadRequest("Product not found");
        }
        if (product.quantity < obj.Quantity)
        {
            return Results.BadRequest("Stock not enough");
        }
        obj.Price = product.price;
        orderDetail.Update(obj);
        return Results.Created($"/orderDetails/{obj.OrderDetailId}", obj);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
