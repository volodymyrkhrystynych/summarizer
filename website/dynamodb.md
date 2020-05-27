# DynamoDb 

## 1. List tables
```
var params = {
    Limit: 10, // optional (to further limit the number of table names returned per page)
};
dynamodb.listTables(params, function(err, data) {
    if (err) ppJson(err); // an error occurred
    else ppJson(data); // successful response
});
```

## 2. Query data
```
var params = {
    TableName: 'Summaries',
    KeyConditionExpression: '#attr_name = :value', // a string representing a constraint on the attribute
    ExpressionAttributeNames: { // a map of substitutions for attribute names with special characters
        '#attr_name': 'pubDate'
    },
    ExpressionAttributeValues: { // a map of substitutions for all attribute values
      ':value': '5-Apr-20'
    },
    Limit: 10, // optional (limit the number of items to evaluate)
    ConsistentRead: false, // optional (true | false)
    ReturnConsumedCapacity: 'NONE', // optional (NONE | TOTAL | INDEXES)
};
docClient.query(params, function(err, data) {
    if (err) ppJson(err); // an error occurred
    else ppJson(data); // successful response
});
```
