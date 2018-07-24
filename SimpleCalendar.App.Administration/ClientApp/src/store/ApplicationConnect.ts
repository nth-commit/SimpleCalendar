import { connect, InferableComponentEnhancerWithProps } from 'react-redux'
import { ApplicationState } from './ApplicationState'
import { ApplicationDispatch } from './ApplicationStore'

interface ApplicationDispatchProp {
  dispatch: ApplicationDispatch
}

interface MapStateToProps<TStateProps, TOwnProps> {
  // tslint:disable-next-line:callable-types
  (state: ApplicationState, ownProps: TOwnProps): TStateProps
}

interface MapStateToPropsFactory<TStateProps, TOwnProps> {
  // tslint:disable-next-line:callable-types
  (initialState: ApplicationState, ownProps: TOwnProps): MapStateToProps<TStateProps, TOwnProps>
}

type MapStateToPropsParam<TStateProps, TOwnProps, State> = MapStateToPropsFactory<TStateProps, TOwnProps> | MapStateToProps<TStateProps, TOwnProps> | null | undefined

interface MapDispatchToPropsFunction<TDispatchProps, TOwnProps> {
  // tslint:disable-next-line:callable-types
  (dispatch: ApplicationDispatch, ownProps: TOwnProps): TDispatchProps
}

type MapDispatchToProps<TDispatchProps, TOwnProps> = MapDispatchToPropsFunction<TDispatchProps, TOwnProps> | TDispatchProps

interface MapDispatchToPropsFactory<TDispatchProps, TOwnProps> {
  // tslint:disable-next-line:callable-types
  (dispatch: ApplicationDispatch, ownProps: TOwnProps): MapDispatchToProps<TDispatchProps, TOwnProps>
}

type MapDispatchToPropsParam<TDispatchProps, TOwnProps> = MapDispatchToPropsFactory<TDispatchProps, TOwnProps> | MapDispatchToProps<TDispatchProps, TOwnProps>


interface MergeProps<TStateProps, TDispatchProps, TOwnProps, TMergedProps> {
  // tslint:disable-next-line:callable-types
  (stateProps: TStateProps, dispatchProps: TDispatchProps, ownProps: TOwnProps): TMergedProps
}

export interface ApplicationConnect {
  (): InferableComponentEnhancerWithProps<{}, {}> 

  <TStateProps = {}, no_dispatch = {}, TOwnProps = {}>(
    mapStateToProps: MapStateToProps<TStateProps, TOwnProps>
  ): InferableComponentEnhancerWithProps<TStateProps & ApplicationDispatchProp, TOwnProps>

  <no_state = {}, TDispatchProps = {}, TOwnProps = {}>(
    mapStateToProps: null | undefined,
    mapDispatchToProps: MapDispatchToPropsParam<TDispatchProps, TOwnProps>
  ): InferableComponentEnhancerWithProps<TDispatchProps, TOwnProps>

  <TStateProps = {}, TDispatchProps = {}, TOwnProps = {}>(
    mapStateToProps: MapStateToPropsParam<TStateProps, TOwnProps, ApplicationState>,
    mapDispatchToProps: MapDispatchToPropsParam<TDispatchProps, TOwnProps>
  ): InferableComponentEnhancerWithProps<TStateProps & TDispatchProps, TOwnProps>

  <TStateProps = {}, no_dispatch = {}, TOwnProps = {}, TMergedProps = {}>(
    mapStateToProps: MapStateToProps<TStateProps, TOwnProps>,
    mapDispatchToProps: null | undefined,
    mergeProps: MergeProps<TStateProps, ApplicationDispatchProp, TOwnProps, TMergedProps>
  ): InferableComponentEnhancerWithProps<TMergedProps, TOwnProps>
}

export const appConnect = connect as ApplicationConnect