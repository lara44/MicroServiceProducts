
using Amazon.SQS;
using Amazon.SQS.Model;
using Application.Common.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Infrastructure.Configuration;

namespace Infrastructure.Messaging
{
    public class SqsEventPublisher : IEventPublisher
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly ILogger<SqsEventPublisher> _logger;

        public SqsEventPublisher(
            IAmazonSQS sqsCliente, 
            ILogger<SqsEventPublisher> logger,
            IConfiguration configuration
        )
        {
            var awsOptions = configuration.GetSection("AWS").Get<AwsOptions>()
                ?? throw new ArgumentException("AWS configuration is missing or incomplete.");

            var awsCredentials = new BasicAWSCredentials(
                awsOptions.AccessKey ?? throw new ArgumentException("AWS Access Key is missing or incomplete."),
                awsOptions.SecretKey ?? throw new ArgumentException("AWS Secret Key is missing or incomplete.")
            );

            var sqsConfig = new AmazonSQSConfig { RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsOptions.Region) };

            _sqsClient = new AmazonSQSClient(awsCredentials, sqsConfig);
            _logger = logger;
        }
        public async Task PublishAsync<T>(T @event, string queueUrl, string eventType, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(queueUrl))
                throw new ArgumentException("Queue URL cannot be null or empty.", nameof(queueUrl));

            if (string.IsNullOrWhiteSpace(eventType))
                throw new ArgumentException("Event type cannot be null or empty.", nameof(eventType));

            var messageBody = JsonConvert.SerializeObject(new
            {
                Event = @event,
                EventType = eventType,
                Timestamp = DateTime.UtcNow
            });

            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = messageBody
            };

            try
            {
                await _sqsClient.SendMessageAsync(sendMessageRequest, cancellationToken);
                _logger.LogInformation("Event published successfully.");
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error occurred while publishing event to SQS.");
                throw new InvalidOperationException("An error occurred while publishing the event to SQS.", ex);
            }
        }
    }
}