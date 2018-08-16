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
}

const styles = (theme: Theme) => ({
  card: {
    display: 'flex'
  },
  cardContent: {
    padding: theme.spacing.unit,
    paddingBottom: 0,
    flex: 1
  },
  eventName: {
    ...theme.typography.subheading
  },
  eventDescription: {
    overflow: 'hidden',
    height: '40px',
    color: theme.palette.grey["700"]
  }
})

const EventList = ({ event, timeGrouping, classes }: EventListItemProps & { classes: any }) => (
  <Card className={classes.card}>
    <EventDate event={event} timeGrouping={timeGrouping} size={100} />
    <CardContent className={classes.cardContent}>
      <Typography className={classes.eventName}>{event.name}</Typography>
      <Typography className={classes.eventTimeAndLocation}>
        <span>{moment(event.startTime).format('HH:mm')}</span>
        <span> · </span>
        <span>Cuba Street, Wellington</span>
      </Typography>
      <Typography className={classes.eventDescription}>
        Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.
      </Typography>
    </CardContent>
  </Card>
)

export default withStyles(styles)(EventList)
