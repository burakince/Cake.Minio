using Cake.Core.Tooling;

namespace Cake.Minio
{
    public class MinioBucketSettings : ToolSettings
    {
        public string BucketName { get; set; }

        public string Region { get; set; } = "us-east-1";
    }
}
