using Cake.Core.Diagnostics;
using Minio;

namespace Cake.Minio
{
    public class NotFoundBucket : IBucket
    {
        private readonly ICakeLog logger;
        private readonly IBucketOperations minio;
        private readonly string bucketName;
        private readonly string region;

        public NotFoundBucket(ICakeLog logger, IBucketOperations minio, string bucketName, string region)
        {
            this.logger = logger;
            this.minio = minio;
            this.bucketName = bucketName;
            this.region = region;
        }

        public void MakeBucket()
        {
            var task = minio.MakeBucketAsync(bucketName, region);
            task.Wait();
            logger.Information(string.Format("Bucket {0} successfully created", bucketName));
        }

        public void RemoveBucket()
        {
            logger.Warning(string.Format("Bucket {0} does not exist. Remove operation cancelled!", bucketName));
        }
    }
}