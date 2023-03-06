
namespace PullUsers
{

	public static class StaticData
	{
		public static int Value { get; set; } = 0;
		public static bool json { get; set; } = false;
		public static bool csv { get; set; } = false;
		public static string path { get; set; }

	}
	public class User
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int SourceId { get; set; }
		public override string ToString()
		{
			return $"{FirstName},{LastName},{Email},{SourceId}";
		}
	}
	public class Program
	{
		public static async Task Main()
		{
			try
			{
				await SetPathAndType();
				using FileHandler UsersFile = new();
				await UsersFile.CreateFile(StaticData.path);
				await UsersFile.InitFile();
				using Mutex mutex = new();

				var t1 = Task.Run(async () =>
				{
					try
					{
						var users = await Source1.GetUsers();
						await UsersFile.WriteOnFile(users, mutex);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"An error occurred while getting users from source 1: {ex.Message}");
					}
				});
				var t2 = Task.Run(async () =>
				{
					try
					{
						var users = await Source2.GetUsers();
						await UsersFile.WriteOnFile(users, mutex);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"An error occurred while getting users from source 2: {ex.Message}");
					}
				});
				var t3 = Task.Run(async () =>
				{
					try
					{
						var users = await Source3.GetUsers();
						await UsersFile.WriteOnFile(users, mutex);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"An error occurred while getting users from source 3: {ex.Message}");
					}
				});
				var t4 = Task.Run(async () =>
				{
					try
					{
						var users = await Source4.GetUsers();
						await UsersFile.WriteOnFile(users, mutex);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"An error occurred while getting users from source 4: {ex.Message}");
					}
				});

				var tasks = new List<Task> { t1, t2, t3, t4 };

				while (tasks.Count > 0)
				{
					Task finishedTask = await Task.WhenAny(tasks);
					await finishedTask;
					tasks.Remove(finishedTask);
				}

				await UsersFile.CloseFile();
				Console.WriteLine($"Total number of users written to file: {StaticData.Value}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
		private static async Task SetPathAndType()
		{
			try
			{
				Console.WriteLine("Insert a path:");
				var inputPath = Console.ReadLine();

				if (!string.IsNullOrEmpty(inputPath))
				{
					StaticData.path = inputPath.Trim();
					if (!Directory.Exists(Path.GetDirectoryName(StaticData.path)))
					{
						throw new ArgumentException("The specified directory does not exist.");
					}
					StaticData.path = StaticData.path.Trim('"');
					StaticData.path = StaticData.path.Replace("\\", "\\\\");
					StaticData.path = StaticData.path.Replace("/", "\\\\");
					StaticData.path = StaticData.path.ToLower();
					if (StaticData.path[StaticData.path.Length - 1] != '\\')
						StaticData.path = StaticData.path + "\\\\";
				}
				else
				{
					throw new ArgumentNullException("The path cannot be null or empty.");
				}

				Console.WriteLine("Insert a file type (json or csv):");
				var inputFileType = Console.ReadLine();

				if (!string.IsNullOrEmpty(inputFileType))
				{
					if (inputFileType.Trim().ToLower() == "json")
					{
						StaticData.json = true;
					}
					else if (inputFileType.Trim().ToLower() == "csv")
					{
						StaticData.csv = true;
					}
					else
					{
						throw new ArgumentException("The specified file type is not supported.");
					}
				}
				else
				{
					throw new ArgumentNullException("The file type cannot be null or empty.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while setting path and file type: {ex.Message}");
				//throw;
				await SetPathAndType();
			}
		}
	}
}
