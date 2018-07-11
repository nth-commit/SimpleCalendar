import * as Enzyme from 'enzyme';
import * as ReactSixteenAdapter from 'enzyme-adapter-react-16';
import './extensions';

const adapter = new ReactSixteenAdapter();
Enzyme.configure({ adapter });