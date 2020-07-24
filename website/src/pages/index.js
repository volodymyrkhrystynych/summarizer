import React from "react"
import { Link } from "gatsby"

import Layout from "../components/layout"
import SEO from "../components/seo"
import AWS from "aws-sdk"
import Summary from "../components/summary"

// import summaries from "../summaries"; 

const IndexPage = () => {

	state = {
		response: '',
		post: '',
		responseToPost: '',
	  };

	const [mystate, setThisState] = React.useState({
		response: '',
		post: '',
		responseToPost: '',
	  });

	componentDidMount = () => {
		callApi()
		  .then(res => setThisState({ response: res.express }))
		  .catch(err => console.log(err));
	  }

	callApi = async () => {
		const response = await fetch('https://azfzpno1fl.execute-api.us-east-2.amazonaws.com/summariesV1');
		const body = await response.json();
		if (response.status !== 200) throw Error(body.message);
		
		return body;
	  };


	return (
		<Layout>
			<SEO title="Home" />
				This is going to be a website that uses machine learning and natural
				language processing in order to summarizer articles.
			<div>
			{
				mystate.response.map((Summary, _) => {
					return (
						<Summary summary={summary}/>
					);
				})
			}
			</div> 			
		</Layout>
	)
}

export default IndexPage
