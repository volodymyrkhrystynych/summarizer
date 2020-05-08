<<<<<<< HEAD
import { Link } from "gatsby"
import PropTypes from "prop-types"
import React from "react"
import {Grid, Container, Button, TextField} from "@material-ui/core"

const Article = () => {
	const [getter, setter] = React.useState({ text: "" })
=======
import { Link } from "gatsby";
import PropTypes from "prop-types";
import React from "react";
import { useState } from "react";

const Article = () => {
	const [getter, setter] = useState({ text: "" })
>>>>>>> 028a612398976cc2f4335c55cf9cc3c502131207
	const buttonHandler = event => {
		event.preventDefault()
		console.log(getter)
	}
	return (
		<form onSubmit={buttonHandler}>
<<<<<<< HEAD
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
=======
			<label>
				Article Text:
			</label>
			<textarea
				type="text"
				name="input"
				value={getter.text}
				onChange={event => setter({ text: event.target.value })}
			/>
			<button>Click me</button>
>>>>>>> 028a612398976cc2f4335c55cf9cc3c502131207
		</form>
	)
}

<<<<<<< HEAD
export default Article
=======
export default Article
>>>>>>> 028a612398976cc2f4335c55cf9cc3c502131207
