using System;
using Minio;
using Cake.Core.Diagnostics;
using Minio.DataModel;

namespace Cake.Minio
{
    public class BucketOperator
    {
        private readonly ICakeLog logger;
        private readonly MinioClient minio;
        private readonly string bucketName;
        private readonly string region;
        private readonly string prefix;
        private readonly bool recursive;

        public BucketOperator(
            ICakeLog logger,
            MinioClient minio)
            : this(logger, minio, new MinioBucketSettings())
        {
        }

        public BucketOperator(
            ICakeLog logger,
            MinioClient minio,
            MinioBucketSettings settings)
            : this(logger, minio, settings.BucketName, settings.Region, settings.Prefix, settings.Recursive)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
        }

        public BucketOperator(ICakeLog logger, MinioClient minio, string bucketName, string region, string prefix, bool recursive)
        {
            this.logger = logger;
            this.minio = minio;
            this.bucketName = bucketName;
            this.region = region;
            this.prefix = prefix;
            this.recursive = recursive;
        }

        public void ListBuckets()
        {
            var task = minio.ListBucketsAsync();
            task.Wait();

            if (!string.IsNullOrEmpty(task.Result.Owner))
            {
                logger.Information(string.Format("Bucket Owner: {0}", task.Result.Owner));
            }
            if (task.Result.Buckets.Count == 0)
            {
                logger.Warning("No bucket found!");
            }
            foreach (Bucket bucket in task.Result.Buckets)
            {
                logger.Information(string.Format("Bucket Name: {0}, Creation Date: {1}", bucket.Name, bucket.CreationDate));
            }
        }

        public HasBucketOperations CheckBucket()
        {
            if (Found())
                return new FoundBucket(logger, minio, bucketName, region, prefix, recursive);
            return new NotFoundBucket(logger, minio, bucketName, region);
        }

        public bool Found()
        {
            var checkTask = minio.BucketExistsAsync(bucketName);
            checkTask.Wait();
            return checkTask.Result;
        }
    }
}
