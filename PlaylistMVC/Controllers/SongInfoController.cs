using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using PlaylistMVC.Migrations.MyDb;
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
                          View(await _context.SongInfo.Include(song => song.SongInfoGenres).ThenInclude(g => g.Genre).ToListAsync()) :
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
            var genres = _context.Genres;
            var model = new CreateSongInfoModelView { Genres = GenresToCheckboxViewModel(genres.ToList()), SongInfo = new SongInfo() };
            return View(model);
        }

        private List<CheckboxViewModel> GenresToCheckboxViewModel(List<Genre> genres, List<SongInfoGenre> dbGenres = null)
        {
            List<CheckboxViewModel> checkboxViewModels = new List<CheckboxViewModel>();
            if (!genres.Any())
            {
                return checkboxViewModels;
            }
            foreach (var genre in genres)
            {
                bool isChecked = false;
                if(dbGenres != null)
                {
                    isChecked = dbGenres.Where(g => g.Genre.Id == genre.Id).Any();
                }
                var model = new CheckboxViewModel { Id = genre.Id, LabelName = genre.Name, IsChecked = isChecked };
                checkboxViewModels.Add(model);
            }
            return checkboxViewModels;
        }

        // POST: SongInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CreateSongInfoModelView model)
        {
            if (ModelState.IsValid)
            {
                var genres = model.Genres.Where(gen => gen.IsChecked).ToList();
                List<SongInfoGenre> dbGenres = new List<SongInfoGenre>(); 
                
                CreateGenres(genres, dbGenres, model.SongInfo);
                model.SongInfo.SongInfoGenres = dbGenres;
                _context.Add(model.SongInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model.SongInfo);
        }

        // GET: SongInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SongInfo == null)
            {
                return NotFound();
            }

            var songInfo = _context.SongInfo.Include(s => s.SongInfoGenres).ThenInclude(g => g.Genre).FirstOrDefault(s => s.Id == id);
            if (songInfo == null)
            {
                return NotFound();
            }
            var genres = _context.Genres;
            var model = new CreateSongInfoModelView { Genres = GenresToCheckboxViewModel(genres.ToList(), songInfo.SongInfoGenres), SongInfo = songInfo };
            return View(model);
        }

        // POST: SongInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateSongInfoModelView songInfoModel)
        {
            if (id != songInfoModel.SongInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbSongInfo = await _context.SongInfo.Include(s => s.SongInfoGenres).ThenInclude(g => g.Genre).FirstAsync(s => s.Id == id);
                    if (dbSongInfo == null)
                    {
                        return NotFound();
                    }
                    var inputGenres = songInfoModel.Genres.Where(g => g.IsChecked).ToList();
                    var dbGenres = dbSongInfo.SongInfoGenres;
                    
                    HandleGenres(inputGenres, dbGenres, dbSongInfo);

                    dbSongInfo.ArtistName = songInfoModel.SongInfo.ArtistName;
                    dbSongInfo.SongName = songInfoModel.SongInfo.SongName;
                    dbSongInfo.SongInfoGenres = dbGenres;
                    _context.Update(dbSongInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongInfoExists(songInfoModel.SongInfo.Id))
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
            return View(songInfoModel.SongInfo);
        }

        private void CreateGenres(List<CheckboxViewModel> inputGenres, List<SongInfoGenre> dbGenres, SongInfo songInfo)
        {

            foreach (var genre in inputGenres)
            {
                var dbGenre = _context.Genres.FirstOrDefault(g => g.Id == genre.Id);
                if (dbGenre == null)
                {
                    continue;
                }
                dbGenres.Add(new SongInfoGenre { Genre = dbGenre, SongInfo = songInfo });
            }
        }

        private void HandleGenres(List<CheckboxViewModel> inputGenres, List<SongInfoGenre> dbGenres, SongInfo dbSongInfo)
        {
            if (dbGenres.Count == 0 && inputGenres.Count > 0)
            {
                CreateGenres(inputGenres, dbGenres, dbSongInfo);
            }
            else if (dbGenres.Count > 0 && inputGenres.Count == 0)
            {
                dbGenres.Clear();
            }
            else if (dbGenres.Count > inputGenres.Count)
            {
                var match = dbGenres.FindAll(m => inputGenres.Any(ig => ig.Id == m.Genre.Id));
                dbGenres.RemoveAll(g => !match.Any(m => m.Id == g.Id));
            }
            else if (inputGenres.Count > dbGenres.Count)
            {
                var match = inputGenres.FindAll(m => !dbGenres.Any(ig => ig.Genre.Id == m.Id));
                CreateGenres(match, dbGenres, dbSongInfo);
            }
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
