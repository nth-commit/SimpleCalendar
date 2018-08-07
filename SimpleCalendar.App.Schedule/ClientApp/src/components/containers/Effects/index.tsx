import * as React from 'react'
import { appConnect } from 'src/store'
import { regionActionCreators } from 'src/store/Region'

interface EffectsStateProps {
  hasFetchedRegion: boolean
}

interface EffectsDispatchProps {
  fetchRegion(): void
}

const Effects = ({
  hasFetchedRegion, fetchRegion
}: EffectsStateProps & EffectsDispatchProps) => {

  if (!hasFetchedRegion) {
    fetchRegion()
  }

  return <div />
}

export default appConnect<EffectsStateProps, EffectsDispatchProps>(
  state => ({
    hasFetchedRegion: state.region.isLoading || state.region.region || state.region.error
  }),
  dispatch => ({
    fetchRegion: () => dispatch(regionActionCreators.fetchRegion())
  })
)(Effects)