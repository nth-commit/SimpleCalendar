import * as React from 'react'
import { appConnect } from 'src/store'
import { eventSelectors } from 'src/store/Events'
import { IEventCreate, IEvent } from 'src/services/Api'

interface EventsListStateProps {
  isLoading: boolean
  events: Array<IEvent | IEventCreate>
}

// tslint:disable-next-line:no-empty-interface
interface EventsListDispatchProps {
}

declare type EventsListProps = EventsListStateProps & EventsListDispatchProps

const EventsList = ({ isLoading, events }: EventsListProps) => {
  if (isLoading) {
    return null
  }

  return <pre>{JSON.stringify(events, undefined, '  ')}</pre>
}

export default appConnect<EventsListStateProps, EventsListDispatchProps>(
  state => {
    const isLoading = !eventSelectors.isFetchEventsCompleted(state)
    return {
      isLoading,
      events: isLoading ?  [] : eventSelectors.getEvents(state)
    }
  },
  dispatch => ({
  })
)(EventsList)
