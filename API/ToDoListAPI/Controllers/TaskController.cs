using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using ToDoListAPI.Models;
using Microsoft.AspNetCore.Authentication;

namespace ToDoListAPI.Controllers
{
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        //[AllowAnonymous]
        [HttpGet]
        public ActionResult GetListTask()
        {
            var ListTask = TodolistContext._context.Tasks.ToList();
            
            return Ok(ListTask);
        }


        [Produces("application/json")]
        [HttpPost]
        public ActionResult AddTask([FromBody] Models.Task task)
        {
            try
            {
                // Получаем пользователя из контекста базы данных по Id
                var user = TodolistContext._context.Users.Find(task.UserId);

                // Проверяем, что пользователь существует
                if (user == null)
                {
                    return BadRequest("User not found");
                }

                // Устанавливаем пользователя для задачи
                task.User = user;
                task.UserId = user.Id;
                task.Id = TodolistContext._context.Tasks.Max(x=>x.Id)+1;
                // Добавляем задачу в контекст базы данных
                TodolistContext._context.Tasks.Add(task);

                // Сохраняем изменения в базе данных
                TodolistContext._context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();
            }
        }



        [HttpGet("{taskId}")]
        public ActionResult GetTask(long taskId)
        {
            var existingTask = TodolistContext._context.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (existingTask == null)
            {
                return NotFound(); 
            }
            return Ok(existingTask);
        }


        [HttpPut("{taskId}")]
        public ActionResult UpdateTask(int taskId, Models.Task updatedTask)
        {
            var existingTask = TodolistContext._context.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (existingTask == null)
            {
                return NotFound(); 
            }

            existingTask.Timeframe = updatedTask.Timeframe;
            existingTask.Description = updatedTask.Description;
            existingTask.Done = updatedTask.Done;
            existingTask.Priority = updatedTask.Priority;
            existingTask.UserId = updatedTask.UserId;

            TodolistContext._context.SaveChanges();

            return Ok(existingTask);
        }


        [HttpDelete("{taskId}")]
        public ActionResult DeleteTask(int taskId)
        {
            try
            {
                var deleteTask = TodolistContext._context.Tasks.FirstOrDefault(x => x.Id == taskId);
                if (deleteTask == null)
                {
                    return NotFound();
                }
                TodolistContext._context.Tasks.Remove(deleteTask);
                TodolistContext._context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
