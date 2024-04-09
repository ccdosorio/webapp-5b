using System.Net.Http.Json;
using System.Text.Json;

namespace webapp_5b;

public class AssignmentService : IAssignmentService
{

  private readonly HttpClient client;
  private readonly JsonSerializerOptions option;

  public AssignmentService(HttpClient httpClient)
  {
    client = httpClient;
    option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
  }
  public async Task<List<Assignment>?> Get()
  {
    var response = await client.GetAsync("assignment");
    var content = await response.Content.ReadAsStringAsync();
    if (!response.IsSuccessStatusCode)
    {
      throw new ApplicationException(content);
    }
    return JsonSerializer.Deserialize<List<Assignment>>(content, option);

  }

  public async Task<Assignment?> GetById(string assignmentId)
  {
    var response = await client.GetAsync($"assignment/{assignmentId}");
    var content = await response.Content.ReadAsStringAsync();
    if (!response.IsSuccessStatusCode)
    {
      throw new ApplicationException(content);
    }
    return JsonSerializer.Deserialize<Assignment>(content, option);
  }

  public async Task Add(Assignment assignment)
  {
    // Crear un nuevo objeto Assignment con solo las propiedades deseadas
    var assignmentToSend = new AssignmentDto
    {
      categoryId = assignment.categoryId,
      userId = assignment.userId,
      title = assignment.title,
      description = assignment.description,
      assignmentPriority = assignment.assignmentPriority
    };

    var response = await client.PostAsync("assignment", JsonContent.Create(assignmentToSend));
    var content = await response.Content.ReadAsStringAsync();
    if (!response.IsSuccessStatusCode)
    {
      throw new ApplicationException(content);
    }
  }


  public async Task Update(Assignment assignment)
  {
    var response = await client.PutAsync($"assignment/{assignment.assignmentId}", JsonContent.Create(assignment));
    var content = await response.Content.ReadAsStringAsync();
    if (!response.IsSuccessStatusCode)
    {
      throw new ApplicationException(content);
    }
  }

  public async Task Delete(string assignmentId)
  {
    var response = await client.DeleteAsync($"assignment/{assignmentId}");
    var content = await response.Content.ReadAsStringAsync();
    if (!response.IsSuccessStatusCode)
    {
      throw new ApplicationException(content);
    }
  }

}

public interface IAssignmentService
{
  Task<List<Assignment>?> Get();
  Task<Assignment?> GetById(string assignmentId);
  Task Add(Assignment assignment);
  Task Update(Assignment assignment);
  Task Delete(string assignmentId);
}