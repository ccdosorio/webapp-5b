using System.Text.Json;

namespace webapp_5b;

public class CategoryService : ICategoryService
{

  private readonly HttpClient client;
  private readonly JsonSerializerOptions option;

  public CategoryService(HttpClient httpClient)
  {
    client = httpClient;
    option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
  }
  public async Task<List<Category>> Get()
  {
    var response = await client.GetAsync("category");
    var content = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
      throw new ApplicationException(content);
    }

    if (string.IsNullOrEmpty(content))
    {
      return new List<Category>();
    }

    return JsonSerializer.Deserialize<List<Category>>(content, option) ?? new List<Category>();
  }

  public async Task<Category?> GetById(string categoryId)
  {
    var response = await client.GetAsync($"category/{categoryId}");
    var content = await response.Content.ReadAsStringAsync();
    if (!response.IsSuccessStatusCode)
    {
      throw new ApplicationException(content);
    }
    return JsonSerializer.Deserialize<Category>(content, option);
  }
}

public interface ICategoryService
{
  Task<List<Category>> Get();
  Task<Category?> GetById(string categoryId);
}