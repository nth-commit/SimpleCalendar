import { default as AuthHelper } from './AuthHelper';
import { default as AuthHandlerFactory } from './AuthHandlerFactory';

export default new AuthHelper(new AuthHandlerFactory);
