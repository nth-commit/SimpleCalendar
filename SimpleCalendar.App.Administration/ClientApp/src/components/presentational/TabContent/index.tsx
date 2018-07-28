import * as React from 'react'
import { withStyles } from '@material-ui/core/styles'
import Button from '@material-ui/core/Button'
import AddIcon from '@material-ui/icons/Add'

const styles = theme => ({
  button: {
    margin: theme.spacing.unit,
  }
})

export interface TabContentProps extends React.HTMLProps<{}> {
  hasAddButton?: boolean
  onAddClick?(): void
  classes: any
}

const TabContentUnstyled = ({ hasAddButton, onAddClick, children, classes }: TabContentProps) => (
  <div style={{ height: '100%' }}>
    {children}
    {hasAddButton && <div style={{ position: 'absolute', right: 0, bottom: 0 }}>
      <Button variant="fab" color="primary" aria-label="Add" className={classes.button} onClick={onAddClick}>
        <AddIcon />
      </Button>
    </div>}
  </div>
)

export default withStyles(styles)(TabContentUnstyled)