using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlaylistMVC.Models;

namespace PlaylistMVC.Controllers
{
    public class SongInfoController : Controller
    {
        private readonly MyDbContext _context;

        public SongInfoController(MyDbContext context)
        {
            _context = context;
        }

        // GET: SongInfo
        public async Task<IActionResult> Index()
        {
              return _context.SongInfo != null ? 
                          View(await _context.SongInfo.ToListAsync()) :
                          Problem("Entity set 'MyDbContext.SongInfo'  is null.");
        }

        // GET: SongInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SongInfo == null)
            {
                return NotFound();
            }

            var songInfo = await _context.SongInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (songInfo == null)
            {
                return NotFound();
            }

            return View(songInfo);
        }

        // GET: SongInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SongInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SongName,ArtistName")] SongInfo songInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(songInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(songInfo);
        }

        // GET: SongInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SongInfo == null)
            {
                return NotFound();
            }

            var songInfo = await _context.SongInfo.FindAsync(id);
            if (songInfo == null)
            {
                return NotFound();
            }
            return View(songInfo);
        }

        // POST: SongInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SongName,ArtistName")] SongInfo songInfo)
        {
            if (id != songInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(songInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongInfoExists(songInfo.Id))
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
            return View(songInfo);
        }

        // GET: SongInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SongInfo == null)
            {
                return NotFound();
            }

            var songInfo = await _context.SongInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (songInfo == null)
            {
                return NotFound();
            }

            return View(songInfo);
        }

        // POST: SongInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SongInfo == null)
            {
                return Problem("Entity set 'MyDbContext.SongInfo'  is null.");
            }
            var songInfo = await _context.SongInfo.FindAsync(id);
            if (songInfo != null)
            {
                _context.SongInfo.Remove(songInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongInfoExists(int id)
        {
          return (_context.SongInfo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
