
using PullUsers;
using System.Net.Http.Json;
using System.Text.Json;

public class Source4
{

    public static async Task<List<User>> GetUsers()
    {
		List<User> users = new List<User>();
        try
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://reqres.in/api/users")
            };

            Users4? user4list = await client.GetFromJsonAsync<Users4>("users");
            foreach (var item in user4list.data)
            {
                User user = new User
                {
                    FirstName = item.first_name,
                    LastName = item.last_name,
                    Email = item.email,
                    SourceId = 4
                };
                users.Add(user);
            }
        }
		catch (HttpRequestException e)
		{
			Console.WriteLine($"An HTTP request exception occurred in source4: {e.Message}");
		}
		catch (JsonException e)
		{
			Console.WriteLine($"A JSON exception occurred in source4: {e.Message}");
		}
		catch (Exception e)
		{
			Console.WriteLine($"An unexpected exception occurred in source4: {e.Message}");
		}

		return users;
	}
}



public class Users4
{
  
    public User4[] data { get; set; }
}

public class User4
{
    public string email { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
}
