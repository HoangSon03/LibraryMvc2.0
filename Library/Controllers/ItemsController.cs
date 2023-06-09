﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using System.Security.Cryptography.Xml;
using Library.Repositories;
using Library.ViewModel;

namespace Library.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IGenericRepository<Item> _context;


        public ItemsController(IGenericRepository<Item> context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index(string ItemSearch, string ItemYear, Category? ItemCategory)
        {
            if (_context.GetAll() == null)
            {
                return Problem("Entity set 'MvcLibraryContext.Item'  is null.");
            }
            var CateQuery = _context.GetAll().OrderBy(x => x.Category)
                                             .Select(x => x.Category.ToString()!);

            var YearQuery = _context.GetAll().OrderBy(x => x.PublishDate.Year)
                                             .Select(x => x.PublishDate.Year.ToString()!);

            var items = _context.GetAll();

            if (!string.IsNullOrEmpty(ItemSearch))
            {
                items = items.OrderBy(x => x.Id).Where(x => x.Title!.ToUpper().Contains(ItemSearch.ToUpper())!);
            }

            if (ItemCategory != null)
            {
                items = items.OrderBy(x => x.Id).Where(x => x.Category == ItemCategory);
            }

            if (!string.IsNullOrEmpty(ItemYear))
            {
                items = items.OrderBy(x => x.Id).Where(x => x.PublishDate.Year.ToString() == ItemYear);
            }

            var itemVM = new ItemViewModel
            {
                Categories = new SelectList(CateQuery.Distinct().ToList()),
                Years = new SelectList(YearQuery.Distinct().ToList()),
                Items = items.ToList()
            };

            return View(itemVM);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (_context.GetAll() == null)
            {
                return NotFound();
            }

            var item = await _context.Get(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(Enum.GetValues(typeof(Category)));
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,PublishDate,RunTime,NumOfPage,Quantity,Price,Category")] Item item)
        {
            if (ModelState.IsValid)
            {
               await _context.Create(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if ( _context.GetAll() == null)
            {
                return NotFound();
            }

            var item = await _context.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
            //return View();
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,PublishDate,RunTime,NumOfPage,Quantity,Price,Category")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await _context.Update(item);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Get(item.Id) == null)
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
            return View(item);
            //return View();
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if ( _context.GetAll() == null)
            {
                return NotFound();
            }

            var item = await _context.Get(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);

        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GetAll() == null)
            {
                return Problem("Entity set 'LibraryContext.Item'  is null.");
            }
            var item = await _context.Get(id);
            if (item != null)
            {
                await _context.Delete(item);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
