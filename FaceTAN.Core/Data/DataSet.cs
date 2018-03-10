using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.IO;

namespace FaceTAN.Core.Data
{
    public class DataSet
    {
        public DataSet(string bucketName, string accessKey, string secretKey, int maxItems)
        {
            BucketName = bucketName;
            Credentials = new BasicAWSCredentials(accessKey, secretKey);
            Client = new AmazonS3Client(Credentials);
            KeyList = GetKeyList(maxItems);
        }

        private string BucketName { get; }

        private AWSCredentials Credentials { get; }

        private AmazonS3Client Client { get; }

        private List<string> KeyList { get; }

        private static Random Random = new Random();

        /*
         * Returns a list of all the images contained within the s3 bucket
         * */
        public List<Image> GetAllImages()
        {
            List<Image> result = new List<Image>();

            KeyList.ForEach((key) =>
            {
                result.Add(GetImage(key));
            });

            return result;
        }

        /*
         * Returns an image with the specified key
         * */
        public Image GetImage(string key)
        {
            Image result;
            using (Client)
            {
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

            }
            return result;
        }


        /*
         * Returns a random image from the s3 bucket
         * */
        public Image GetRandomImage()
        {
            return GetImage(GetRandomKey());
        }

        /*
         * Returns a random key from the list of file keys
         * */
        private string GetRandomKey()
        {
            int idx = Random.Next(KeyList.Count);
            return KeyList[idx];
        }

        /*
         * Returns a complete list of all file keys contained in the amazon S3 bucket
         * */
        private List<string> GetKeyList(int maxKeys)
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
