import { Link } from "gatsby";
import PropTypes from "prop-types";
import React from "react";
import { useState } from "react";

const Article = () => {
	const [getter, setter] = useState({ text: "" })
	const buttonHandler = event => {
		event.preventDefault()
		console.log(getter)
	}
	return (
		<form onSubmit={buttonHandler}>
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
		</form>
	)
}

export default Article
