// tslint:disable:only-arrow-functions

interface Array<T> {
  difference(other: T[]): T[];
  selectMany<V>(selector: (item: T) => V[]): V[];
}

Array.prototype.difference = function<T>(other: T[]): T[] {
  return Array.from(new Set(this).difference(new Set(other)));
}

Array.prototype.selectMany = function<T, V>(selector: (item: T) => V[]): V[] {
  return this.reduce((acc, curr) => [...acc, ...selector(curr)], []);
}