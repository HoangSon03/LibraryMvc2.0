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
using Library.UnitOfWork;
using System.Security.Policy;

namespace Library.Controllers
{
    public class BorrowingHistoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BorrowingHistoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: BorrowingHistories
        public async Task<IActionResult> Index()
        {
            var libraryContext =  _unitOfWork.BorrowingHistories.GetAll();
            return View(libraryContext);
        }

        // GET: BorrowingHistories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (_unitOfWork.BorrowingHistories.GetAll() == null)
            {
                return NotFound();
            }

            var borrowingHistory = await _unitOfWork.BorrowingHistories.Get(id);
            var borrowingDetail = _unitOfWork.BorrowingDetails.GetAllById(id);
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
            ViewData["BorrowerId"] = new SelectList(_unitOfWork.Borrowers.GetAll(), "Id", "Name");
            var ListItem = _unitOfWork.Items.GetAll();
            var HistoryVM = new HistoryViewModel();
            HistoryVM.ListItem = ListItem.ToList();

            return View(HistoryVM);
        }

        // POST: BorrowingHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        //[Bind("Id,BorrowerId")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HistoryViewModel HistoryVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var borrowing = new BorrowingHistory();
                    borrowing.BorrowDate = DateTime.Now;
                    borrowing.BorrowerId = HistoryVM.BorrowerId;
                    borrowing.Status = "Chưa Hoàn Thành";
                    await _unitOfWork.BorrowingHistories.Create(borrowing);

                    HistoryVM.ListSelectQuantity.RemoveAll(x => x == 0);
                    int CountItem = 0;
                    int CountItemQuantity = 0;
                    decimal TotalCost = 0;
                    int i = 0;
                    foreach (int item in HistoryVM.ListSelectItem)
                    {
                        var SelectedItem = await _unitOfWork.Items.Get(item);
                        SelectedItem.Quantity = SelectedItem.Quantity - HistoryVM.ListSelectQuantity[i];
                        await _unitOfWork.Items.Update(SelectedItem);

                        var borrowDetail = new BorrowingDetail();
                        borrowDetail.BorrowingHistoryId = borrowing.Id;
                        borrowDetail.ItemId = item;
                        borrowDetail.Quantity = HistoryVM.ListSelectQuantity[i];
                        borrowDetail.Cost = SelectedItem.Price * borrowDetail.Quantity;
                        borrowDetail.ReturnDate = null;
                        await _unitOfWork.BorrowingDetails.Create(borrowDetail);

                        i++;
                        //i--;
                        CountItem++;
                        CountItemQuantity += borrowDetail.Quantity;
                        TotalCost += borrowDetail.Cost;
                    }
                    borrowing.TotalCost = TotalCost;
                    borrowing.CountItem = CountItem;
                    borrowing.CountItemQuantity = CountItemQuantity;
                    await _unitOfWork.BorrowingHistories.Update(borrowing);

                    await _unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {

            }
            var ListItem = _unitOfWork.Items.GetAll();
            HistoryVM.ListItem = ListItem.ToList();
            ViewData["BorrowerId"] = new SelectList(_unitOfWork.Borrowers.GetAll(), "Id", "Name");

            return View(HistoryVM);
        }

        // GET: BorrowingHistories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (_unitOfWork.BorrowingHistories.GetAll() == null)
            {
                return NotFound();
            }

            var borrowingHistory = await _unitOfWork.BorrowingHistories.Get(id);
            var borrowingDetail = _unitOfWork.BorrowingDetails.GetAllById(id);
            var DetailVM = new DetailViewModel();
            DetailVM.BorrowingDetail = borrowingDetail.ToList();
            DetailVM.Borrower = borrowingHistory.Borrower;
            //DetailVM.BorrowDate = borrowingHistory.BorrowDate;
            DetailVM.BorrowingHistory = borrowingHistory;
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
                    var borrowDetail = await _unitOfWork.BorrowingDetails.Get(DetailId);
                    borrowDetail.ReturnDate = DateTime.Now;
                    await _unitOfWork.BorrowingDetails.Update(borrowDetail);

                    var SelectedItem = await _unitOfWork.Items.Get(ItemId);
                    SelectedItem.Quantity = SelectedItem.Quantity + borrowDetail.Quantity;
                    await _unitOfWork.Items.Update(SelectedItem);

                    var borrowingHistory = await _unitOfWork.BorrowingHistories.Get(id);
                    DetailVM.BorrowingDetail = borrowingHistory.BorrowingDetails;
                    DetailVM.BorrowingHistory = borrowingHistory;
                    DetailVM.Borrower = borrowingHistory.Borrower!;
                    //DetailVM.BorrowDate = borrowingHistory.BorrowDate;

                    var borrowingDetail = _unitOfWork.BorrowingDetails.GetAllById(id);
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
                        await _unitOfWork.BorrowingHistories.Update(borrowingHistory);
                    }
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_unitOfWork.BorrowingHistories.Get(DetailId) == null)
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
            if (_unitOfWork.BorrowingHistories.GetAll() == null)
            {
                return NotFound();
            }

            var borrowingHistory = await _unitOfWork.BorrowingHistories.Get(id);
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
            if (_unitOfWork.BorrowingHistories.GetAll() == null)
            {
                return Problem("Entity set 'LibraryContext.BorrowingHistory'  is null.");
            }
            var detailHistory = _unitOfWork.BorrowingDetails.GetAllById(id);
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

            var borrowingHistory = await _unitOfWork.BorrowingHistories.Get(id);
            if (borrowingHistory != null)
            {
                await _unitOfWork.BorrowingDetails.DeleteMany(detailHistory);
                await _unitOfWork.BorrowingHistories.Delete(borrowingHistory);
                await _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
