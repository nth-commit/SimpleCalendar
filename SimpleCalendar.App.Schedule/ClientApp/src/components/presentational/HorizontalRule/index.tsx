import * as React from 'react'
import { withStyles, Theme, StyleRules } from '@material-ui/core/styles'


const styles = (theme: Theme): StyleRules => ({
  hr: {
    backgroundColor: theme.palette.grey["300"],
    height: '1px',
    width: 'calc(100% - 20px)',
    margin: '0 10px',
    border: 0
  }
})

const HorizontalRule = ({ classes }: { classes: any }) => <hr className={classes.hr} />

export default withStyles(styles)(HorizontalRule)