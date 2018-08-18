import * as React from 'react'
import { appConnect } from 'src/store'
import { eventSelectors, EventGroupCollection } from 'src/store/Events'
import EventList from 'src/components/presentational/EventList'
import EventGroup from 'src/components/presentational/EventGroup'

interface EventGroupsStateProps {
  isLoading: boolean
  eventGroupCollection: EventGroupCollection | null
}

// tslint:disable-next-line:no-empty-interface
interface EventGroupsDispatchProps {
  onEventClick(): void
}

declare type EventGroupsProps = EventGroupsStateProps & EventGroupsDispatchProps

const EventGroups = ({ isLoading, eventGroupCollection }: EventGroupsProps) => {
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
    return <EventList {...eventGroups[0]} />
  }

  return (
    <div>
      {eventGroups.map(g => (
        <div key={g.timeGrouping}>
          <EventGroup eventGroup={g} />
        </div>
      ))}
    </div>
  )
}

export default appConnect<EventGroupsStateProps, EventGroupsDispatchProps>(
  state => {
    const isLoading = !eventSelectors.isFetchEventsCompleted(state)
    return {
      isLoading,
      eventGroupCollection: isLoading ? null : eventSelectors.getEventGroups(state)
    }
  },
  dispatch => ({
    onEventClick: () => { }
  })
)(EventGroups)
