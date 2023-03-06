

using PullUsers;
using System.Net.Http.Json;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;

public class Source2 
{
    public static async Task<List<User>> GetUsers()
	{
		List<User> users = new List<User>();
		try
		{
			using HttpClient client = new()
			{
				BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
			};
			List<User2>? user2list = await client.GetFromJsonAsync<List<User2>>("users");
			foreach (var item in user2list)
			{
				User user = new User
				{
					FirstName = item.name.Split(' ')[0],
					LastName = item.name.Split(' ')[1],
					Email = item.email,
					SourceId = 2
				};
				users.Add(user);
			}
		}
		catch (HttpRequestException e)
		{
			Console.WriteLine($"An HTTP request exception occurred: {e.Message}");
		}
		catch (JsonException e)
		{
			Console.WriteLine($"A JSON exception occurred: {e.Message}");
		}
		catch (Exception e)
		{
			Console.WriteLine($"An unexpected exception occurred: {e.Message}");
		}

		return users;
	}
}
 
public class User2
{
    public string name { get; set; }
    public string email { get; set; }
}