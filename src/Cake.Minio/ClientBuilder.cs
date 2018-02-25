using System;
using Minio;

namespace Cake.Minio
{
    public class ClientBuilder
    {
        private readonly string endpoint;
        private readonly string accessKey;
        private readonly string secretKey;
        private readonly string region;
        private readonly bool hasSSL;

        private ClientBuilder(string endpoint, string accessKey, string secretKey, string region, bool ssl)
        {
            this.endpoint = endpoint;
            this.accessKey = accessKey;
            this.secretKey = secretKey;
            this.region = region;
            this.hasSSL = ssl;
        }

        public static ClientBuilder Client(MinioSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return Client(settings.Endpoint, settings.AccessKey, settings.SecretKey, settings.Region, settings.SSL);
        }

        public static ClientBuilder Client(string endpoint, string accessKey, string secretKey, string region, bool ssl)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            return new ClientBuilder(endpoint, accessKey, secretKey, region, ssl);
        }

        public MinioClient Build()
        {
            var minioClient = new MinioClient(endpoint, accessKey, secretKey, region);
            return hasSSL ? minioClient.WithSSL() : minioClient;
        }
    }
}
