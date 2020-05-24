using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;

namespace dbclient
{
    [DynamoDBTable("Summaries")]
    public class Summary
    {
        // https://www.w3schools.com/xml/xml_rss.asp

        [DynamoDBHashKey]
        public string title { get; set; }

        //<pubDate>	Optional. Defines the last publication date for the content of the feed
        [DynamoDBRangeKey]
        public DateTime pubDate { get; set; }

        // <link>	Required. Defines the hyperlink to the channel
        public string link { get; set; }


        public string summary { get; set; }

        public List<string> Genres { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0} - {1}", title, pubDate);
        }
    }

    public static class Requests
    {
        public static CreateTableRequest Summaries()
        {
            return new CreateTableRequest
            {
                TableName = "Summaries",
                ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 10, WriteCapacityUnits = 5 },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "pubDate",
                        KeyType = KeyType.HASH
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "title",
                        KeyType = KeyType.RANGE
                    }
                },
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition { AttributeName = "pubDate", AttributeType = ScalarAttributeType.S },
                    new AttributeDefinition { AttributeName = "title", AttributeType = ScalarAttributeType.S }
                }
            };
        }

        // Querying a Global Secondary Index
        // https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/GSI.html#GSI.Querying
        public static QueryOperationConfig pubDate(string date, int limit = 1)
        {
            var filter = new QueryFilter();
            filter.AddCondition("pubDate", QueryOperator.Equal, date);
            return new QueryOperationConfig
            {
                Filter = filter,
                Limit = limit
            };
        }
    }
}