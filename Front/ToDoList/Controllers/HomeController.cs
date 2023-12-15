using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using ToDoList.Models;
using ToDoList.API;
using System.Security.Claims;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiService _apiService;

        public List<TaskModel> tasksList { get; set; } = new List<TaskModel>();

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _apiService = new ApiService(httpContextAccessor);
        }

        public async Task<IActionResult> Index()
        {
            tasksList = await _apiService.GetTaskListAsync();

            if (tasksList != null && tasksList.Any())
            {

                return View(tasksList);
            }
            else
            {
                // Handle the case where tasksList is null or empty

                return View(new List<TaskModel>());

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}