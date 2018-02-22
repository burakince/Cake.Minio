using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Minio;
using Minio.DataModel;

namespace Cake.Minio
{
    [CakeAliasCategory("Minio")]
    public static class MinioAliases
    {
        [CakeMethodAlias]
        public static void MinioListBuckets(this ICakeContext context, MinioSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (string.IsNullOrEmpty(settings.Endpoint))
            {
                throw new ArgumentNullException(nameof(settings.Endpoint));
            }

            var minioClient = new MinioClient(settings.Endpoint, settings.AccessKey, settings.SecretKey, settings.Region);
            var minio = settings.SSL ? minioClient.WithSSL() : minioClient;
            var task = minio.ListBucketsAsync();
            task.Wait();

            if (!string.IsNullOrEmpty(task.Result.Owner))
            {
                context.Log.Information(string.Format("Bucket Owner: {0}", task.Result.Owner));
            }
            if (task.Result.Buckets.Count == 0)
            {
                context.Log.Warning("No bucket found!");
            }
            foreach (Bucket bucket in task.Result.Buckets)
            {
                context.Log.Information(string.Format("Bucket Name: {0}, Creation Date: {1}", bucket.Name, bucket.CreationDate));
            }
        }

        [CakeMethodAlias]
        public static void MinioMakeBucket(this ICakeContext context, MinioSettings settings, MinioBucketSettings bucketSettings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (string.IsNullOrEmpty(settings.Endpoint))
            {
                throw new ArgumentNullException(nameof(settings.Endpoint));
            }
            if (bucketSettings == null)
            {
                throw new ArgumentNullException(nameof(bucketSettings));
            }

            var minioClient = new MinioClient(settings.Endpoint, settings.AccessKey, settings.SecretKey, settings.Region);
            var minio = settings.SSL ? minioClient.WithSSL() : minioClient;

            var checkTask = minioClient.BucketExistsAsync(bucketSettings.BucketName);
            checkTask.Wait();
            bool found = checkTask.Result;

            if (found)
            {
                context.Log.Warning(string.Format("Bucket {0} already exists. Create operation cancelled!", bucketSettings.BucketName));
                return;
            }

            var task = minio.MakeBucketAsync(bucketSettings.BucketName, bucketSettings.Region);
            task.Wait();

            context.Log.Information(string.Format("Bucket {0} successfully created", bucketSettings.BucketName));
        }

        [CakeMethodAlias]
        public static void MinioRemoveBucket(this ICakeContext context, MinioSettings settings, MinioBucketSettings bucketSettings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (string.IsNullOrEmpty(settings.Endpoint))
            {
                throw new ArgumentNullException(nameof(settings.Endpoint));
            }
            if (bucketSettings == null)
            {
                throw new ArgumentNullException(nameof(bucketSettings));
            }

            var minioClient = new MinioClient(settings.Endpoint, settings.AccessKey, settings.SecretKey, settings.Region);
            var minio = settings.SSL ? minioClient.WithSSL() : minioClient;

            var checkTask = minioClient.BucketExistsAsync(bucketSettings.BucketName);
            checkTask.Wait();
            bool found = checkTask.Result;

            if (!found)
            {
                context.Log.Warning(string.Format("Bucket {0} does not exist. Remove operation cancelled!", bucketSettings.BucketName));
                return;
            }

            var task = minio.RemoveBucketAsync(bucketSettings.BucketName);
            task.Wait();

            context.Log.Information(string.Format("Bucket {0} is removed successfully", bucketSettings.BucketName));
        }
    }
}
