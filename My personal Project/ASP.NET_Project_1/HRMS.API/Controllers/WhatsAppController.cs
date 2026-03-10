using BCrypt.Net;
using HRMS.API.DTOs;
using HRMS.DAL.Data;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsAppController : ControllerBase
    {
        private readonly HRMSDbContext _context;

        public WhatsAppController(HRMSDbContext context)
        {
            _context = context;
        }
        // Verification
        //[HttpGet("webhook")]
        //public IActionResult Verify(
        //    [FromQuery(Name = "hub.mode")] string mode,
        //    [FromQuery(Name = "hub.verify_token")] string token,
        //    [FromQuery(Name = "hub.challenge")] string challenge)
        //{
        //    if (mode == "subscribe" && token == "12345")
        //        return Ok(challenge);

        //    return Unauthorized();
        //}

        // Receive message
        [HttpPost("webhook")]
        public async Task<IActionResult> Receive([FromBody] WhatsAppPayload payload)
        {
            try
            {
                var msg = payload.entry[0].changes[0].value.messages[0];

                string phone = msg.from;
                string text = msg.text.body;

                var message = new Message
                {
                    PhoneNumber = phone,
                    MessageText = text,
                    CreatedDate = DateTime.Now
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error to see what went wrong
                return BadRequest(ex.Message);
            }

            return Ok();
        }
        [HttpGet("check-retailer")]
        public async Task<IActionResult> CheckRetailer(string phone)
        {
            var retailer = await _context.Retailers
                .FirstOrDefaultAsync(r => r.Phone == phone);

            if (retailer == null)
            {
                return Ok(new
                {
                    registered = false
                });
            }

            return Ok(new
            {
                registered = true,
                shop_name = retailer.ShopName
            });
        }
        [HttpPost("register-retailer")]
        public async Task<IActionResult> RegisterRetailer([FromBody] Retailer retailer)
        {
            retailer.CreatedAt = DateTime.Now;

            _context.Retailers.Add(retailer);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Retailer registered successfully"
            });
        }

        [HttpGet("session")]
        public async Task<IActionResult> GetSession([FromQuery] string phone)
        {
            var session = await _context.WhatsAppSessions
                .FirstOrDefaultAsync(s => s.Phone == phone);

            if (session == null)
            {
                return Ok(new WhatsAppSessions
                {
                    Phone = phone,
                    CurrentStep = "MENU",
                    TempData = "",
                    UpdatedAt = DateTime.Now
                });
            }

            return Ok(session);
        }
        // Update session
        [HttpPost("session")]
        public async Task<IActionResult> UpdateSession([FromBody] WhatsAppSessions session)
        {
            var existing = await _context.WhatsAppSessions
                .FirstOrDefaultAsync(s => s.Phone == session.Phone);
            if (existing == null)
                _context.WhatsAppSessions.Add(session);
            else
            {
                existing.CurrentStep = session.CurrentStep;
                existing.TempData = session.TempData;
                existing.UpdatedAt = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("getCategories")]
        public async Task<ActionResult<IEnumerable<Categories>>> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<IEnumerable<SubCategories>>> GetSubCategoriesByCategory(int categoryId)
        {
            // Check if category exists
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == categoryId);
            if (!categoryExists)
            {
                return NotFound(new { message = "Category not found" });
            }

            // Get subcategories with prices
            var subCategories = await _context.SubCategories
                .Where(s => s.CategoryId == categoryId)
                .Include(s => s.Prices) // include price info
                .ToListAsync();

            if (subCategories == null || !subCategories.Any())
            {
                return NotFound(new { message = "No subcategories found for this category" });
            }

            // Optional: format result for cleaner JSON
            var result = subCategories.Select(s => new
            {
                s.Id,
                s.Name,
                s.Description,
                s.IsActive,
                Prices = s.Prices.Select(p => new
                {
                    p.Price,
                    p.EffectiveDate
                })
            });

            return Ok(result);
        }

    }
}