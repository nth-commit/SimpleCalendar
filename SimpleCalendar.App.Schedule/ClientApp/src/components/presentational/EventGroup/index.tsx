import * as React from 'react'
import Typography from '@material-ui/core/Typography'
import { withStyles } from '@material-ui/core'
import { EventGroup, TimeGrouping } from 'src/store/Events'
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

const getEventGroupName = (eventGroup: EventGroup): string => {
  switch (eventGroup.timeGrouping) {
    case TimeGrouping.Today: return 'Today'
    case TimeGrouping.Tomorrow: return 'Tomorrow'
    case TimeGrouping.ThisWeek: return 'This Week'
    case TimeGrouping.NextWeek: return 'Next Week'
    case TimeGrouping.ThisMonth: return 'This Month'
    case TimeGrouping.NextMonth: return 'Next Month'
    case TimeGrouping.Later: return 'Later'
    case TimeGrouping.Tomorrow: return 'Tomorrow'
    default: throw new Error('Unrecognised TimeGrouping')
  }
}

const EventGroup = ({ eventGroup, classes }: EventGroupProps) => (
  <div>
    <Typography variant="caption" align="center" className={classes.typography}>
      {getEventGroupName(eventGroup)}
    </Typography>
    <EventList {...eventGroup} />
  </div>
)

export default withStyles(styles)(EventGroup)
