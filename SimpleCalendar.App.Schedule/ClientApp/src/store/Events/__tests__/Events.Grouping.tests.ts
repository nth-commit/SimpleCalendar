import * as moment from 'moment'
import * as fetchMock from 'fetch-mock'
import configureStore from 'src/__testutils__/configureStore'
import { mock as mockDate, reset as resetDate } from 'src/__testutils__/mocks/date'
import { eventsActionCreators } from '../EventsActionCreators'
import { eventSelectors, TimeGrouping } from '../EventsSelectors'

const MOMENT_ON_MONDAY = Object.freeze(moment.utc('2010-02-15'))

const from = (m: moment.Moment, d: moment.Duration): Readonly<moment.Moment> => {
  const result = moment(m)
  result.add(d)
  return result
}

const fromToday = (d: moment.Duration): Readonly<moment.Moment> =>
  from(MOMENT_ON_MONDAY, d)

interface IEventTime {
  startTime: moment.Moment
  duration: moment.Duration
}

const mockFetchEvents = (events: IEventTime[]) => {
  fetchMock.mock({
    matcher: (urlStr) => {
      const url = new URL(urlStr)
      return url.pathname === '/events/today'
    },
    response: {
      body: events.map(e => ({
        startTime: e.startTime.toISOString(),
        endTime: from(e.startTime, e.duration).toISOString()
      })),
      status: 200
    }
  })
}

describe('Store.Events.Grouping', () => {

  const { dispatch, getState } = configureStore({
    region: {
      isLoading: false,
      region: {
        id: 'new-zealand/wellington',
        name: 'Wellington',
        permissions: {
          canAddMemberships: {},
          canCreateEvents: true,
          canPublishEvents: true
        },
        timezone: 'UTC'
      },
      error: null
    }
  })

  beforeEach(() => {
    mockDate(MOMENT_ON_MONDAY)
  })

  afterEach(() => {
    resetDate()
  })

  test('When there is an event on Monday, then it is grouped in Today', async () => {
    mockFetchEvents([
      {
        startTime: fromToday(moment.duration(6, 'hours')),
        duration: moment.duration(1, 'hours')
      }
    ])

    await dispatch(eventsActionCreators.fetchEvents())

    const eventGroups = eventSelectors.getEventGroups(getState())
    expect(eventGroups.hasGroup(TimeGrouping.Today)).toEqual(true)
  })

  test('When there is an event that started the previous Sunday and is in progress, then it is grouped in Today', async () => {
    mockFetchEvents([
      {
        startTime: fromToday(moment.duration(-1, 'days').add(moment.duration(6, 'hours'))),
        duration: moment.duration(2, 'days')
      }
    ])

    await dispatch(eventsActionCreators.fetchEvents())

    const eventGroups = eventSelectors.getEventGroups(getState())
    expect(eventGroups.hasGroup(TimeGrouping.Today)).toEqual(true)
  })

  test('When there is an event on Tuesday, then it is grouped in Tomorrow', async () => {
    mockFetchEvents([
      {
        startTime: fromToday(moment.duration(1, 'days').add(moment.duration(6, 'hours'))),
        duration: moment.duration(1, 'hours')
      }
    ])

    await dispatch(eventsActionCreators.fetchEvents())

    const eventGroups = eventSelectors.getEventGroups(getState())
    expect(eventGroups.hasGroup(TimeGrouping.Tomorrow)).toEqual(true)
  })

  test('When there is an event on Wednesday, then it is grouped in ThisWeek', async () => {
    mockFetchEvents([
      {
        startTime: fromToday(moment.duration(2, 'days').add(moment.duration(6, 'hours'))),
        duration: moment.duration(1, 'hours')
      }
    ])

    await dispatch(eventsActionCreators.fetchEvents())

    const eventGroups = eventSelectors.getEventGroups(getState())
    expect(eventGroups.hasGroup(TimeGrouping.ThisWeek)).toEqual(true)
  })

  test('When there is an event that started the previous Sunday and is in progress, then it is grouped in Today', async () => {
    mockFetchEvents([
      {
        startTime: fromToday(moment.duration(-1, 'days').add(moment.duration(6, 'hours'))),
        duration: moment.duration(2, 'days')
      }
    ])

    await dispatch(eventsActionCreators.fetchEvents())

    const eventGroups = eventSelectors.getEventGroups(getState())
    expect(eventGroups.hasGroup(TimeGrouping.Today)).toEqual(true)
  })
})