import { Link } from "gatsby"
import PropTypes from "prop-types"
import React from "react"

const article = () => {
	const [getter, setter] = React.useState({ text: "" })
	const buttonHandler = event => {
		event.preventDefault()
		console.log(getter)
	}
	return (
		<form onSubmit={buttonHandler}>
			<label>
				<textarea
					type="text"
					name="input"
					value={getter.text}
					onChange={event => setter({ text: event.target.value })}
				/>
			</label>
			<button>Click me</button>
		</form>
	)
}
