using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prog_part_2.Data;
using Prog_part_2.Models;

namespace Prog_part_2.Controllers
{
    public class ContractsController : Controller
    {
        private readonly Prog_part_2Context _context;
        private readonly IWebHostEnvironment _environment;

        public ContractsController(Prog_part_2Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        private ContractStatus GetContractStatus(Contracts contract)
        {
            var today = DateTime.Today;

            if (contract.StartDate > today)
                return ContractStatus.Draft;

            if (contract.StartDate <= today && contract.EndDate >= today)
                return ContractStatus.Active;

            if (contract.EndDate < today)
                return ContractStatus.Expired;

            return ContractStatus.Draft;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(string searchstring)
        {
            if (_context.Contracts == null)
                return Problem("Entity set not found");

            var contracts = from c in _context.Contracts
                            select c;

            if (!string.IsNullOrEmpty(searchstring))
            {
                searchstring = searchstring.ToLower();

                contracts = contracts.Where(c =>
                    c.ContractName.ToLower().Contains(searchstring) ||
                    c.StartDate.ToString().Contains(searchstring) ||
                    c.EndDate.ToString().Contains(searchstring)
                );
            }

            return View(await contracts.ToListAsync());
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var contract = await _context.Contracts
       .Include(c => c.Files)
       .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
                return NotFound();


            contract.ContractStatus = GetContractStatus(contract);
            await _context.SaveChangesAsync();

            ViewBag.Status = contract.ContractStatus;

            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId,ContractName,ContractDescription,ContractType,StartDate,EndDate")] Contracts contract)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = contract.Id });
            }
            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            return View(contract);
        }

        // POST: Contracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contracts contract)
        {
            if (id != contract.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Contracts.Any(e => e.Id == contract.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var contract = await _context.Contracts
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract != null)
                _context.Contracts.Remove(contract);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Upload(int contractId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a file.";
                return RedirectToAction("Details", new { id = contractId });
            }

            if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Only PDF files are allowed.";
                return RedirectToAction("Details", new { id = contractId });
            }

            var contract = await _context.Contracts.FindAsync(contractId);
            if (contract == null)
                return NotFound();

            string folder = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(folder, uniqueName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var contractFile = new ContractFile
            {
                FileName = file.FileName,
                FilePath = uniqueName,
                FileSize = file.Length,
                UploadedDate = DateTime.Now,
                ContractId = contractId
            };

            _context.ContractFiles.Add(contractFile);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = contractId });
        }
        public IActionResult DownloadFile(int fileId)
        {
            var file = _context.ContractFiles.FirstOrDefault(f => f.Id == fileId);

            if (file == null)
                return NotFound();

            string path = Path.Combine(_environment.WebRootPath, "uploads", file.FilePath);

            if (!System.IO.File.Exists(path))
                return NotFound();

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", file.FileName);
        }

    }

}