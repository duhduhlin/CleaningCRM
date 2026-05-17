using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CleaningCRM.API.Data;
using CleaningCRM.API.Models;
using System.Security.Claims;

namespace CleaningCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Include(o => o.Employee)
                .ToListAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Не удалось определить пользователя");
            }

            int userId = int.Parse(userIdClaim.Value);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                return Ok(new List<Order>());
            }

            var orders = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Where(o => o.EmployeeId == employee.Id)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            if (order.ClientId <= 0 || order.ServiceId <= 0 || order.EmployeeId <= 0 || string.IsNullOrEmpty(order.Address))
            {
                return BadRequest("Заполните все поля");
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var createdOrder = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, createdOrder);
        }

        // Обновление статуса (старый метод, оставляем для совместимости)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            await _context.SaveChangesAsync();

            var updatedOrder = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.Id == id);

            return Ok(updatedOrder);
        }

        // Полное обновление заказа (для редактирования)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderRequest request)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Address = request.Address;
            order.ClientId = request.ClientId;
            order.ServiceId = request.ServiceId;
            order.EmployeeId = request.EmployeeId;
            order.Status = request.Status;

            await _context.SaveChangesAsync();

            var updatedOrder = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.Id == id);

            return Ok(updatedOrder);
        }

        [HttpPut("{id}/complete")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                int userId = int.Parse(userIdClaim.Value);
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);

                if (employee != null && order.EmployeeId != employee.Id)
                    return Forbid("Этот заказ назначен другому сотруднику");
            }

            order.Status = "Выполнен";
            await _context.SaveChangesAsync();

            var updatedOrder = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.Id == id);

            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class UpdateOrderRequest
    {
        public string Address { get; set; }
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public int EmployeeId { get; set; }
        public string Status { get; set; }
    }
}