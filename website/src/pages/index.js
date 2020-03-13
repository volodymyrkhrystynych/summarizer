import React from "react"
import { Link } from "gatsby"

import Layout from "../components/layout"
import Image from "../components/image"
import SEO from "../components/seo"

const [getter, setter] = React.useState("")

const IndexPage = () => (
  <Layout>
    <SEO title="Home" />
    <p>This is going to be a website that uses machine learning and natural language processing in order to summarizer articles</p>
    <Link to="/page-2/">Go to page 2</Link>
    <form>
	<label>
		Text in:
		<textarea type="text" name="input" />
	</label>
	<button onClick={() }>Click me</button>
    </form>
  </Layout>
)

    // <div style={{ maxWidth: `300px`, marginBottom: `1.45rem` }}>
    //   <Image />
    // </div>
export default IndexPage
