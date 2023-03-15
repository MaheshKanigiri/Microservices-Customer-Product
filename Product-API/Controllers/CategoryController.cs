using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_API.Context;
using Product_API.Models;

namespace Product_API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController
    {
        private readonly StoreContext _context;

        public CategoryController(StoreContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Category>> GetById(int id)
        //{
        //    var category = await _context.Categories.FindAsync(id);

        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return category;
        //}

        //[HttpPost]
        //public async Task<ActionResult<Category>> Create(Category category)
        //{
        //    _context.Categories.Add(category);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, Category category)
        //{
        //    if (id != category.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(category).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!_context.Categories.Any(c => c.Id == id))
        //        {
        //            return NotFound();
        //        }

        //        throw;
        //    }

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var category = await _context.Categories.FindAsync(id);

        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Categories.Remove(category);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
