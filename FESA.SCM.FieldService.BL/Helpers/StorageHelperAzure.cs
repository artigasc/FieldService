using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BL.Helpers {
	public class StorageHelperAzure {

		public static async Task<dynamic> UploadFileToAzure(dynamic file, string connectionStorage) {
			var blockBlob = await GetBlockBlob(file, connectionStorage);
			blockBlob.UploadFromByteArray(file.File, 0, file.File.Length);
			file.FileUrl = blockBlob.Uri.AbsoluteUri;
			return file;
		}

		private static async Task<CloudBlockBlob> GetBlockBlob(dynamic file, string connectionStorage) {
			var blobContainer = await GetBlobContainer(file.Container, connectionStorage);
			var permissions = new BlobContainerPermissions();
			permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
			await blobContainer.SetPermissionsAsync(permissions);
			var blob = blobContainer.GetBlockBlobReference(file.Name);
			return blob;
		}

		private static async Task<CloudBlobContainer> GetBlobContainer(string container, string connectionStorage) {
			var storageAccount = CloudStorageAccount.Parse(connectionStorage);
			var blobClient = storageAccount.CreateCloudBlobClient();
			var blobContainer = blobClient.GetContainerReference(container);
			await blobContainer.CreateIfNotExistsAsync();
			return blobContainer;
		}

		public static async Task GetBlobContainer2(string idContainer, string connectionStorage) {
			BlobContinuationToken blobContinuationToken = null;
			var blobContainer = await GetBlobContainer(idContainer, connectionStorage);
			do {
				var result = await blobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
				foreach (var item in result.Results) {
					var i = item.Uri;
				}
			} while (blobContinuationToken != null);
		}

	}
}
