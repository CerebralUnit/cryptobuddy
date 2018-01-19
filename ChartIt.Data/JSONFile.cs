using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ChartIt.Data
{
    public class JSONFile<T>
    {
        private const string BLOB_DATA_ROOT = "https://coinbuddy.blob.core.windows.net/data/";

        private string _json;
  
        public T Object { get; private set; }

        public string JSON
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_json) && Object != null)
                    return JsonConvert.SerializeObject(Object);
                else
                    return _json; 
            } 
        }

        public JSONFile()
        {

        }

        public JSONFile(string jsonString)
        {
            this._json = jsonString;
            this.Object = JsonConvert.DeserializeObject<T>(jsonString);
        }

        public JSONFile(string fileName, bool isBlob)
        {
            this.Object = GetFromBlob(fileName);  
        }
        public JSONFile(T obj, string fileName)
        { 
            this._json = JsonConvert.SerializeObject(obj);
            this.Object = obj;
        }
        public bool Save(string fileName)
        { 
            SaveJsonToBlob(fileName); 
            return true;
        }
        private bool SaveJsonToBlob(string fileName)
        {
            CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);
            CloudBlobClient Client = StorageAccount.CreateCloudBlobClient();

            var URI = new Uri(BLOB_DATA_ROOT + fileName);

            var blockBlob = Client.GetBlobReferenceFromServer(URI);
 
            blockBlob.Properties.ContentType = "application/json";
          
            using (var ms = new MemoryStream())
            {
               
                StreamWriter writer = new StreamWriter(ms);
                writer.Write(this.JSON);
                writer.Flush();
                ms.Position = 0; 
                blockBlob.UploadFromStream(ms);
            }

            return true;
        }
        private T GetFromBlob(string fileName)
        {
            CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);

            CloudBlobClient Client = StorageAccount.CreateCloudBlobClient();

            var URI = new Uri(BLOB_DATA_ROOT + fileName); 

            var Blob = Client.GetBlobReferenceFromServer(URI);

            T result = default(T);

            using (var stream = new MemoryStream())
            {
                Blob.DownloadToStream(stream);
                stream.Position = 0;
                var serializer = new JsonSerializer();

                using (var sr = new StreamReader(stream))
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    result = serializer.Deserialize<T>(jsonTextReader);
                }
            }

            return result;
        }
    }
}
