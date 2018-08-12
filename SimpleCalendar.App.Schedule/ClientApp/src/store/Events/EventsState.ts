import { IEvent } from "src/services/Api"

export interface EventsState {
  isLoading: boolean
  events: IEvent[] | null
  error: any
}