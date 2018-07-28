// tslint:disable:ban-types
interface ObjectConstructor {
  filter<T, P = any>(obj: T, pred: (key: string, propValue?: P) => boolean): Partial<T>
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