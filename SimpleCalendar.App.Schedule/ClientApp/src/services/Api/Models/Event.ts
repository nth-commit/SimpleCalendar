export interface IEvent {
  id: string
  regionId: string
  startTime: Date
  endTime: Date
  name: string
  description: string
}

export interface IEventCreate {
  regionId: string
  startTime: Date
  endTime: Date
  name: string
  description: string
}
