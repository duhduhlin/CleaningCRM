using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CleaningCRM.API.Data;
using CleaningCRM.API.Models;

namespace CleaningCRM.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Customer")]
public class ClientCabinetController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientCabinetController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (client == null) return NotFound("Профиль не найден");

        return Ok(client);
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (client == null) return NotFound("Профиль не найден");

        var orders = await _context.Orders
            .Include(o => o.Service)
            .Include(o => o.Employee)
            .Where(o => o.ClientId == client.Id)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return Ok(orders);
    }

    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (client == null) return NotFound("Профиль не найден");

        var service = await _context.Services.FindAsync(request.ServiceId);
        if (service == null) return BadRequest("Услуга не найдена");

        var firstEmployee = await _context.Employees.FirstOrDefaultAsync();
        int employeeId = firstEmployee?.Id ?? 1;

        var order = new Order
        {
            ClientId = client.Id,
            ServiceId = request.ServiceId,
            EmployeeId = employeeId,
            Address = request.Address ?? client.Address ?? "",
            Status = "Новый",
            OrderDate = DateTime.Now
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var createdOrder = await _context.Orders
            .Include(o => o.Service)
            .Include(o => o.Client)
            .FirstOrDefaultAsync(o => o.Id == order.Id);

        return Ok(createdOrder);
    }

    [HttpGet("services")]
    public async Task<IActionResult> GetServices()
    {
        var services = await _context.Services.ToListAsync();
        return Ok(services);
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return null;
        return int.Parse(userIdClaim.Value);
    }
}

public class CreateOrderRequest
{
    public int ServiceId { get; set; }
    public string? Address { get; set; }
}