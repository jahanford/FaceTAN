using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.IO;

namespace FaceTAN.Core.Data
{

    public interface IImageElement
    {
        string guid { get; }
        string name { get; }
        string url { get; }
    }

    public class ImageElement : IImageElement
    {
        

        public ImageElement(string name, string url)
        {
            this.guid = Guid.NewGuid().ToString();
            this.name = name;
            this.url = url;
        }

        public string guid { get; set; }

        public string name { get; set; }

        public string url { get; set; }
    }

    public class DataSet
    {
        public DataSet(string bucketName, string accessKey, string secretKey, int maxTargets)
        {
            BucketName = bucketName;
            Credentials = new BasicAWSCredentials(accessKey, secretKey);
            Client = new AmazonS3Client(Credentials, Amazon.RegionEndpoint.USEast1);
            TargetImages = new Dictionary<string, Image>();
            SourceImages = new Dictionary<string, Image>();
            PopulateImages(maxTargets);

        }

        private string BucketName { get; }

        private AWSCredentials Credentials { get; }

        private AmazonS3Client Client { get; }

        public Dictionary<string, Image> TargetImages { get; }

        public Dictionary<string, Image> SourceImages { get; }

        private static Random Random = new Random();

        /*
         * Returns a list of all the images contained within the s3 bucket
         * */
        public void PopulateImages(int maxImages)
        {
            Console.WriteLine("Populating DataSet from S3 bucket...");
            List<string> keyList = GetKeyList();

            int count = 0;
            foreach(string key in keyList)
            {
                if (key.EndsWith("0001.jpg"))
                {
                    TargetImages.Add(key, GetImage(key));
                    Console.WriteLine("Image {0} retrieved.", key);

                    string sourceKey = key.Substring(0, key.Length - 8) + "0002.jpg";
                    if (keyList.Contains(sourceKey))
                    {
                        Console.WriteLine("Source image found: {0}", sourceKey);
                        SourceImages.Add(sourceKey, GetImage(sourceKey));
                    }

                    count += 1;
                    if (count >= maxImages)
                        break;
                }               
            }

            Console.WriteLine("DataSet population complete. {0} target images retrieved. {1} source images retrieved.", TargetImages.Count, SourceImages.Count);
        }

        public IImageElement[] GetImageArray()
        {

            List<string> keyList = GetKeyList();
            List<IImageElement> imageList = new List<IImageElement>();

            int titleIndex;
            foreach (var key in keyList)
            {
                titleIndex = 0;
                //Removing folder name for image title
                if (key.ToString().Split('/').Length > 1) titleIndex = 1;

                //Add to list
                imageList.Add(new ImageElement(key.ToString().Split('/')[titleIndex], "https://s3-ap-southeast-2.amazonaws.com/capstone-dataset/" + key.ToString().Replace(" ", "%20") ));
            }

            return imageList.ToArray();
        }

        /*
         * Returns an image with the specified key
         * */
        public Image GetImage(string key)
        {
            Image result;

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = key
            };

            using (GetObjectResponse response = Client.GetObject(request))
            using (Stream responseStream = response.ResponseStream)
            {
                result = Image.FromStream(responseStream);
            }

            return result;
        }

        public Stream GetImageStream(string key)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = key
            };

            GetObjectResponse response = Client.GetObject(request);
            return response.ResponseStream;
        }

        /*
         * Returns a complete list of all file keys contained in the amazon S3 bucket
         * */
        public List<string> GetKeyList(int maxKeys = 256)
        {
            List<string> keyList = new List<string>();

            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = BucketName,
                    MaxKeys = maxKeys
                };

                ListObjectsV2Response response;

                do
                {
                    response = Client.ListObjectsV2(request);

                    response.S3Objects.ForEach((img) =>
                    {
                        keyList.Add(img.Key);
                    });

                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated == true);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("An error occured while retrieving image key list: {0}", e);
            }

            return keyList;
        }
    }
}
