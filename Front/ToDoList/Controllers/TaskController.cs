using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.API;
using System.Security.Claims;

namespace ToDoList.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApiService _apiService;

        public TaskController(IHttpContextAccessor httpContextAccessor)
        {
            _apiService = new ApiService(httpContextAccessor);
        }

        public IActionResult TaskCreate()
        {
            
            return View();
        }

        public async Task<IActionResult> TaskAdd(TaskModel taskModel)
        {
            if(taskModel == null)
            {
                return BadRequest();
            }
            await _apiService.AddTaskAsync(taskModel);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> TaskUpdate(long id)
        {

            var tasks = await _apiService.GetTaskApi(id);
            if (tasks != null)
            {
                return View(tasks);
            }
            else
            {
                // Handle the case where tasksList is null or empty
                return BadRequest(string.Empty);
            }
            
        }

        public async Task<IActionResult> TaskEdit(TaskModel taskModel)
        {
            if (taskModel == null)
            {
                return BadRequest();
            }
            
            var usr = await _apiService.GetCurrentUser();
            if (usr.Id==taskModel.UserId)
            {
                taskModel.User = usr;
                taskModel.User.PasswordHash = "0";
                if (await _apiService.EditTaskApi(taskModel))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return BadRequest();
        }

        public async Task<ActionResult> DeleteTask(long id, long UserId)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            long usrId = (await _apiService.GetCurrentUser()).Id;
            if (usrId == UserId)
            {
                if (await _apiService.DeleteTaskApi(id))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return BadRequest();
        }

       

    }
}
