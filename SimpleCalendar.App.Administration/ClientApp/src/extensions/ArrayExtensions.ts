interface Array<T> {
  difference(other: T[]): T[];
}

// tslint:disable-next-line:only-arrow-functions
Array.prototype.difference = function <T>(other: T[]): T[] {
  return Array.from(new Set(this).difference(new Set(other)));
}