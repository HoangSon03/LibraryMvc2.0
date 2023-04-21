using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;

namespace Library.Controllers
{
    public class BorrowingDetailsController : Controller
    {
        private readonly LibraryContext _context;


        public BorrowingDetailsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: BorrowingDetails
        public async Task<IActionResult> Index()
        {
            var libraryContext = _context.BorrowingDetail!.Include(b => b.BorrowingHistory).Include(b => b.Item);
            return View(await libraryContext.ToListAsync());
        }

        // GET: BorrowingDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BorrowingDetail == null)
            {
                return NotFound();
            }

            var borrowingDetail = await _context.BorrowingDetail
                .Include(b => b.BorrowingHistory)
                .Include(b => b.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowingDetail == null)
            {
                return NotFound();
            }

            return View(borrowingDetail);
        }

        // GET: BorrowingDetails/Create
        public IActionResult Create()
        {
            ViewData["BorrowingHistoryId"] = new SelectList(_context.BorrowingHistory, "Id", "Id");
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id");
            return View();
        }

        // POST: BorrowingDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BorrowingHistoryId,ItemId,Quantity,Cost")] BorrowingDetail borrowingDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(borrowingDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BorrowingHistoryId"] = new SelectList(_context.BorrowingHistory, "Id", "Id", borrowingDetail.BorrowingHistoryId);
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", borrowingDetail.ItemId);
            return View(borrowingDetail);
        }

        // GET: BorrowingDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BorrowingDetail == null)
            {
                return NotFound();
            }

            var borrowingDetail = await _context.BorrowingDetail.FindAsync(id);
            if (borrowingDetail == null)
            {
                return NotFound();
            }
            ViewData["BorrowingHistoryId"] = new SelectList(_context.BorrowingHistory, "Id", "Id", borrowingDetail.BorrowingHistoryId);
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", borrowingDetail.ItemId);
            return View(borrowingDetail);
        }

        // POST: BorrowingDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BorrowingHistoryId,ItemId,Quantity,Cost")] BorrowingDetail borrowingDetail)
        {
            if (id != borrowingDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowingDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowingDetailExists(borrowingDetail.Id))
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
            ViewData["BorrowingHistoryId"] = new SelectList(_context.BorrowingHistory, "Id", "Id", borrowingDetail.BorrowingHistoryId);
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", borrowingDetail.ItemId);
            return View(borrowingDetail);
        }

        // GET: BorrowingDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BorrowingDetail == null)
            {
                return NotFound();
            }

            var borrowingDetail = await _context.BorrowingDetail
                .Include(b => b.BorrowingHistory)
                .Include(b => b.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowingDetail == null)
            {
                return NotFound();
            }

            return View(borrowingDetail);
        }

        // POST: BorrowingDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BorrowingDetail == null)
            {
                return Problem("Entity set 'LibraryContext.BorrowingDetail'  is null.");
            }
            var borrowingDetail = await _context.BorrowingDetail.FindAsync(id);
            if (borrowingDetail != null)
            {
                _context.BorrowingDetail.Remove(borrowingDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowingDetailExists(int id)
        {
          return (_context.BorrowingDetail?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
