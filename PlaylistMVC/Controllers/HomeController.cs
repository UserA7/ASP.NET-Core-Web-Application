using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlaylistMVC.Models;
using System.Diagnostics;

namespace PlaylistMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> GetAllTimeSongs()
        {
            using(var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", "fd598c74cbmsh70010f8e638b9c8p115781jsn56068c4a489d");
                string apiUrl = "https://billboard3.p.rapidapi.com/greatest-hot-100-singles";

                HttpResponseMessage response = await _httpClient.GetAsync($"{apiUrl}?range=1-20");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    List<AllTimeWorld> topChart = JsonConvert.DeserializeObject<List<AllTimeWorld>>(result);
                    return View("AllTimeResult1", topChart);
                }
                else
                {
                    // Handle the error scenario, e.g., return an error view
                    return View("Error");
                }

            }
        }

        [HttpPost]
        public async Task<IActionResult> GetTopByMonthlyListeners()
        {
            using (var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", "fd598c74cbmsh70010f8e638b9c8p115781jsn56068c4a489d");
                string apiUrl = "https://spotify81.p.rapidapi.com/top_20_by_monthly_listeners";

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    List<TopByMonthlyListeners> topByMonthlyListenersData = JsonConvert.DeserializeObject<List<TopByMonthlyListeners>>(result);
                    return View("TopByMonthlyListeners", topByMonthlyListenersData);
                }
                else
                {
                    // Handle the error scenario, e.g., return an error view
                    return View("Error");
                }

            }
        }
    }
}