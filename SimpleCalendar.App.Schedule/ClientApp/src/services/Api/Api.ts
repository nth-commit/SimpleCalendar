import { ROOT_REGION_ID } from 'src/constants'
import { getConfiguration } from './Configure'
import { IRegion, IRegionMembership, IRegionMembershipQuery, IRegionRole, IRegionMembershipCreate, IUser } from './Models'
import { IEvent, IEventResponse, IEventCreate } from './Models/Event'

export class Api {

  private configuration = getConfiguration()

  constructor(private accessToken: string | null) { }

  getRegion(id: string): Promise<IRegion> {
    return this.getJson<IRegion>(this.getUrl(`regions/${id}`))
  }
  
  getRegions(parentRegionId: string): Promise<IRegion[]> {
    const url = this.getUrl('regions')

    const search = new URLSearchParams()
    if (parentRegionId !== ROOT_REGION_ID) {
      search.append('parentId', parentRegionId)
    }

    return this.getJson<IRegion[]>(url, search)
  }

  getMyRegionMemberships(): Promise<IRegionMembership[]> {
    return this.getJson<IRegionMembership[]>(this.getUrl('regionmemberships/my'))
  }

  getRegionMemberships(query: IRegionMembershipQuery): Promise<IRegionMembership[]> {
    const url = this.getUrl('regionmemberships')

    const search = new URLSearchParams()
    if (query.regionId) {
      search.append('regionId', query.regionId)
    }
    if (query.userId) {
      search.append('userId', query.userId)
    }

    return this.getJson<IRegionMembership[]>(url, search)
  }

  getMyUser(): Promise<IUser> {
    return this.getJson<IUser>(this.getUrl('users/my'))
  }

  getRegionRoles(): Promise<IRegionRole[]> {
    return this.getJson<IRegionRole[]>(this.getUrl('regionroles'))
  }

  async queryEventsToday(regionId: string, timezone: string): Promise<IEvent[]> {
    const search = new URLSearchParams()
    search.append('regionId', regionId)
    search.append('timezone', timezone)

    const response = await this.get(this.getUrl('events/today'), search)
    const json: IEventResponse[] = await response.json()

    return json.map(this.mapEventResponseToEvent)
  }

  async queryMyEvents(regionId: string): Promise<IEvent[]> {
    const search = new URLSearchParams()
    search.append('regionId', regionId)

    const response = await this.get(this.getUrl('events/my'), search)
    const json: IEventResponse[] = await response.json()

    return json.map(this.mapEventResponseToEvent)
  }

  async createEvent(create: IEventCreate): Promise<IEvent> {
    const response = await this.post(this.getUrl('events'), create)
    const json: IEventResponse = await response.json()
    return this.mapEventResponseToEvent(json)
  }

  async createRegionMembership(create: IRegionMembershipCreate): Promise<IRegionMembership> {
    const response = await this.post(this.getUrl('regionmemberships'), create)
    return await response.json()
  }

  deleteRegionMembersip(regionMembershipId: string): Promise<void> {
    return this.delete(this.getUrl(`regionmemberships/${regionMembershipId}`)).then(() => {})
  }

  private async getJson<T>(url: URL, search?: URLSearchParams): Promise<T> {
    const response = await this.get(url, search)
    const json: T = await response.json()
    return json
  }

  private async get(url: URL, search?: URLSearchParams): Promise<Response> {
    if (search) {
      url.search = search.toString()
    }

    const headers = new Headers()

    if (this.accessToken) {
      headers.append('Authorization', `Bearer ${this.accessToken}`)
    }

    const response = await fetch(url.toString(), {
      headers
    })

    if (response.status !== 200) {
      throw new Error(await response.text())
    }

    return response
  }

  private async post(url: URL, body?: any): Promise<Response> {
    const headers = new Headers()

    if (this.accessToken) {
      headers.append('Authorization', `Bearer ${this.accessToken}`)
    }

    headers.append('Content-Type', 'application/json')

    const response = await fetch(url.toString(), {
      headers,
      method: 'POST',
      body: body ? JSON.stringify(body) : undefined
    })

    return response
  }

  private async delete(url: URL): Promise<Response> {
    const headers = new Headers()

    if (this.accessToken) {
      headers.append('Authorization', `Bearer ${this.accessToken}`)
    }

    const response = await fetch(url.toString(), {
      headers,
      method: 'DELETE'
    })

    return response
  }

  private getUrl(path: string): URL {
    const uri = new URL(this.configuration.baseUri)
    uri.pathname += path
    return uri
  }

  private mapEventResponseToEvent(eventResponse: IEventResponse): IEvent {
    return {
      ...eventResponse,
      startTime: new Date(eventResponse.startTime),
      endTime: new Date(eventResponse.endTime)
    }
  }
}