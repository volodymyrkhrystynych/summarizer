import React from "react"
import Summary from "./summary"


const SummaryList = (props) => {
	return (
		<div>
			{
				props.summaries.map((summary, _) => {
					return (
						<Summary summary={summary}/>
					);
				})
			}
		</div>
		)

}


export default SummaryList
