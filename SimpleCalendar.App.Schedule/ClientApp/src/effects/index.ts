import { ApplicationStore, ApplicationEffect } from 'src/store'

const effects: ApplicationEffect[] = []

export default function registerEffects(store: ApplicationStore) {
  effects.forEach(effect => store.subscribe(() => {
    effect(
      (action: any) => store.dispatch(action),
      () => store.getState()
    )
  }))
}