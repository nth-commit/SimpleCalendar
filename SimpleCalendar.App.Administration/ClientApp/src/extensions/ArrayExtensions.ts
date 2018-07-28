// tslint:disable:only-arrow-functions

interface Array<T> {
  difference(other: T[]): T[]
  selectMany<V>(selector: (item: T) => V[]): V[]
  toObject<V = T>(keySelector: (item: T) => string, valueSelector?: (item: T) => V): { [key: string]: V }
}

Array.prototype.difference = function<T>(other: T[]): T[] {
  return Array.from(new Set(this).difference(new Set(other)))
}

Array.prototype.selectMany = function<T, V>(selector: (item: T) => V[]): V[] {
  return this.reduce((acc, curr) => [...acc, ...selector(curr)], [])
}

Array.prototype.toObject = function<T, V = T>(
  keySelector: (item: T) => string,
  valueSelector?: (item: T) => V): { [key: string]: V } {
    const result = {}
    this.forEach(item => result[keySelector(item)] = valueSelector ? valueSelector(item) : item)
    return result
}

interface ArrayConstructor {
  range(count: number, start?: number): number[]
}

Array.range = function(count: number, start: number = 0): number[] {
  return Array.from(new Array(count).keys()).map(x => x + start)
}