// tslint:disable:unified-signatures
// tslint:disable:callable-types
// tslint:disable:max-line-length

// This file is copied and pasted from the react-redux types, and altered to reference the types involved in our redux setup.
import {
  InferableComponentEnhancer,
  InferableComponentEnhancerWithProps,
  Options,
  connect
} from 'react-redux'
import { ApplicationDispatchProp, ApplicationDispatch } from './ApplicationStore'
import { ApplicationState } from './ApplicationState'

export interface ApplicationConnect {
  (): InferableComponentEnhancer<ApplicationDispatchProp>

  <TStateProps = {}, no_dispatch = {}, TOwnProps = {}>(
      mapStateToProps: MapStateToPropsParam<TStateProps, TOwnProps, ApplicationState>
  ): InferableComponentEnhancerWithProps<TStateProps & ApplicationDispatchProp & TOwnProps, TOwnProps>

  <no_state = {}, TDispatchProps = {}, TOwnProps = {}>(
      mapStateToProps: null | undefined,
      mapDispatchToProps: MapDispatchToPropsParam<TDispatchProps, TOwnProps>
  ): InferableComponentEnhancerWithProps<TDispatchProps & TOwnProps, TOwnProps>

  <TStateProps = {}, TDispatchProps = {}, TOwnProps = {}>(
      mapStateToProps: MapStateToPropsParam<TStateProps, TOwnProps, ApplicationState>,
      mapDispatchToProps: MapDispatchToPropsParam<TDispatchProps, TOwnProps>
  ): InferableComponentEnhancerWithProps<TStateProps & TDispatchProps & TOwnProps, TOwnProps>

  <TStateProps = {}, no_dispatch = {}, TOwnProps = {}, TMergedProps = {}>(
      mapStateToProps: MapStateToPropsParam<TStateProps, TOwnProps, ApplicationState>,
      mapDispatchToProps: null | undefined,
      mergeProps: MergeProps<TStateProps, undefined, TOwnProps, TMergedProps>,
  ): InferableComponentEnhancerWithProps<TMergedProps, TOwnProps>

  <no_state = {}, TDispatchProps = {}, TOwnProps = {}, TMergedProps = {}>(
      mapStateToProps: null | undefined,
      mapDispatchToProps: MapDispatchToPropsParam<TDispatchProps, TOwnProps>,
      mergeProps: MergeProps<undefined, TDispatchProps, TOwnProps, TMergedProps>,
  ): InferableComponentEnhancerWithProps<TMergedProps, TOwnProps>

  <no_state = {}, no_dispatch = {}, TOwnProps = {}, TMergedProps = {}>(
      mapStateToProps: null | undefined,
      mapDispatchToProps: null | undefined,
      mergeProps: MergeProps<undefined, undefined, TOwnProps, TMergedProps>,
  ): InferableComponentEnhancerWithProps<TMergedProps, TOwnProps>

  <TStateProps = {}, TDispatchProps = {}, TOwnProps = {}, TMergedProps = {}>(
      mapStateToProps: MapStateToPropsParam<TStateProps, TOwnProps, ApplicationState>,
      mapDispatchToProps: MapDispatchToPropsParam<TDispatchProps, TOwnProps>,
      mergeProps: MergeProps<TStateProps, TDispatchProps, TOwnProps, TMergedProps>,
  ): InferableComponentEnhancerWithProps<TMergedProps, TOwnProps>

  <TStateProps = {}, no_dispatch = {}, TOwnProps = {}>(
      mapStateToProps: MapStateToPropsParam<TStateProps, TOwnProps, ApplicationState>,
      mapDispatchToProps: null | undefined,
      mergeProps: null | undefined,
      options: Options<ApplicationState, TStateProps, TOwnProps>
  ): InferableComponentEnhancerWithProps<ApplicationDispatchProp & TStateProps, TOwnProps>

  <TStateProps = {}, TDispatchProps = {}, TOwnProps = {}>(
      mapStateToProps: null | undefined,
      mapDispatchToProps: MapDispatchToPropsParam<TDispatchProps, TOwnProps>,
      mergeProps: null | undefined,
      options: Options<{}, TStateProps, TOwnProps>
  ): InferableComponentEnhancerWithProps<TDispatchProps, TOwnProps>

  <TStateProps = {}, TDispatchProps = {}, TOwnProps = {}>(
      mapStateToProps: MapStateToPropsParam<TStateProps, TOwnProps, ApplicationState>,
      mapDispatchToProps: MapDispatchToPropsParam<TDispatchProps, TOwnProps>,
      mergeProps: null | undefined,
      options: Options<ApplicationState, TStateProps, TOwnProps>
  ): InferableComponentEnhancerWithProps<TStateProps & TDispatchProps, TOwnProps>

  <TStateProps = {}, TDispatchProps = {}, TOwnProps = {}, TMergedProps = {}>(
      mapStateToProps: MapStateToPropsParam<TStateProps, TOwnProps, ApplicationState>,
      mapDispatchToProps: MapDispatchToPropsParam<TDispatchProps, TOwnProps>,
      mergeProps: MergeProps<TStateProps, TDispatchProps, TOwnProps, TMergedProps>,
      options: Options<ApplicationState, TStateProps, TOwnProps, TMergedProps>
  ): InferableComponentEnhancerWithProps<TMergedProps, TOwnProps>
}

export const applicationConnect = connect as ApplicationConnect

interface MapStateToProps<TStateProps, TOwnProps, State> {
  (state: State, ownProps: TOwnProps): TStateProps
}

interface MapStateToPropsFactory<TStateProps, TOwnProps, State> {
  (initialState: State, ownProps: TOwnProps): MapStateToProps<TStateProps, TOwnProps, State>
}

type MapStateToPropsParam<TStateProps, TOwnProps, State> = MapStateToPropsFactory<TStateProps, TOwnProps, State> | MapStateToProps<TStateProps, TOwnProps, State> | null | undefined

interface MapDispatchToPropsFunction<TDispatchProps, TOwnProps> {
  (dispatch: ApplicationDispatch, ownProps: TOwnProps): TDispatchProps
}

type MapDispatchToProps<TDispatchProps, TOwnProps> =
  MapDispatchToPropsFunction<TDispatchProps, TOwnProps> | TDispatchProps

interface MapDispatchToPropsFactory<TDispatchProps, TOwnProps> {
  (dispatch: ApplicationDispatch, ownProps: TOwnProps): MapDispatchToProps<TDispatchProps, TOwnProps>
}

type MapDispatchToPropsParam<TDispatchProps, TOwnProps> = MapDispatchToPropsFactory<TDispatchProps, TOwnProps> | MapDispatchToProps<TDispatchProps, TOwnProps>

interface MergeProps<TStateProps, TDispatchProps, TOwnProps, TMergedProps> {
  (stateProps: TStateProps, dispatchProps: TDispatchProps, ownProps: TOwnProps): TMergedProps
}
