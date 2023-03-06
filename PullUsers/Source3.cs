
using PullUsers;
using System.Net.Http.Json;
using System.Text.Json;

public class Source3
{
    public static async Task<List<User>> GetUsers()
    {
		List<User> users = new List<User>();
        try
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://dummyjson.com/users")
            };
            // Get the user information.
            Users3? user3list = await client.GetFromJsonAsync<Users3>("users");
            foreach (var item in user3list.users)
            {
                User user = new User
                {
                    FirstName = item.firstName,
                    LastName = item.lastName,
                    Email = item.email,
                    SourceId = 3
                };
                users.Add(user);
            }
        }
		catch (HttpRequestException e)
		{
			Console.WriteLine($"An HTTP request exception occurred in source3: {e.Message}");
		}
		catch (JsonException e)
		{
			Console.WriteLine($"A JSON exception occurred in source3: {e.Message}");
		}
		catch (Exception e)
		{
			Console.WriteLine($"An unexpected exception occurred in source3: {e.Message}");
		}

		return users;
	}
}


public class Users3
{
    public User3[] users { get; set; }
}

public class User3
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
   
}
