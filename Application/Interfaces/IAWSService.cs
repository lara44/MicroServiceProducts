using Amazon.SQS;

namespace Application.Interfaces
{
    public interface IAwsService
    {
        IAmazonSQS CreateSqsClient();
    }
}