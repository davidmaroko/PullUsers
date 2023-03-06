
using System.Text.Json;

namespace PullUsers
{
	public class FileHandler : IDisposable
	{
		private FileStream _stream;
		private StreamWriter _writer;
		private bool _disposed = false;

		public async Task InitFile()
		{
			try
			{
				_writer = new StreamWriter(_stream);
				if (StaticData.json)
				{
					await _writer.WriteAsync("[");
				}
				else if (StaticData.csv)
				{
					await _writer.WriteAsync("FirstName,LastName,Email,SourceId");
					await _writer.WriteAsync(Environment.NewLine);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while initializing the file: {ex.Message}");
			}
		}

		public async Task CreateFile(string path)
		{
			try
			{
				if (StaticData.json)
					path = path + "pull_users.json";
				else if (StaticData.csv)
					path = path + "pull_users.csv";

				_stream = File.Create(path);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while creating the file: {ex.Message}");
			}
		}

		public async Task WriteOnFile<T>(List<T> data, Mutex mutex)
		{
			try
			{
				if (_stream == null)
				{
					throw new InvalidOperationException("File stream is not created.");
				}

				var t = Task.Run(async () =>
				{
					foreach (T item in data)
					{
						mutex.WaitOne();
						if (StaticData.json)
						{
							_writer.Write(JsonSerializer.Serialize(item));
							_writer.Write(",");
						}
						else if (StaticData.csv)
						{
							_writer.WriteLine(item.ToString());
						}

						StaticData.Value++;
						mutex.ReleaseMutex();
					}
				});

				await t;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
			}
		}

		public async Task CloseFile()
		{
			try
			{
				if (_stream == null)
				{
					throw new InvalidOperationException("File stream is null, nothing to dispose");
				}

				if (StaticData.json)
				{
					_writer.Write("]");
				}

				_writer.Close();
				await _stream.DisposeAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while closing the file: {ex.Message}");
			}
		}
		public void Dispose()
		{
			_writer.Dispose();
			_stream.Dispose();
		}
	}
}




