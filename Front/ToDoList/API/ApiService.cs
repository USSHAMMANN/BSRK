using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using ToDoList.Models;

namespace ToDoList.API
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        public static string? _token;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ApiService(IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5000/api/");
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }


        public async Task RegisterUserAsync(User user)
        {
            try
            {
                var model = new User { Id = 0, UserName = user.UserName, PasswordHash = user.PasswordHash };
                var json = JsonConvert.SerializeObject(model);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("registr", content);

            }
            catch { }
        }

        public async Task<string> AuthorizationUserApi(User user)
        {
            try
            {
                var model = new User { Id = 0, UserName = user.UserName, PasswordHash = user.PasswordHash };
                var json = JsonConvert.SerializeObject(model);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("login", content);

                if (response.IsSuccessStatusCode) 
                {
                    var tokenResponse = await response.Content.ReadAsStringAsync();
                    _token = tokenResponse;
                    return tokenResponse;
                    
                }
                return null;
            }
            catch 
            {
                return null;
            }
        }

        private async Task AddAuthorizationHeader()
        {
            var token = await GetTokenFromCookie();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
            }
            
        }

        private async Task<string> GetTokenFromCookie()
        {
            var tokenCookie = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            return tokenCookie;
        }

        public async Task<List<TaskModel>> GetTaskListAsync()
        {
            try
            {
                await AddAuthorizationHeader(); // Добавляем токен к запросу

                var response = await _httpClient.GetAsync("tasks", HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    

                    return JsonConvert.DeserializeObject<List<TaskModel>>(jsonString);
                }

                return new List<TaskModel>();
            }
            catch
            {
                return new List<TaskModel>();
            }
        }

        public async Task<TaskModel> GetTaskApi(long id)
        {
            try
            {
                await AddAuthorizationHeader(); // Добавляем токен к запросу
                var response = await _httpClient.GetAsync($"tasks/{id}", HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TaskModel>(jsonString);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User> GetCurrentUser()
        {
            try
            {
                await AddAuthorizationHeader();

                var response = await _httpClient.GetAsync("user", HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(jsonString);
                    return JsonConvert.DeserializeObject<User>(jsonString); 

                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task AddTaskAsync(TaskModel taskModel)
        {
            try
            {

                await AddAuthorizationHeader(); // Добавляем токен к запросу

                var task_user = await GetCurrentUser();
                task_user.PasswordHash = "";
                              
                var model = new TaskModel
                {
                    Id = 0,
                    Description = taskModel.Description,
                    Timeframe = taskModel.Timeframe,
                    UserId = task_user.Id,
                    Priority = taskModel.Priority,
                    Done = false,
                    User = task_user,
                };

                var json = JsonConvert.SerializeObject(model);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                               
                var response = await _httpClient.PostAsync("tasks", content);
                Console.Write(response.IsSuccessStatusCode);
            }
            catch { }
        }

        public async Task<bool> EditTaskApi(TaskModel taskModel)
        {
            try
            {                
                await AddAuthorizationHeader(); // Добавляем токен к запросу
                                
                var jsonContent = JsonConvert.SerializeObject(taskModel);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"tasks/{taskModel.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTaskApi(long id)
        {
            try
            {
                await AddAuthorizationHeader(); // Добавляем токен к запросу

                var response = await _httpClient.DeleteAsync($"tasks/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
