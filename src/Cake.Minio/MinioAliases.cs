using System;
using Cake.Core;
using Cake.Core.Annotations;

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

            var minio = ClientBuilder.Client(settings).Build();
            new BucketOperator(context.Log, minio)
                .ListBuckets();
        }

        [CakeMethodAlias]
        public static void MinioMakeBucket(this ICakeContext context, MinioSettings settings, MinioBucketSettings bucketSettings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var minio = ClientBuilder.Client(settings).Build();
            new BucketOperator(context.Log, minio, bucketSettings)
                .CheckBucket()
                .MakeBucket();
        }

        [CakeMethodAlias]
        public static void MinioRemoveBucket(this ICakeContext context, MinioSettings settings, MinioBucketSettings bucketSettings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var minio = ClientBuilder.Client(settings).Build();
            new BucketOperator(context.Log, minio, bucketSettings)
                .CheckBucket()
                .RemoveBucket();
        }

        [CakeMethodAlias]
        public static void MinioListObjects(this ICakeContext context, MinioSettings settings, MinioBucketSettings bucketSettings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var minio = ClientBuilder.Client(settings).Build();
            new BucketOperator(context.Log, minio, bucketSettings)
                .CheckBucket()
                .ListObjects();
        }

        [CakeMethodAlias]
        public static void MinioListIncompleteUploads(this ICakeContext context, MinioSettings settings, MinioBucketSettings bucketSettings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var minio = ClientBuilder.Client(settings).Build();
            new BucketOperator(context.Log, minio, bucketSettings)
                .CheckBucket()
                .ListIncompleteUploads();
        }
    }
}
