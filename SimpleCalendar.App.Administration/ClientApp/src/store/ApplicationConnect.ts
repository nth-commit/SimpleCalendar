import { connect, InferableComponentEnhancerWithProps } from 'react-redux';
import { ApplicationState } from './ApplicationState';
import { ApplicationDispatch } from './ApplicationStore';

interface ApplicationDispatchProp {
  dispatch: ApplicationDispatch;
}

interface MapStateToProps<TStateProps, TOwnProps> {
  // tslint:disable-next-line:callable-types
  (state: ApplicationState, ownProps: TOwnProps): TStateProps;
}

interface MergeProps<TStateProps, TDispatchProps, TOwnProps, TMergedProps> {
  // tslint:disable-next-line:callable-types
  (stateProps: TStateProps, dispatchProps: TDispatchProps, ownProps: TOwnProps): TMergedProps;
}

export interface ApplicationConnect {
  // tslint:disable-next-line:callable-types
  <TStateProps = {}, TOwnProps = {}>(
    mapStateToProps: MapStateToProps<TStateProps, TOwnProps>
  ): InferableComponentEnhancerWithProps<TStateProps & ApplicationDispatch, {}>

  <TStateProps = {}, TOwnProps = {}, TMergedProps = {}>(
    mapStateToProps: MapStateToProps<TStateProps, TOwnProps>,
    mapDispatchToProps: null | undefined,
    mergeProps: MergeProps<TStateProps, ApplicationDispatchProp, TOwnProps, TMergedProps>
  ): InferableComponentEnhancerWithProps<TMergedProps, TOwnProps>;
}

export const appConnect = connect as ApplicationConnect;