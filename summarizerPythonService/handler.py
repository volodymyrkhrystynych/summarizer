import boto3
import os
import json
import uuid

from boto3.dynamodb.conditions import Key

dynamodb = boto3.resource('dynamodb')

TABLE_NAME = os.environ['DYNAMODB_TABLE']
table = dynamodb.Table(TABLE_NAME)

def scan_summaries(date_range=None, title=None , dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', endpoint_url="http://localhost:8000")

    scan_kwargs = {
        'FilterExpression': Key('year').between(*date_range),
        'ProjectionExpression': "#yr, title, info.rating",
        'ExpressionAttributeNames': {"#yr": "year"}
    }

if __name__ == '__main__':
    def print_movies(movies):
        for movie in movies:
            print(f"\n{movie['year']} : {movie['title']}")
            pprint(movie['info'])

    query_range = (1950, 1959)
    print(f"Scanning for movies released from {query_range[0]} to {query_range[1]}...")
    scan_movies(query_range, print_movies)