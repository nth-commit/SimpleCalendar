import { getConfiguration } from './Configure';
import { IRegion, IRegionMembership } from './Models';

export class Api {

  private configuration = getConfiguration();

  async getRegion(id: string): Promise<IRegion> {
    const response = await fetch(this.getUri(`regions/${id}`));
    const region: IRegion = await response.json()
    return region;
  }
  
  async getRegions(parentRegionId: string): Promise<IRegion[]> {
    return Promise.resolve([]);
  }

  async getRegionMemberships(regionId: string): Promise<IRegionMembership[]> {
    return Promise.resolve([]);
  }

  private getUri(path: string): string {
    const uri = new URL(this.configuration.baseUri);
    uri.pathname += path;
    return uri.toString();
  }
}