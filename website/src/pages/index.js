import React from "react"
import { Link } from "gatsby"

import Layout from "../components/layout"
import SEO from "../components/seo"
import AWS from "aws-sdk"
import Summary from "../components/summary"

import summaries from "../../../dbclient/summaries"; 

const IndexPage = () => {

	AWS.config.update({
		region: "us-west-2",
		endpoint: "http://localhost:9000",
		credentials: {
		  accessKeyId: 'fakeAccessKeyId',
		  secretAccessKey: 'fakeAWSSecretAccessKey'
		}
	});

	var ddb = new AWS.DynamoDB({apiVersion: '2012-08-10'});

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
	
	  ddb.query(params, function(err, data) {
		if (err) {
		  console.log("Error", err);
		} else {
		  //console.log("Success", data.Items);
		  data.Items.forEach(function(element, index, array) {
			console.log(element.Title.S + " (" + element.Subtitle.S + ")");
		  });
		}
	  });

	return (
		<div>
		<Layout>
			<SEO title="Home" />
			<Link to="/article/">New Article</Link>
				This is going to be a website that uses machine learning and natural
				language processing in order to summarizer articles.
			<div>
			{
				summaries.map((summary, _) => {
					return (
						<Summary summary={summary}/>
					);
				})
			}
			</div> 			
		</Layout>
		</div>
	)
}

export default IndexPage
