using System.Text.Json;

namespace webapp_5b;

public class UserService : IUserService
{

  private readonly HttpClient client;
  private readonly JsonSerializerOptions option;

  public UserService(HttpClient httpClient)
  {
    client = httpClient;
    option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
  }
  public async Task<List<User>> Get()
  {
    var response = await client.GetAsync("user");
    var content = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
      throw new ApplicationException(content);
    }

    if (string.IsNullOrEmpty(content))
    {
      return new List<User>();
    }

    return JsonSerializer.Deserialize<List<User>>(content, option) ?? new List<User>();
  }


  public async Task<User?> GetById(string userId)
  {
    var response = await client.GetAsync($"user/{userId}");
    var content = await response.Content.ReadAsStringAsync();
    if (!response.IsSuccessStatusCode)
    {
      throw new ApplicationException(content);
    }
    return JsonSerializer.Deserialize<User>(content, option);
  }
}

public interface IUserService
{
  Task<List<User>> Get();
  Task<User?> GetById(string userId);
}