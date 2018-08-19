import * as React from 'react'
import * as moment from 'moment'
import Card from '@material-ui/core/Card'
import CardContent from '@material-ui/core/CardContent'
import Typography from '@material-ui/core/Typography'
import { withStyles, Theme } from '@material-ui/core/styles'
import { IEvent } from 'src/services/Api'
import { TimeGrouping } from 'src/store/Events'
import EventDate from './EventDate'

interface EventListItemProps {
  event: IEvent
  timeGrouping: TimeGrouping
  onClick?(): void
}

const styles = (theme: Theme) => ({
  card: {
    display: 'flex'
  },
  cardContent: {
    padding: theme.spacing.unit,
    paddingBottom: '0 !important',
    flex: 1
  },
  eventName: {
    ...theme.typography.subheading
  },
  eventHost: {
    overflow: 'hidden',
    height: '40px',
    color: theme.palette.grey["700"]
  }
})

const EventList = ({ event, timeGrouping, onClick, classes }: EventListItemProps & { classes: any }) => (
  <Card className={classes.card} onClick={onClick} style={{ cursor: onClick ? 'pointer' : 'default' }}>
    <EventDate event={event} timeGrouping={timeGrouping} size={100} />
    <CardContent className={classes.cardContent}>
      <Typography className={classes.eventName}>{event.name}</Typography>
      <Typography className={classes.eventTimeAndLocation}>
        <span>{moment(event.startTime).format('HH:mm')}</span>
        <span> · </span>
        <span>Cuba Street, Wellington</span>
      </Typography>
      <Typography className={classes.eventHost}>Speak Up For Animals</Typography>
    </CardContent>
  </Card>
)

export default withStyles(styles)(EventList)
