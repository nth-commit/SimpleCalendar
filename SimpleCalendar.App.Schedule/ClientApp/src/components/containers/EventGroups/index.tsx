import * as React from 'react'
import { appConnect } from 'src/store'
import { eventSelectors, EventGroup } from 'src/store/Events'
import { IEventCreate, IEvent } from 'src/services/Api'

interface EventGroupsStateProps {
  isLoading: boolean
  eventGroups: EventGroup[]
}

// tslint:disable-next-line:no-empty-interface
interface EventGroupsDispatchProps {
}

declare type EventGroupsProps = EventGroupsStateProps & EventGroupsDispatchProps

const EventGroup = (eventGroup: EventGroup) => (
  <div>
    <div>{eventGroup.name}</div>
    {EventList(eventGroup.events)}
  </div>
)

const EventList = (events: Array<IEvent | IEventCreate>) => <pre>{JSON.stringify(events, undefined, '  ')}</pre>

const EventGroups = ({ isLoading, eventGroups }: EventGroupsProps) => {
  if (isLoading) {
    return null
  }

  if (eventGroups.length === 0) {
    return <div>Nothing here!</div>
  }

  if (eventGroups.length === 1) {
    return EventList(eventGroups[0].events)
  }

  return (
    <div>
      {eventGroups.map(g => (
        <div key={g.name}>
          {EventGroup(g)}
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
      eventGroups: isLoading ?  [] : eventSelectors.getEventGroups(state)
    }
  },
  dispatch => ({
  })
)(EventGroups)
