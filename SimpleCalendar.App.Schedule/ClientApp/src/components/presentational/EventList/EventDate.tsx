import * as React from 'react'
import * as moment from 'moment'
import { withStyles, Theme, StyleRules } from '@material-ui/core/styles'
import { IEvent } from 'src/services/Api'
import { TimeGrouping } from 'src/store/Events'

interface EventListItemProps {
  event: IEvent
  timeGrouping: TimeGrouping
  size: number
}

const styles = (theme: Theme): StyleRules => ({
  container: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
    backgroundColor: theme.palette.primary.light
  },
  majorDatePart: {
    ...theme.typography.subheading,
    textTransform: 'uppercase',
    color: theme.palette.getContrastText(theme.palette.primary.light)
  },
  minorDatePart: {
    ...theme.typography.caption,
    color: theme.palette.getContrastText(theme.palette.primary.light),
    marginBottom: 1.5 * theme.spacing.unit
  }
})

const EventDate = ({ event, timeGrouping, size, classes }: EventListItemProps & { classes: any }) => {
  const majorDatePart =
    timeGrouping === TimeGrouping.Today ? moment(event.startTime).format('HH:mm') :
    timeGrouping === TimeGrouping.Tomorrow ? 'Tomorrow' :
    moment(event.startTime).format('MMM')

  const minorDatePart =
    timeGrouping === TimeGrouping.Today ? null :
    timeGrouping === TimeGrouping.Tomorrow ? moment(event.startTime).format('HH:mm') :
    moment(event.startTime).format('DD')

  return (
    <div className={classes.container} style={{ width: size + 'px', height: size + 'px' }}>
      <div className={classes.majorDatePart}>{majorDatePart}</div>
      {minorDatePart && <div className={classes.minorDatePart}>{minorDatePart}</div>}
    </div>
  )
}

export default withStyles(styles)(EventDate)
