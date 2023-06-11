using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlaylistMVC.Models;
using System.Net.Http;
using System.Net.Http.Json;
using PlaylistMVC.Models.ApiModels;

namespace PlaylistMVC.Controllers
{
    public class SongDetailsController : Controller
    {
        private readonly HttpClient _httpClient;

        public SongDetailsController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", "fd598c74cbmsh70010f8e638b9c8p115781jsn56068c4a489d");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
    public async Task<IActionResult> GetSongDetails(string songName)
    {
        string apiUrl = "https://shazam-core7.p.rapidapi.com/search";

        HttpResponseMessage response = await _httpClient.GetAsync($"{apiUrl}?term={songName}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            SearchResponse songDetails = JsonConvert.DeserializeObject<SearchResponse>(result);
            return View("SongDetails", songDetails);
        }
        else
        {
            // Handle the error scenario, e.g., return an error view
            return View("Error");
        }
    }
    }
}
