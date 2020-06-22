import React from "react"
import summaryStyles from "./summary.module.css"
import {Grid, Paper} from "@material-ui/core"

const Summary = (props) => {
	return (
        <Paper elevation={3} className={summaryStyles.paper}>
            <Grid container spacing={3} direction="row" alignItems="center">
                <Grid item xs className={summaryStyles.image}>

                </Grid>
                <Grid item xs={10}>
                    <div>
                        <a href={props.summary.link}>{props.summary.title}</a>
                    </div>
                    <div>
                        <span>{props.summary.pubDate}</span>
                        <p>{props.summary.summary}</p>
                    </div>
                </Grid>
            </Grid>
        </Paper>
	)
}

export default Summary
