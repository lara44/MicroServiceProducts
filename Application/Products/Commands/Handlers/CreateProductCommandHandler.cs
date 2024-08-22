
using MediatR;
using Domain.Entities;
using Domain.Repositories;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Products.Commands.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAwsService _awsService;
        private readonly string _productQueueUrl;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IAwsService awsService,
            IConfiguration configuration
        )
        {
            _productRepository = productRepository;
            _awsService = awsService;
            _productQueueUrl = configuration["AWS:SQS:Queues:ProductEventsQueue"]
                ?? throw new ArgumentException("The Product Events Queue URL is missing or incomplete in the configuration.", 
                configuration["AWS:SQS:Queues:ProductEventsQueue"]);
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(Guid.NewGuid(), request.Name, request.Price, request.Stock);
            await _productRepository.AddAsync(product);

            var sqsClient = _awsService.CreateSqsClient();

            var message = new SendMessageRequest
            {
                QueueUrl = _productQueueUrl,
                MessageBody = JsonConvert.SerializeObject(new
                {
                    ProductId = product.Id,
                    Name = product.Name!,
                    Price = product.Price!,
                    EventType = "ProductCreated"
                })
            };

            await sqsClient.SendMessageAsync(message);

            return product.Id;
        }
    }
}