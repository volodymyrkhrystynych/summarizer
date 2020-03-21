import React from "react"
import { Link } from "gatsby"

import Layout from "../components/layout"
import Article from "../components/article"
import SEO from "../components/seo"

const IndexPage = () => {
	return (
		<Layout>
			<SEO title="Home" />
			<p>
				This is going to be a website that uses machine learning and natural
				language processing in order to summarizer articles
			</p>
			<Article/>
		</Layout>
	)
}

export default IndexPage
