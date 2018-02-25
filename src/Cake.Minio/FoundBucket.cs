using System;
using Cake.Core.Diagnostics;
using Minio;
using Minio.DataModel;

namespace Cake.Minio
{
    public class FoundBucket : HasBucketOperations
    {
        private readonly ICakeLog logger;
        private readonly MinioClient minio;
        private readonly string bucketName;
        private readonly string region;
        private readonly string prefix;
        private readonly bool recursive;

        public FoundBucket(ICakeLog logger, MinioClient minio, string bucketName, string region, string prefix, bool recursive)
        {
            this.logger = logger;
            this.minio = minio;
            this.bucketName = bucketName;
            this.region = region;
            this.prefix = prefix;
            this.recursive = recursive;
        }

        public void MakeBucket()
        {
            logger.Warning(string.Format("Bucket {0} already exists. Create operation cancelled!", bucketName));
        }

        public void RemoveBucket()
        {
            var task = minio.RemoveBucketAsync(bucketName);
            task.Wait();
            logger.Information(string.Format("Bucket {0} is removed successfully", bucketName));
        }

        public void ListObjects()
        {
            IObservable<Item> observable = minio.ListObjectsAsync(bucketName, prefix, recursive);
            observable.Subscribe(
                item => logger.Information(string.Format("Object: {0}", item.Key)),
                ex => logger.Information(string.Format("OnError: {0}", ex.Message)),
                () => logger.Information(string.Format("Listed all objects in bucket {0}", bucketName))
            );
        }

        public void ListIncompleteUploads()
        {
            IObservable<Upload> observable = minio.ListIncompleteUploads(bucketName, prefix, recursive);
            observable.Subscribe(
                item => logger.Information(string.Format("Upload: {0}", item.Key)),
                ex => logger.Information(string.Format("OnError: {0}", ex.Message)),
                () => logger.Information(string.Format("Listed the pending uploads to bucket {0}", bucketName))
            );
        }
    }
}