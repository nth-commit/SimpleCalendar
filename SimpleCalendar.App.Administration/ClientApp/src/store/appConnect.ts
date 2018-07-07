import { connect } from 'react-redux';
import { ApplicationState } from './ApplicationState';

// TODO: Better typings here? Especially with dispatch
export const appConnect = <TStateProps = {}, TDispatchProps = {}>(
  mapStateToProps?: (state: ApplicationState) => TStateProps,
  mapDispatchToProps?: (dispatch: (...args: any[]) => void) => TDispatchProps
) => connect(mapStateToProps, mapDispatchToProps as any);
