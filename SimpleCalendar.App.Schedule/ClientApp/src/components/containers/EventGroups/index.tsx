import * as React from 'react'
import { appConnect } from 'src/store'
import { IEvent } from 'src/services/Api'
import { eventSelectors, EventGroupCollection } from 'src/store/Events'
import { uiActionCreators } from 'src/store/UI'
import EventList from 'src/components/presentational/EventList'
import EventGroup from 'src/components/presentational/EventGroup'
import { EVENT_DETAILS_DIALOG_ID, EventDetailsDialogOptions } from 'src/components/dialogs/EventDetailsDialog'

interface EventGroupsStateProps {
  isLoading: boolean
  eventGroupCollection: EventGroupCollection | null
}

interface EventGroupsDispatchProps {
  openEventDetails(event: IEvent): void
}

declare type EventGroupsProps = EventGroupsStateProps & EventGroupsDispatchProps

const EventGroups = ({ isLoading, eventGroupCollection, openEventDetails }: EventGroupsProps) => {
  if (isLoading) {
    return null
  }

  if (!eventGroupCollection) {
    throw new Error('Expected eventGroupCollection to be populated when isLoading is false')
  }

  const eventGroups = eventGroupCollection.getGroups()
  if (eventGroups.length === 0) {
    return <div>Nothing here!</div>
  }

  if (eventGroups.length === 1) {
    return <EventList
      events={eventGroups[0].events}
      timeGrouping={eventGroups[0].timeGrouping}
      onEventClick={openEventDetails} />
  }

  return (
    <div>
      {eventGroups.map(g => (
        <div key={g.timeGrouping}>
          <EventGroup
            eventGroup={g}
            onEventClick={openEventDetails} />
        </div>
      ))}
    </div>
  )
}

export default appConnect<EventGroupsStateProps, EventGroupsDispatchProps>(
  state => {
    const isLoading = !eventSelectors.isFetchEventsCompletedSelector(state)
    return {
      isLoading,
      eventGroupCollection: isLoading ? null : eventSelectors.getEventGroupsSelector(state)
    }
  },
  dispatch => ({
    openEventDetails: event => {
      dispatch(uiActionCreators.openDialog(EVENT_DETAILS_DIALOG_ID, {
        eventId: event.id
      } as EventDetailsDialogOptions))
    }
  })
)(EventGroups)
