using Amazon.Runtime;
using Amazon.SQS;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.AWS
{
    public class AwsService : IAwsService
    {
        private readonly IConfiguration _configuration;

        public AwsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IAmazonSQS CreateSqsClient()
        {
            var awsOptions = _configuration.GetSection("AWS").Get<AwsOptions>()
        ?? throw new ArgumentException("AWS configuration is missing or incomplete.");

            var awsCredentials = new BasicAWSCredentials(
                awsOptions.AccessKey ?? throw new ArgumentException("AWS Access Key is missing or incomplete."),
                awsOptions.SecretKey ?? throw new ArgumentException("AWS Secret Key is missing or incomplete.")
            );

            var sqsConfig = new AmazonSQSConfig { RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsOptions.Region) };

            return new AmazonSQSClient(awsCredentials, sqsConfig);
        }
    }
}