
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
				using FileHandler UsersFile = new();
				using Mutex mutex = new();

				var CreateFile = Task.Run(async () => {
					await SetPathAndType();
					await UsersFile.CreateFile(StaticData.path);
					await UsersFile.InitFile();
				});

				var users1 =  Source1.GetUsers(); 
				var users2 =  Source2.GetUsers(); 
				var users3 =  Source3.GetUsers(); 
				var users4 =  Source4.GetUsers();

				await CreateFile;

				var write1 = Task.Run(async () =>
				{
					try
					{		
						await UsersFile.WriteOnFile(await users1, mutex);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"An error occurred while getting users from source 1: {ex.Message}");
					}
				});
				var write2 = Task.Run(async () =>
				{
					try
					{
						await UsersFile.WriteOnFile(await users2, mutex);

					}
					catch (Exception ex)
					{
						Console.WriteLine($"An error occurred while getting users from source 2: {ex.Message}");
					}
				});
				var write3 = Task.Run(async () =>
				{
					try
					{
						await UsersFile.WriteOnFile(await users3, mutex);

					}
					catch (Exception ex)
					{
						Console.WriteLine($"An error occurred while getting users from source 3: {ex.Message}");
					}
				});
				var write4 = Task.Run(async () =>
				{
					try
					{
						await UsersFile.WriteOnFile(await users4, mutex);

					}
					catch (Exception ex)
					{
						Console.WriteLine($"An error occurred while getting users from source 4: {ex.Message}");
					}
				});

				var WriteTasks = new List<Task> { write1, write2, write3, write4 };

				while (WriteTasks.Count > 0)
				{
					Task finishedTask = await Task.WhenAny(WriteTasks);
					await finishedTask;
					WriteTasks.Remove(finishedTask);
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
