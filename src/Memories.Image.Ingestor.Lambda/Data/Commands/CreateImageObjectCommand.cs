﻿using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Memories.Image.Ingestor.Lambda.Commands;
using Memories.Image.Ingestor.Lambda.Common;
using Memories.Image.Ingestor.Lambda.Data.Requests;
using Microsoft.Extensions.Options;
using Serilog;

namespace Memories.Image.Ingestor.Lambda.Data.Commands
{
    public interface ICreateImageObjectCommand
    {
        Task CreateImageObject(CreateImageRequest request);
    }

    public class CreateImageObjectCommand : CommandBase, ICreateImageObjectCommand
    {
        private readonly ILogger _logger;
        public CreateImageObjectCommand(IAmazonDynamoDB dynamoDbClient, IOptions<DynamoDbOptions> dynamoDbOptions,
            ILogger logger) : base(dynamoDbClient, dynamoDbOptions.Value)
        {
            _logger = logger;
        }

        public async Task CreateImageObject(CreateImageRequest request)
        {

            var imageObjectItem = new MemoriesModel
            {
                Account = request.Account,
                Album = request.Album,
                Filename = request.Filename,
                ObjectKey = request.ObjectKey,
                UploadDate = request.UploadDate
            }.ToAttributeValueMap();

            try
            {
                await DynamoDbClient.PutItemAsync(TableName, imageObjectItem);
            }
            catch (Exception e)
            {
                _logger.Information("Failed to add item to dynamo db for key {Key} created on {Date} \n Exception Message: {Exception}"
                    , request.ObjectKey, request.UploadDate, e.Message);

                throw e;
            }
        }
    }
}