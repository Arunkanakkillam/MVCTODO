using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sample_.Context;
using Sample_.Models;

namespace Sample_.Controllers
{
    public class TodoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }
       public async Task<IActionResult> Index()
        {
            return View(await _context.todoItems.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Task,IsCompleted")]TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(todoItem);
        }
        public async Task<IActionResult>Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var todoitem=await _context.todoItems.FindAsync(id);
            if (todoitem == null) 
            {
                return NotFound();
            }
            return View(todoitem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Task,IsCompleted")] TodoItem todoitem)
        {
            if (id != todoitem.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid) 
            {
                try
                {
                    _context.Update(todoitem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) 
                {
                    if (!TodoItemExists(todoitem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoitem);
        }
        private bool TodoItemExists(int id)
        {
            return _context.todoItems.Any(e => e.Id == id);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var todoItem = await _context.todoItems.FirstOrDefaultAsync(m => m.Id == id);
            if (todoItem == null) 
            {
                return NotFound();
            }
            return View(todoItem);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoitem= await _context.todoItems.FirstOrDefaultAsync(e => e.Id == id);
             _context.todoItems.Remove(todoitem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
