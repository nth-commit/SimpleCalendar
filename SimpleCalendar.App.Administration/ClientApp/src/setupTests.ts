import * as Enzyme from 'enzyme';
import * as ReactSixteenAdapter from 'enzyme-adapter-react-16';
import './extensions';

const adapter = new ReactSixteenAdapter();
Enzyme.configure({ adapter });

class LocalStorageMock {
  private store = {};

  clear() {
    this.store = {};
  }

  getItem(key) {
    return this.store[key] || null;
  }

  setItem(key, value) {
    this.store[key] = value.toString();
  }

  removeItem(key) {
    delete this.store[key];
  }
};

(global as any).localStorage = new LocalStorageMock;