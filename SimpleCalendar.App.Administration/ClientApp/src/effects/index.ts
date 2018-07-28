import { ApplicationStore, ApplicationEffect } from 'src/store'
import setRegionEffect from 'src/effects/setRegionEffect'

const effects: ApplicationEffect[] = [
  setRegionEffect
]

export default function registerEffects(store: ApplicationStore) {
  effects.forEach(effect => store.subscribe(() => {
    effect(
      (action: any) => store.dispatch(action),
      () => store.getState()
    )
  }))
}