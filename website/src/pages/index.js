import React from "react"
import { Link } from "gatsby"

import Layout from "../components/layout"
import article from "../components/article"
import SEO from "../components/seo"

const IndexPage = () => {
	return (
		<Layout>
			<SEO title="Home" />
			<p>
				This is going to be a website that uses machine learning and natural
				language processing in order to summarizer articles
			</p>
			<article/>
		</Layout>
	)
}

// <div style={{ maxWidth: `300px`, marginBottom: `1.45rem` }}>
//   <Image />
// </div>
export default IndexPage
