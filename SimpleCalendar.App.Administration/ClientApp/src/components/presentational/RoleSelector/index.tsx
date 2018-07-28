import * as React from 'react'
import MenuItem from '@material-ui/core/MenuItem'
import FormControl from '@material-ui/core/FormControl'
import Select from '@material-ui/core/Select'
import { IRegionRole } from 'src/services/Api'

export interface RoleSelectorProps {
  roleSelected(roleId: string): void
  roles: IRegionRole[]
}

class RoleSelector extends React.PureComponent<RoleSelectorProps> {
  state = {
    roleId: ''
  }

  handleChange = (ev) => {
    const newRoleId = ev.target.value
    if (newRoleId !== this.state.roleId) {
      this.setState({ roleId: newRoleId })
      this.props.roleSelected(newRoleId)
    }
  }

  render() {
    const { roles } = this.props
    return (
      <FormControl style={{ width: '100%' }}>
        <Select
          value={this.state.roleId}
          onChange={this.handleChange}
          displayEmpty={true}
          name="role"
        >
          <MenuItem value=''>
            <em>Select a role</em>
          </MenuItem>
          {roles.map(r => (
            <MenuItem key={r.id} value={r.id}>{r.name}</MenuItem>
          ))}
        </Select>
      </FormControl>
    )
  }
}

export default RoleSelector