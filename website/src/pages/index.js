import React, {useEffect} from "react"
import { Link } from "gatsby"

import Layout from "../components/layout"
import SEO from "../components/seo"
import AWS from "aws-sdk"
import SummaryList from "../components/summary-list"

import summaries from "../summaries"; 

const IndexPage = () => {


	const [mystate, setThisState] = React.useState();

	const callApi = async () => {
		const response = await fetch('https://azfzpno1fl.execute-api.us-east-2.amazonaws.com/summariesV1/summaries',
			{
				method: 'GET',
				headers: {
					'Accept': 'application/json',
					'Content-Type': 'application/json'
					}
				});
		const body = await response.json();
		if (response.status !== 200) throw Error(body.message);
		
		return body;
	  };
	
	useEffect( () => {
		console.log('hello');
		callApi()
		.then(res => {console.log('res');console.log(res);setThisState(res);})
		  .catch(err => console.log(err));

	  }, []);

	console.log('summaries');
	console.log(summaries);
	return (
		<Layout>
			<SEO title="Home" />
				This is going to be a website that uses machine learning and natural
				language processing in order to summarizer articles.
			<div>
				{mystate ? <SummaryList summaries={mystate}/> : null}	
			</div> 			
		</Layout>
	)
}

export default IndexPage

				// {mystate.Items}	
