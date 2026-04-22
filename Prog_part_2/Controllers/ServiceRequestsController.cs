using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prog_part_2.Data;
using Prog_part_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Prog_part_2.Models.ConcreteObserver;

namespace Prog_part_2.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly Prog_part_2Context _context;
        private readonly IHttpClientFactory _clientFactory;
        private const string ApiKey = "df519876d9748b4f06940c70";

        public ServiceRequestsController(Prog_part_2Context context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }
        private ContractStatus GetContractStatus(Contracts contract)
        {
            var today = DateTime.Today;

            if (contract.ContractStatus == ContractStatus.OnHold)
                return ContractStatus.OnHold;

            if (contract.StartDate > today)
                return ContractStatus.Draft;

            if (contract.StartDate <= today && contract.EndDate >= today)
                return ContractStatus.Active;

            if (contract.EndDate < today)
                return ContractStatus.Expired;

            return ContractStatus.Draft;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceRequests.ToListAsync());
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequests = await _context.ServiceRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceRequests == null)
            {
                return NotFound();
            }

            return View(serviceRequests);
        }

        // GET: ServiceRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequests serviceRequests)
        {
            if (!ModelState.IsValid)
            {
                return View(serviceRequests);
            }

            var contract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.Id == serviceRequests.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("", "Invalid contract, contract has the staus of draft");
                return View(serviceRequests);
            }


            contract.ContractStatus = GetContractStatus(contract);


            if (contract.ContractStatus == ContractStatus.Draft ||
                contract.ContractStatus == ContractStatus.OnHold)
            {
                ModelState.AddModelError("",
                    "Cannot create service request because contract is Draft or On Hold.");

                return View(serviceRequests);
            }


            _context.Add(serviceRequests);
            await _context.SaveChangesAsync();


            var notificationSystem = new NotificationSystem();

            notificationSystem.EnableNotifications(new Email());
            notificationSystem.EnableNotifications(new SMS());

            notificationSystem.NotifyObservers(
                $"Service Request #{serviceRequests.Id} created. " +
                $"Contract Status: {contract.ContractStatus}"
            );

            return RedirectToAction(nameof(Index));
        }
        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequests = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequests == null)
            {
                return NotFound();
            }
            return View(serviceRequests);
        }

        // POST: ServiceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServiceStatus,ContractCost,RequestTypes,RequestDate,RequestDescription")] ServiceRequests serviceRequests)
        {
            if (id != serviceRequests.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestsExists(serviceRequests.Id))
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
            return View(serviceRequests);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequests = await _context.ServiceRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceRequests == null)
            {
                return NotFound();
            }

            return View(serviceRequests);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequests = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequests != null)
            {
                _context.ServiceRequests.Remove(serviceRequests);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequestsExists(int id)
        {
            return _context.ServiceRequests.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> Convert(string from, string to, double amount)
        {
            var client = _clientFactory.CreateClient("CurrencyApi");

            try
            {
                string fromUpper = from.ToUpper().Trim();
                string toUpper = to.ToUpper().Trim();

                string relativeUrl = $"{ApiKey}/pair/{fromUpper}/{toUpper}/{amount}";

                var data = await client.GetFromJsonAsync<ExchangeResponse>(relativeUrl);

                if (data != null && data.result == "success")
                {
                    data.target_code = toUpper;
                    data.base_code = fromUpper;
                    data.conversion_result = amount * data.conversion_rate;

            
                    ViewBag.Result = data;
                }
                else
                {
                    ViewBag.Error = "Conversion failed. Invalid response.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Conversion failed: {ex.Message}";
            }

          
            var serviceRequests = await _context.ServiceRequests.ToListAsync();

            return View("Index", serviceRequests);
        }
    }
}