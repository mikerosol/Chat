using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Models;

namespace Chat.Messages
{
    public abstract class KafkaMessage
    {
        public MetaDataModel MetaData { get; set; }

        public KafkaMessage()
        {
            MetaData = new MetaDataModel()
            {
                MessageType = MessageTypeEnum.UndefinedMessage,
                MessageCreated = DateTime.Now
            };
        }

        public class MetaDataModel
        {
            //public Guid? CorrelationId { get; set; }
            public MessageTypeEnum MessageType { get; set; }
            public DateTime MessageCreated { get; set; }
            public DateTime MessageConsumed { get { return DateTime.Now; } }
        }

        public string ToJsonString()
        {
            return $"============================================================================================================================\n" +
                $"{MetaData.MessageType}: {JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter())}";
        }
    }
}