using PullUsers;
using System.Net.Http.Json;
using System.Text.Json;

public class Source1
{
	public static async Task<List<User>> GetUsers()
	{
		List<User> users = new List<User>();

		try
		{
			var client = new HttpClient();
			client.BaseAddress = new Uri("https://randomuser.me/api");

			var user1list = await client.GetFromJsonAsync<Users1>("");

			foreach (var item in user1list.results)
			{
				User user = new User
				{
					FirstName = item.name.first,
					LastName = item.name.last,
					Email = item.email,
					SourceId = 1
				};
				users.Add(user);
			}
		}
		catch (HttpRequestException e)
		{
			Console.WriteLine($"An HTTP request exception occurred in source1: {e.Message}");
		}
		catch (JsonException e)
		{
			Console.WriteLine($"A JSON exception occurred in source1: {e.Message}");
		}
		catch (Exception e)
		{
			Console.WriteLine($"An unexpected exception occurred in source1: {e.Message}");
		}

		return users;
	}
	//public static async Task<List<User>> GetUsers()
	//{
	//    List<User> users = new List<User>();

	//    var client = new HttpClient();
	//    client.BaseAddress = new Uri("https://randomuser.me/api");

	//    var user1list = await client.GetFromJsonAsync<Users1>("");


	//    foreach (var item in user1list.results)
	//    {
	//        User user = new User
	//        {
	//            FirstName = item.name.first,
	//            LastName = item.name.last,
	//            Email = item.email,
	//            SourceId = 1
	//        };
	//        users.Add(user);
	//    }
	//    return users;
	//}
}

public class Users1
{
    public User1[] results { get; set; }
}



public class User1
{
    public Name name { get; set; }
    public string email { get; set; }
}

public class Name
{
    public string first { get; set; }
    public string last { get; set; }
}


