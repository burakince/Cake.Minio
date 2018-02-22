using Cake.Core.Tooling;

namespace Cake.Minio
{
    public class MinioSettings : ToolSettings
    {
        public string Endpoint { get; set; }

        public string AccessKey { get; set; } = "";

        public string SecretKey { get; set; } = "";

        public string Region { get; set; } = "";

        public bool SSL { get; set; } = false;
    }
}
