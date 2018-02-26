using Cake.Core.Diagnostics;
using Minio;

namespace Cake.Minio
{
    public class FoundBucket : IBucket
    {
        private readonly ICakeLog logger;
        private readonly IBucketOperations minio;
        private readonly string bucketName;

        public FoundBucket(ICakeLog logger, IBucketOperations minio, string bucketName)
        {
            this.logger = logger;
            this.minio = minio;
            this.bucketName = bucketName;
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
    }
}