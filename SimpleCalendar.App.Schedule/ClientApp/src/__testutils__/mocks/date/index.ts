import * as moment from 'moment'

const OriginalDateConstructor = Date

let mockMoment: moment.Moment | null

export const mock = (m: moment.Moment) => {
  (Date as any) = MockDateConstructor
  mockMoment = m
}

export const reset = () => {
  (Date as any) = OriginalDateConstructor
}

function MockDateConstructor(...args: any[]): Date {
  if (args.length) {
    return new (OriginalDateConstructor as any)(...args)
  } else {
    return new OriginalDateConstructor((mockMoment as moment.Moment).unix() * 1000)
  }
}
MockDateConstructor.prototype = Date.prototype