import { IEventCreate, Api } from 'src/services/Api'
import { ApplicationThunkActionAsync } from 'src/store'
import { regionSelectors } from 'src/store/Region'
import * as EventsActions from './EventsActions'
import { hasFetchEventsStartedSelector } from './EventsSelectors'

export type IEventCreateGivenRegion = Pick<IEventCreate, Exclude<keyof IEventCreate, 'regionId'>>

let trackingIdCounter = 0

export const eventsActionCreators = {

  fetchEvents: (collection: EventsActions.EventCollectionType = EventsActions.EventCollectionType.TODAY): ApplicationThunkActionAsync => async (dispatch, getState) => {
    const state = getState()

    if (hasFetchEventsStartedSelector(state, collection)) {
      dispatch(new EventsActions.FetchEventsError(collection, 'Event fetch has already started'))
      return
    }

    if (!regionSelectors.isFetchRegionCompleted(state)) {
      dispatch(new EventsActions.FetchEventsError(collection, 'Region has not been loaded'))
      return
    }

    dispatch(new EventsActions.FetchEventsBegin(collection))

    const region = regionSelectors.getRegion(state)
    try {
      const events = await new Api(state.auth.accessToken).queryEventsToday(region.id, region.timezone)
      dispatch(new EventsActions.FetchEventsComplete(collection, events))
    } catch (e) {
      dispatch(new EventsActions.FetchEventsError(collection, e))
    }
  },

  createEvent: (create: IEventCreateGivenRegion, collection: EventsActions.EventCollectionType = EventsActions.EventCollectionType.TODAY): ApplicationThunkActionAsync => async (dispatch, getState) => {
    const state = getState()
    const region = regionSelectors.getRegion(state)

    const createWithRegion: IEventCreate = { ...create, regionId: region.id }
    const trackingId = (trackingIdCounter++).toString()
    dispatch(new EventsActions.CreateEventBegin(collection, createWithRegion, trackingId))

    const api = new Api(state.auth.accessToken)
    try {
      const event = await api.createEvent(createWithRegion)
      dispatch(new EventsActions.CreateEventComplete(collection, event, trackingId))
    } catch (e) {
      dispatch(new EventsActions.CreateEventError(collection, e, trackingId))
    }
  }
}