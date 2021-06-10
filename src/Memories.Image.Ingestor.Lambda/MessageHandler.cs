﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Memories.Image.Ingestor.Lambda.InboundMessages;
using Serilog;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace Memories.Image.Ingestor.Lambda
{
    public class MessageHandler
    {
        private readonly MessageAttributeHelper _messageAttributeHelper;
        private readonly ILogger _logger;

        public MessageHandler(MessageAttributeHelper messageAttributeHelper, ILogger logger)
        {
            _messageAttributeHelper = messageAttributeHelper;
            _logger = logger;
        }

        public async Task Handle(SQSMessage sqsMessage)
        {
            try
            {
                var messageAttributes = _messageAttributeHelper.Extract(sqsMessage);
                using var trace = new Trace(messageAttributes);

                _logger.Information("Processing message {@message}", sqsMessage.Body);

                switch (messageAttributes.Type)
                {
                    case nameof(ImageCreated):
                        await HandleAccountCreated(sqsMessage);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(messageAttributes.Type), messageAttributes.Type);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error occurred while processing SQS message {@message}", sqsMessage);
                throw;
            }
        }

        private async Task HandleAccountCreated(SQSMessage sqsMessage)
        {
            var receivedMessage = JsonConvert.DeserializeObject<ImageCreated>(sqsMessage.Body);
        }
    }
}