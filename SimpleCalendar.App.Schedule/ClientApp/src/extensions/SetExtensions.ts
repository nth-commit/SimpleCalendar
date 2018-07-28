interface Set<T> {
  difference(other: Set<T>): Set<T>
}

interface SetConstructor {
  fromArray<T, V>(array: T[], selector: ((item: T) => V)): Set<V>
}

Set.prototype.difference = function difference<T>(other: Set<T>): Set<T> {
  return new Set(Array.from(this).filter(x => !other.has(x)))
}

Set.fromArray = function fromArray<T, V>(array: T[], selector: ((item: T) => V)): Set<V> {
  const result = new Set<V>()
  array.forEach(v => result.add(selector(v)))
  return result
}