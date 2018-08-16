import * as React from 'react'
import { IEvent } from 'src/services/Api'
import { withStyles } from '@material-ui/core'
import EventListItem from './EventListItem'
import { TimeGrouping } from 'src/store/Events'

const styles = theme => ({
  event: {
    'margin-top': 2 * theme.spacing.unit
  }
})

interface EventListProps {
  events: IEvent[]
  timeGrouping: TimeGrouping
  classes: any
}

const EventList = ({ events, timeGrouping, classes }: EventListProps) =>
  <React.Fragment>
    {events.map(e => (
      <div key={e.id} className={classes.event}>
        <EventListItem event={e} timeGrouping={timeGrouping} />
      </div>
    ))}
  </React.Fragment>

export default withStyles(styles)(EventList)
