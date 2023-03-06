//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Json;
//using System.Text;
//using System.Threading.Tasks;

//namespace PullUsers
//{
//	public  abstract class BaseSources
//	{
//		protected static async Task<List<User>> GetUsers<T1, T2>(string baseAddress, string apiPath) where T : class
//		{
//			using HttpClient client = new()
//			{
//				BaseAddress = new Uri(baseAddress)
//			};
//			List<User> users = new List<User>();
//			T1? userList = await client.GetFromJsonAsync<T1>(apiPath);
//			foreach (var item in userList.T2)
//			{
//				User user = new User
//				{
//					FirstName = GetFirstName(item),
//					LastName = GetLastName(item),
//					Email = GetEmail(item),
//					SourceId = GetSourceId()
//				};
//				users.Add(user);
//			}
//			return users;
//		}

//		protected abstract string GetFirstName<T>(T item);
//		protected abstract string GetLastName<T>(T item);
//		protected abstract string GetEmail<T>(T item);
//		protected abstract int GetSourceId();
//	}
//}
