import * as React from 'react'
import { appConnect } from 'src/store'
import { regionActionCreators, regionSelectors } from 'src/store/Region'
import { eventsActionCreators, eventSelectors } from 'src/store/Events'

interface EffectsStateProps {
  hasFetchRegionStarted: boolean
  hasFetchEventsStarted: boolean
}

interface EffectsDispatchProps {
  fetchRegion(): Promise<void>
  fetchEvents(): Promise<void>
}

class Effects extends React.PureComponent<EffectsStateProps & EffectsDispatchProps> {

  async componentDidMount() {
    const {
      hasFetchRegionStarted, fetchRegion,
      hasFetchEventsStarted, fetchEvents
    } = this.props

    if (!hasFetchRegionStarted) {
      await fetchRegion()
    }

    if (!hasFetchEventsStarted) {
      await fetchEvents()
    }
  }

  render() {
    return null
  }
}

export default appConnect<EffectsStateProps, EffectsDispatchProps>(
  state => ({
    hasFetchRegionStarted: regionSelectors.hasFetchRegionStarted(state.region),
    hasFetchEventsStarted: eventSelectors.hasFetchEventsStarted(state.events)
  }),
  dispatch => ({
    fetchRegion: () => dispatch(regionActionCreators.fetchRegion()),
    fetchEvents: () => dispatch(eventsActionCreators.fetchEvents())
  })
)(Effects)
