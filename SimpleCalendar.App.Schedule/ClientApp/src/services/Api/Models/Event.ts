export interface IEvent {
  id: string
}

export interface IEventCreate {
  regionId: string
  startTime: Date
  endTime: Date
  name: string
  description: string
}
