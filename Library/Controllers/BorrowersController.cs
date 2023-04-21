using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using Library.Repositories;
using System.Runtime.CompilerServices;
using Library.ViewModel;

namespace Library.Controllers
{
    public class BorrowersController : Controller
    {
        //private readonly LibraryContext _context;
        private readonly IGenericRepository<Borrower> _context;

        public BorrowersController(IGenericRepository<Borrower> context)
        {
            _context = context;
        }

        // GET: Borrowers
        public async Task<IActionResult> Index(string BorrowerSearch)
        {
            //return _context.GetAll() != null ?
            //            View(_context.GetAll()) :
            //            Problem("Entity set 'LibraryContext.Borrower'  is null.");
            if (_context.GetAll() == null)
            {
                return Problem("Entity set 'MvcLibraryContext.Item'  is null.");
            }
            var borrowers = _context.GetAll();

            if (!string.IsNullOrEmpty(BorrowerSearch))
            {
                borrowers = borrowers.OrderBy(x => x.Id).Where(x => x.Name!.ToUpper().Contains(BorrowerSearch.ToUpper())!);
            }
            var borrowerVM = new BorrowerViewModel
            {
                Borrowers = borrowers.ToList()
            };

            return View(borrowerVM);
        }

        // GET: Borrowers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (_context.GetAll() == null)
            {
                return NotFound();
            }

            var borrower = await _context.Get(id);
            if (borrower == null)
            {
                return NotFound();
            }

            return View(borrower);
        }

        // GET: Borrowers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Borrowers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address")] Borrower borrower)
        {
            if (ModelState.IsValid)
            {
                await _context.Create(borrower);
                return RedirectToAction(nameof(Index));
            }
            return View(borrower);
        }

        // GET: Borrowers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (_context.GetAll() == null)
            {
                return NotFound();
            }

            var borrower = await _context.Get(id);
            if (borrower == null)
            {
                return NotFound();
            }
            return View(borrower);
        }

        // POST: Borrowers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] Borrower borrower)
        {
            if (id != borrower.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Update(borrower);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Get(borrower.Id) == null)
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
            return View(borrower);
        }

        // GET: Borrowers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.GetAll() == null)
            {
                return NotFound();
            }

            var borrower = await _context.Get(id);
            if (borrower == null)
            {
                return NotFound();
            }

            return View(borrower);
        }

        // POST: Borrowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GetAll() == null)
            {
                return Problem("Entity set 'LibraryContext.Borrower'  is null.");
            }
            var borrower = await _context.Get(id);
            if (borrower != null)
            {
                await _context.Delete(borrower);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
