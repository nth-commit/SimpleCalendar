export interface IAuthResult {
  bearerToken: string;
  expiresIn: number;
  [key: string]: any;
}
