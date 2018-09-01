import * as React from 'react'
import * as moment from 'moment'
import { withStyles, Theme, StyleRules } from '@material-ui/core/styles'
import { SvgIconProps } from '@material-ui/core/SvgIcon'
import AccessTimeIcon from '@material-ui/icons/AccessTime'
import LocationOnIcon from '@material-ui/icons/LocationOn'
import PersonIcon from '@material-ui/icons/Person'
import { IEvent } from 'src/services/Api'
import HorizontalRule from 'src/components/presentational/HorizontalRule'

interface EventDetailsProps {
  event: IEvent
}

interface EventDetailProps {
  icon: React.ComponentType<SvgIconProps>
  detailLine1: string
  detailLine2?: string
}

const styles = (theme: Theme): StyleRules => ({
  detail: {
    display: 'flex',
    alignItems: 'center',
    minHeight: 3 * theme.spacing.unit,
    '&:not(:first-child)': {
      marginTop: 1.5 * theme.spacing.unit
    }
  },
  detailIcon: {
    color: theme.palette.grey["600"],
    fontSize: 2.5 * theme.spacing.unit,
    marginRight: 1.5 * theme.spacing.unit
  },
  detailText: {
    display: 'flex',
    flexDirection: 'column'
  },
  detailLine1: {},
  detailLine2: {
    ...theme.typography.caption
  },
  description: {
  }
})

const EventDetail = ({ detailLine1, detailLine2, icon, classes }: EventDetailProps & { classes: any }) => (
  <div className={classes.detail}>
    {React.createElement(icon, { className: classes.detailIcon })}
    <div className={classes.detailText}>
      <div className={classes.detailLine1}>{detailLine1}</div>
      {detailLine2 && <div className={classes.detailLine2}>{detailLine2}</div>}
    </div>
  </div>
)

const EventDetails = ({ event, classes }: EventDetailsProps & { classes: any }) => {
  const startTime = moment(event.startTime)
  const isToday = moment().diff(startTime, 'days') === 0

  return (
    <React.Fragment>
      <div style={{ padding: '15px 0' }}>
        <EventDetail
          icon={AccessTimeIcon}
          detailLine1={`${isToday ? 'Today' : startTime.format('dddd')} at ${startTime.format('HH:mm')}`}
          detailLine2={isToday ? undefined : startTime.fromNow(true)}
          classes={classes} />
        <EventDetail
          icon={LocationOnIcon}
          detailLine1={'Wellington'}
          classes={classes}/>
        <EventDetail
          icon={PersonIcon}
          detailLine1={'Tania @ Wellington Animal Save'}
          classes={classes}/>
      </div>
      <HorizontalRule />
      <div style={{ padding: '15px 0' }} className={classes.description}>
        {event.description}
      </div>
    </React.Fragment>
  )
}

export default withStyles(styles)(EventDetails)
