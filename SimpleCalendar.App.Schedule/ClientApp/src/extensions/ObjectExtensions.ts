// tslint:disable:ban-types
interface ObjectConstructor {
  filter<T, P = any>(obj: T, pred: (key: string, propValue?: P) => boolean): Partial<T>
  map<T, U, V>(obj: T, selector: (item: U, key: string) => V): V[]
}

Object.filter = function filter<T, P = any>(obj: T, pred: (key: string, propValue?: P) => boolean): Partial<T> {
  const result = {} as Partial<T>
  Object
    .keys(obj)
    .filter(k => pred(k, obj[k]))
    .forEach(k => {
      result[k] = obj[k]
    })
  return result
}

Object.map = function map<T, U, V>(obj: T, selector: (item: U, key: string) => V): V[] {
  return Object
    .keys(obj)
    .map(k => selector(obj[k], k))
}