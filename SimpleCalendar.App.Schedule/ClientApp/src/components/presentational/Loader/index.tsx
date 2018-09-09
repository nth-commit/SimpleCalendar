import * as React from 'react'
import { applicationConnect, ApplicationThunkActionAsync, ApplicationState } from 'src/store'

interface LoaderStateProps {
  isLoaded: boolean
}

interface LoaderDispatchProps {
  load(): void
}

interface LoaderOwnProps {
  loadAction: () => ApplicationThunkActionAsync
  isLoadedFunc?: (state: ApplicationState) => boolean
}

class Loader extends React.Component<LoaderStateProps & LoaderDispatchProps> {

  async componentDidMount() {
    this.props.load()
  }

  render() {
    if (!this.props.isLoaded) {
      return <div className="loading" />
    }

    return <div>{this.props.children}</div>
  }
}

export default applicationConnect<LoaderStateProps, LoaderDispatchProps, LoaderOwnProps>(
  (state, { isLoadedFunc }) => ({
    isLoaded: !isLoadedFunc || isLoadedFunc(state)
  }),
  (dispatch, { loadAction }) => ({
    load: () => dispatch(loadAction())
  })
)(Loader)
