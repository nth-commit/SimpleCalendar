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
  onEventClick?(event: IEvent): void
  classes: any
}

const EventList = ({ events, timeGrouping, onEventClick, classes }: EventListProps) =>
  <React.Fragment>
    {events.map(e => (
      <div key={e.id} className={classes.event}>
        <EventListItem
          event={e}
          timeGrouping={timeGrouping}
          onClick={onEventClick ? onEventClick.bind(null, e) : undefined} />
      </div>
    ))}
  </React.Fragment>

export default withStyles(styles)(EventList)
