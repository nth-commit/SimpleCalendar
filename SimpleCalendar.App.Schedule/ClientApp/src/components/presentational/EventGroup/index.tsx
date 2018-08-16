import * as React from 'react'
import Typography from '@material-ui/core/Typography'
import { withStyles } from '@material-ui/core'
import { EventGroup } from 'src/store/Events'
import EventList from 'src/components/presentational/EventList'

const styles = theme => ({
  typography: {
    'margin-top': 5 * theme.spacing.unit
  }
})

interface EventGroupProps {
  eventGroup: EventGroup
  classes: any
}

const EventGroup = ({ eventGroup, classes }: EventGroupProps) => (
  <div>
    <Typography variant="caption" align="center" className={classes.typography}>
      {eventGroup.name}
    </Typography>
    <EventList {...eventGroup} />
  </div>
)

export default withStyles(styles)(EventGroup)
