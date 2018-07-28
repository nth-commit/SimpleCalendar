import * as React from 'react'
import TextField from '@material-ui/core/TextField'

export interface UserAutocompleteProps {
  userSelected(email: string | null, userExists: boolean): void
}

const UserAutocomplete = ({ userSelected }: UserAutocompleteProps) => {
  let email = ''
  let lastSelectedEmail = ''
  let isValid: boolean = false

  const onTextFieldChange = (event) => {
    const v = event.target.value
    isValid = !!v
    email = isValid ? v : ''

    if (email && email !== lastSelectedEmail) {
      lastSelectedEmail = email
      userSelected(email, false)
    }
  }

  return <TextField
    style={{ width: '100%' }}
    label='Email'
    error={isValid}
    onChange={onTextFieldChange}  />
}

export default UserAutocomplete