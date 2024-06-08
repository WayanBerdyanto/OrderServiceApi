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
// , IOrderHeader orderHeader, UserService userService
builder.Services.AddScoped<ICustomer, CustomerDAL>();
builder.Services.AddScoped<IOrderHeader, OrderHeaderDAL>();
builder.Services.AddScoped<IOrderDetail, OrderDetailDAL>();

//register HttpClient
builder.Services.AddHttpClient<IProductService, ProductService>().AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(6000)));

builder.Services.AddHttpClient<IUserService, UserService>().AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(6000)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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

app.MapPost("/orderHeaders", async (IOrderHeader orderHeader, IUserService userService, OrderHeaderDTO obj, IProductService productService) =>
{
    try
    {
        var user = await userService.GetUserByName(obj.username);
        if (user == null)
        {
            return Results.BadRequest("data not found");
        }
        OrderHeader order = new OrderHeader
        {
            UserName = obj.username,
            OrderDate = obj.OrderDate,
        };
        orderHeader.Insert(order);
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

app.MapPost("/orderDetails", async (IOrderDetail orderDetail, IProductService productService, OrderDetailDTO obj, IOrderHeader orderHeader, IUserService userService) =>
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
        var order = orderHeader.GetById(obj.OrderHeaderId);
        if (order == null)
        {
            return Results.BadRequest("Order Header not found");
        }

        obj.Price = obj.Quantity * product.price;
        OrderDetail detail = new OrderDetail
        {
            OrderHeaderId = obj.OrderHeaderId,
            ProductId = obj.ProductId,
            Quantity = obj.Quantity,
            Price = obj.Price
        };

        orderDetail.Insert(detail);
        var productUpdateStockDto = new ProductUpdateStockDto
        {
            ProductID = obj.ProductId,
            Quantity = obj.Quantity
        };

        var userUpdateBalance = new UserUpdateBalance
        {
            UserName = order.UserName,
            Balance = obj.Price
        };


        await productService.UpdateProductStock(productUpdateStockDto);
        await userService.UpdateUserBalance(userUpdateBalance);
        return Results.Created($"/orderDetails/{detail.OrderDetailId}", detail);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/cancle/orderDetails", async (IOrderDetail orderDetail, IProductService productService, OrderDetail obj, IOrderHeader orderHeader, IUserService userService) =>
{
    try
    {
        var details = orderDetail.GetById(obj.OrderDetailId);
        var order = orderHeader.GetById(details.OrderHeaderId);
        var userUpdateBalance = new UserUpdateBalance
        {
            UserName = order.UserName,
            Balance = details.Price
        };

        orderDetail.Delete(details.OrderDetailId);
        await userService.UpdateUserBackBalance(userUpdateBalance);
        return Results.Ok(new { Message = "Success Cancle", userUpdateBalance});
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
