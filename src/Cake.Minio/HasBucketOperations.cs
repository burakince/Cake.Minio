namespace Cake.Minio
{
    public interface HasBucketOperations
    {
        void MakeBucket();
        void RemoveBucket();
        void ListObjects();
        void ListIncompleteUploads();
    }
}
