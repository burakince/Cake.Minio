using System;
using Minio;
using Cake.Core.Diagnostics;
using Minio.DataModel;

namespace Cake.Minio
{
    public class BucketOperator
    {
        private readonly ICakeLog logger;
        private readonly IBucketOperations minio;
        private readonly string bucketName;
        private readonly string region;

        public BucketOperator(
            ICakeLog logger,
            IBucketOperations minio)
            : this(logger, minio, new MinioBucketSettings())
        {
        }

        public BucketOperator(
            ICakeLog logger,
            IBucketOperations minio,
            MinioBucketSettings settings)
            : this(logger, minio, settings.BucketName, settings.Region)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
        }

        public BucketOperator(ICakeLog logger, IBucketOperations minio, string bucketName, string region)
        {
            this.logger = logger;
            this.minio = minio;
            this.bucketName = bucketName;
            this.region = region;
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

        public IBucket CheckBucket()
        {
            if (Found())
                return new FoundBucket(logger, minio, bucketName);
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
