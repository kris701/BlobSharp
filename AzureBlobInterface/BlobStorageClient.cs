using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobInterface
{
	public class BlobStorageClient
	{
		public string ConnectionString { get; internal set; }

		public BlobStorageClient(string connectionString)
		{
			ConnectionString = connectionString;
		}

		public async Task<List<string>> ListFilesAsync(string directory)
		{
			var container = await GetCloudBlobContainer(directory, true);
			List<IListBlobItem> list = await ListBlobsAsync(container);
			List<string> outList = new List<string>();
			foreach (CloudBlockBlob fileitem in list)
				outList.Add(fileitem.Name);

			return outList;
		}

		public async Task<MemoryStream> DownloadAsStreamAsync(string directory, string fileName)
		{
			var container = await GetCloudBlobContainer(directory, true);
			var picBlob = container.GetBlockBlobReference(fileName);
			if (!(await picBlob.ExistsAsync()))
				throw new Exception($"File does not exist in the directory '{directory}'");
			MemoryStream memoryStream = new MemoryStream();
			await picBlob.DownloadToStreamAsync(memoryStream);
			return memoryStream;
		}

		public async Task<MemoryStream> DownloadAsStreamRangeAsync(string directory, string fileName, long from, long length)
		{
			var container = await GetCloudBlobContainer(directory, true);
			var picBlob = container.GetBlockBlobReference(fileName);
			if (!(await picBlob.ExistsAsync()))
				throw new Exception($"File does not exist in the directory '{directory}'");

			var totalLength = await GetFileLengthAsync(directory, fileName);
			if (from + length > totalLength)
				length = totalLength - from;
			if (length == 0)
				length = 1;

			MemoryStream memoryStream = new MemoryStream();
			await picBlob.DownloadRangeToStreamAsync(memoryStream, from, length);
			return memoryStream;
		}

		public async Task<long> GetFileLengthAsync(string directory, string fileName)
		{
			var container = await GetCloudBlobContainer(directory, true);
			var picBlob = container.GetBlockBlobReference(fileName);
			if (!(await picBlob.ExistsAsync()))
				throw new Exception($"File does not exist in the directory '{directory}'");

			await picBlob.FetchAttributesAsync();
			return picBlob.Properties.Length;
		}

		public async Task UploadFileAsync(string directory, string fileName, MemoryStream stream)
		{
			var container = await GetCloudBlobContainer(directory, true);
			var picBlob = container.GetBlockBlobReference(fileName);
			await picBlob.UploadFromStreamAsync(stream);
		}

		public async Task DeleteFileAsync(string directory, string fileName)
		{
			var container = await GetCloudBlobContainer(directory, true);
			var picBlob = container.GetBlockBlobReference(fileName);
			await picBlob.DeleteIfExistsAsync();
		}

		public async Task DeleteDirectoryAsync(string directory)
		{
			var container = await GetCloudBlobContainer(directory, false);
			await container.DeleteIfExistsAsync();
		}

		private static async Task<List<IListBlobItem>> ListBlobsAsync(CloudBlobContainer container)
		{
			BlobContinuationToken continuationToken = null;
			List<IListBlobItem> results = new List<IListBlobItem>();
			do
			{
				bool useFlatBlobListing = true;
				BlobListingDetails blobListingDetails = BlobListingDetails.None;
				int maxBlobsPerRequest = 500;
				var response = await container.ListBlobsSegmentedAsync("", useFlatBlobListing, blobListingDetails, maxBlobsPerRequest, continuationToken, null, null);
				continuationToken = response.ContinuationToken;
				results.AddRange(response.Results);
			}
			while (continuationToken != null);
			return results;
		}

		private async Task<CloudBlobContainer> GetCloudBlobContainer(string directory, bool createIfNone = false)
		{
			if (CloudStorageAccount.TryParse(ConnectionString, out CloudStorageAccount storageAccount))
			{
				CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

				var cloudBlobContainer = blobClient.GetContainerReference(directory);
				if (!await cloudBlobContainer.ExistsAsync() && createIfNone)
					await cloudBlobContainer.CreateIfNotExistsAsync();
				return cloudBlobContainer;
			}
			throw new Exception("Connection string not valid!");
		}
	}
}
