import boto3
import os
import json
import uuid
import Lib

from boto3.dynamodb.conditions import Key

dynamodb = boto3.resource('dynamodb')

TABLE_NAME = os.environ['DYNAMODB_TABLE']
table = dynamodb.Table(TABLE_NAME)

def scan_summaries(date_range, display_movies,dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', endpoint_url="http://localhost:8000")

    done = False
    start_key = None
    while not done:
        response = table.scan()
        display_movies(response.get('Items', []))
        start_key = response.get('LastEvaluatedKey', None)
        done = start_key is None

if __name__ == '__main__':
    def print_movies(movies):
        for movie in movies:
            print(f"\n{movie['pubDate']} : {movie['summary']}")

    query_range = (2000, 3000)
    print(f"Scanning for movies released from {query_range[0]} to {query_range[1]}...")
    scan_summaries(query_range, print_movies)