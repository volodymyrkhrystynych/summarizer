import { Link } from "gatsby"
import PropTypes from "prop-types"
import Layout from "../components/layout"
import SEO from "../components/seo"
import React from "react"
import {Grid, Container, Button, TextField} from "@material-ui/core"

const Article = () => {
	const [getter, setter] = React.useState({ text: "" })
	const buttonHandler = event => {
		event.preventDefault()
		console.log(getter)
	}
	return (
		<Layout>
			<SEO title="Home" />
			<form onSubmit={buttonHandler}>
				<Grid container
				direction="column"
				justify="center"
				alignItems="center"
				>
					<label>
						Paste the article text into here:
					</label>
					<Container maxWidth='xl'>
					<TextField
						class=".MuiContainer-maxWidthXl"
						type="text"
						name="input"
						multiline
						value={getter.text}
						onChange={event => setter({ text: event.target.value })}
					/>
					</Container>
					<Button>Click me</Button>
				</Grid>
			</form>
		</Layout>
	)
}

export default Article
