import { ApplicationStore, ApplicationEffect } from 'src/store'
import setRegionEffect from 'src/effects/setRegionEffect'
import setAuthorizedEffect from 'src/effects/setAuthorizedEffect'
import setUnauthorizedEffect from 'src/effects/setUnauthorizedEffect'

const effects: ApplicationEffect[] = [
  setRegionEffect,
  setAuthorizedEffect,
  setUnauthorizedEffect
]

export default function registerEffects(store: ApplicationStore) {
  effects.forEach(effect => store.subscribe(() => {
    effect(
      (action: any) => store.dispatch(action),
      () => store.getState()
    )
  }))
}