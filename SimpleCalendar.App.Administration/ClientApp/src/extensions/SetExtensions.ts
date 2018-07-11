interface Set<T> {
  difference(other: Set<T>): Set<T>;
}

Set.prototype.difference = function difference<T>(other: Set<T>): Set<T> {
  return new Set(Array.from(this).filter(x => !other.has(x)));
}