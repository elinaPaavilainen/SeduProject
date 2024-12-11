using System.Text;
using Newtonsoft.Json;
using SharedModels;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient(new HttpClientHandler 
        {
            MaxConnectionsPerServer = 1, AllowAutoRedirect = true 
        });

        //_httpClient.BaseAddress = new Uri("http://192.168.68.51:5195/");
        //_httpClient.BaseAddress = new Uri("://localhost:5195/)";
        _httpClient.BaseAddress = new Uri("http://10.0.2.2:5195/");
    }

    // API requests for BooksController
    // GET request
    public async Task<IEnumerable<Books>> GetBooksControllerDataAsync()
    {
        var response = await _httpClient.GetAsync("api/Books");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<Books>>(content);
    }

    // GET by ID
    public async Task<string> GetBooksControllerDataByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/Books/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    //GET, search
    public async Task<IEnumerable<Books>> SearchBooksAsync(string query)
    {
        var response = await _httpClient.GetAsync($"api/Books/search/{query}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<Books>>(content);
    }

    // POST request
    public async Task<string> PostBooksControllerDataAsync(object data)
    {
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Books", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    // PUT request
    public async Task<string> PutBooksControllerDataAsync(int id, object data)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Books/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return null;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    // DELETE request
    public async Task<string> DeleteBooksControllerDataAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Books/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    // API requests for UsersController
    // GET request
    public async Task<IEnumerable<Users>> GetUsersControllerDataAsync()
    {
        var response = await _httpClient.GetAsync("api/Users");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<Users>>(json); 
    }

    // GET by username
    public async Task<string> LoginGetUsersControllerDataByUsernameAsync(string username)
    {
        var response = await _httpClient.GetAsync($"api/Users/{username}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

   // GET list by username
    public async Task<IEnumerable<Users>> DeleteGetUsersControllerDataByUsernameAsync(string username)
    {
        var response = await _httpClient.GetAsync($"api/Users/{username}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<Users>>(json);
    }

    //GET, search
    public async Task<IEnumerable<Users>> SearchUsersAsync(string query)
    {
        var response = await _httpClient.GetAsync($"api/Users/search/{query}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<Users>>(content);
    }

    // POST request
    public async Task<string> PostUsersControllerDataAsync(object data)
    {
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Users", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    // PUT request
    public async Task<string> PutUsersControllerDataAsync(int id, object data)
    {
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"api/Users/{id}", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    // DELETE request
    public async Task<string> DeleteUsersControllerDataAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Users/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    //API REQUEST FOR DeletedInformation
    //POST LOG DELETED
    public async Task LogDeletionAsync(string deletedBy, string authorAndTitle, string lossOrSold, string notes)
    {
        var deletionRecord = new
        {
            AuthorAndTitle = authorAndTitle,
            User = deletedBy,
            LossOrSold = lossOrSold,
            Notes = notes
        };

        var content = new StringContent(JsonConvert.SerializeObject(deletionRecord), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/DeleteLog", content);
        response.EnsureSuccessStatusCode(); // Ensure the request was successful
    }
    //GET LOG DELETED
    public async Task<IEnumerable<Deleted_books>> GetDeletedBooksDataAsync()
    {
        var response = await _httpClient.GetAsync("api/DeleteLog");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<Deleted_books>>(content);
    }
    //GET, SEARCH LOG DELETED
    public async Task<IEnumerable<Deleted_books>> SearchDeletedBooksAsync(string query)
    {
        var response = await _httpClient.GetAsync($"api/DeleteLog/search/{query}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<Deleted_books>>(content);
    }

}

