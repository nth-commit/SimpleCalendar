import { ROOT_REGION_ID } from 'src/constants';
import { getConfiguration } from './Configure';
import { IRegion, IRegionMembership, IRegionMembershipQuery } from './Models';
import { Auth } from '../Auth';

export class Api {

  private configuration = getConfiguration();

  getRegion(id: string): Promise<IRegion> {
    return this.fetchJson<IRegion>(this.getUrl(`regions/${id}`));
  }
  
  getRegions(parentRegionId: string): Promise<IRegion[]> {
    const url = this.getUrl('regions');

    const search = new URLSearchParams();
    if (parentRegionId !== ROOT_REGION_ID) {
      search.append('parentId', parentRegionId);
    }

    return this.fetchJson<IRegion[]>(url, search);
  }

  getMyRegionMemberships(): Promise<IRegionMembership[]> {
    return this.fetchJson<IRegionMembership[]>(this.getUrl('regionmemberships/my'));
  }

  getRegionMemberships(query: IRegionMembershipQuery): Promise<IRegionMembership[]> {
    const url = this.getUrl('regionmemberships');

    const search = new URLSearchParams();
    if (query.regionId) {
      search.append('regionId', query.regionId);
    }
    if (query.userId) {
      search.append('userId', query.userId);
    }

    return this.fetchJson<IRegionMembership[]>(url, search);
  }

  private async fetchJson<T>(url: URL, search?: URLSearchParams): Promise<T> {
    const response = await this.fetchResponse(url, search);
    const json: T = await response.json();
    return json;
  }

  private async fetchResponse(url: URL, search?: URLSearchParams): Promise<Response> {
    if (search) {
      url.search = search.toString();
    }

    const headers = new Headers();

    const auth = new Auth();
    if (auth.isAuthenticated()) {
      headers.append('Authorization', `Bearer ${localStorage.getItem('access_token')}`);
    }

    const response = await fetch(url.toString(), {
      headers
    });

    if (response.status !== 200) {
      throw new Error(await response.text());
    }

    return response;
  }

  private getUrl(path: string): URL {
    const uri = new URL(this.configuration.baseUri);
    uri.pathname += path;
    return uri;
  }
}