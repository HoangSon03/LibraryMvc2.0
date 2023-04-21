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
using Library.ViewModel;
using System.Collections.Immutable;

namespace Library.Controllers
{
    public class BorrowingHistoriesController : Controller
    {
        private readonly IGenericRepository<BorrowingHistory> _context;
        private readonly IGenericRepository<Borrower> _borrower;
        private readonly IGenericRepository<Item> _item;
        private readonly IGenericRepository<BorrowingDetail> _detail;

        public BorrowingHistoriesController(
            IGenericRepository<BorrowingHistory> context,
            IGenericRepository<Borrower> borrower,
            IGenericRepository<Item> item,
            IGenericRepository<BorrowingDetail> detail
            )
        {
            _context = context;
            _borrower = borrower;
            _item = item;
            _detail = detail;
        }

        // GET: BorrowingHistories
        public async Task<IActionResult> Index()
        {
            var libraryContext = _context.GetAll();
            return View(libraryContext);
        }

        // GET: BorrowingHistories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (_context.GetAll() == null)
            {
                return NotFound();
            }

            var borrowingHistory = await _context.Get(id);
            var borrowingDetail = _detail.GetAllById(id);
            borrowingHistory.BorrowingDetails = borrowingDetail.ToList();
            if (borrowingHistory == null)
            {
                return NotFound();
            }

            return View(borrowingHistory);
        }

        // GET: BorrowingHistories/Create
        public IActionResult Create()
        {
            ViewData["BorrowerId"] = new SelectList(_borrower.GetAll(), "Id", "Name");
            ViewData["ItemId"] = new SelectList(_item.GetAll(), "Id", "Title");

            return View();
        }

        // POST: BorrowingHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        //[Bind("Id,BorrowerId")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HistoryViewModel HistoryVM)
        {
            if (ModelState.IsValid)
            {
                var borrowing = new BorrowingHistory();
                borrowing.BorrowDate = DateTime.Now;
                borrowing.BorrowerId = HistoryVM.BorrowerId;
                borrowing.Status = "Chưa Hoàn Thành";
                await _context.Create(borrowing);

                int CountItem = 0;
                int CountItemQuantity = 0;
                decimal TotalCost = 0;

                foreach (int item in HistoryVM.ListItemId!)
                {
                    var SelectedItem = await _item.Get(item);
                    SelectedItem.Quantity = SelectedItem.Quantity - 1;
                    await _item.Update(SelectedItem);

                    var borrowDetail = new BorrowingDetail();
                    borrowDetail.BorrowingHistoryId = borrowing.Id;
                    borrowDetail.ItemId = item;
                    borrowDetail.Quantity = 1;
                    borrowDetail.Cost = SelectedItem.Price * borrowDetail.Quantity;
                    borrowDetail.ReturnDate = null;
                    await _detail.Create(borrowDetail);

                    CountItem++;
                    CountItemQuantity += borrowDetail.Quantity;
                    TotalCost += borrowDetail.Cost;
                }
                borrowing.TotalCost = TotalCost;
                borrowing.CountItem = CountItem;
                borrowing.CountItemQuantity = CountItemQuantity;
                await _context.Update(borrowing);

                return RedirectToAction(nameof(Index));

            }
            ViewData["BorrowerId"] = new SelectList(_borrower.GetAll(), "Id", "Name");
            ViewData["ItemId"] = new SelectList(_item.GetAll(), "Id", "Title");

            return View(HistoryVM);
        }

        // GET: BorrowingHistories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (_context.GetAll() == null)
            {
                return NotFound();
            }
            
            var borrowingHistory = await _context.Get(id);
            var borrowingDetail =  _detail.GetAllById(id);
            var DetailVM = new DetailViewModel();
            DetailVM.BorrowingDetail = borrowingDetail.ToList();
            DetailVM.Borrower = borrowingHistory.Borrower;
            DetailVM.BorrowDate = borrowingHistory.BorrowDate;

            if (DetailVM == null)
            {
                return NotFound();
            }
            return View(DetailVM);
        }

        // POST: BorrowingHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        //, [Bind("Id,BorrowerId")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int DetailId, int ItemId)
        {
            //if (id != borrowingHistory.Id)
            //{
            //    return NotFound();
            //}
            var DetailVM = new DetailViewModel();

            if (ModelState.IsValid)
            {
                try
                {
                    var borrowDetail = await _detail.Get(DetailId);
                    borrowDetail.ReturnDate = DateTime.Now;
                    await _detail.Update(borrowDetail);

                    var SelectedItem = await _item.Get(ItemId);
                    SelectedItem.Quantity = SelectedItem.Quantity + 1;
                    await _item.Update(SelectedItem);

                    var borrowingHistory = await _context.Get(id);
                    DetailVM.BorrowingDetail = borrowingHistory.BorrowingDetails;
                    DetailVM.Borrower = borrowingHistory.Borrower!;
                    DetailVM.BorrowDate = borrowingHistory.BorrowDate;

                    var borrowingDetail = _detail.GetAllById(id);
                    int temp = 0;
                    foreach (var item in borrowingDetail)
                    {
                        if (item.ReturnDate == null)
                        {
                            temp++;
                        }
                    }
                    if (temp == 0)
                    {
                        borrowingHistory.Status = "Hoàn Thành";
                        await _context.Update(borrowingHistory);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Get(DetailId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(DetailVM);
        }

        // GET: BorrowingHistories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.GetAll() == null)
            {
                return NotFound();
            }

            var borrowingHistory = await _context.Get(id);
            if (borrowingHistory == null)
            {
                return NotFound();
            }

            return View(borrowingHistory);
        }

        // POST: BorrowingHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GetAll() == null)
            {
                return Problem("Entity set 'LibraryContext.BorrowingHistory'  is null.");
            }
            var detailHistory = _detail.GetAllById(id);
            int x = 0;
            foreach (var item in detailHistory)
            {
                if (item.ReturnDate == null)
                {
                    x++;
                }
            }
            if (x > 0)
            {
                return Problem("This BorrowingHistory is not return all item");
            }

            var borrowingHistory = await _context.Get(id);
            if (borrowingHistory != null)
            {
                await _detail.DeleteMany(detailHistory);
                await _context.Delete(borrowingHistory);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
