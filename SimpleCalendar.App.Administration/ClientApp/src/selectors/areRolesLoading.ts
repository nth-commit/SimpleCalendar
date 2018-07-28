import { ApplicationState } from 'src/store'


export default function areRolesLoading(state: ApplicationState): boolean {
  return state.roles.loading
}