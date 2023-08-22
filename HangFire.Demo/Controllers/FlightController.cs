using HangFire.Demo.Models;
using HangFire.Demo.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangFire.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : Controller
    {
        public static List<Flight> flights = new List<Flight>();
        private readonly IserviceManagement _iserviceManagement;

        public FlightController(IserviceManagement iserviceManagement)
        {
            _iserviceManagement = iserviceManagement;
        }
        [HttpPost]
        public IActionResult AddFlight(Flight flight)
        {
            if (ModelState.IsValid)
            {
                flights.Add(flight);
                _iserviceManagement.InsertRecords(flight);
                BackgroundJob.Enqueue<IserviceManagement>(x => x.SendEmail());
                return CreatedAtAction("GetPilot", new { flight.Id }, flight);
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var flights = _iserviceManagement.GetAllRecords();

            var flight = flights.FirstOrDefault(x => x.Id == id);
            if (flight == null)
                return NotFound();
            BackgroundJob.Enqueue<IserviceManagement>(x => x.SyncData());
            return Ok(flight);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
