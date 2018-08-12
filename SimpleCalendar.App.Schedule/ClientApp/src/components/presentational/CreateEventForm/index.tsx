import * as React from 'react'
import TextField, { TextFieldProps } from '@material-ui/core/TextField'
import Grid from '@material-ui/core/Grid'
import { IEventCreate } from 'src/services/Api'
import { validators } from 'src/services/Validation'
import ValidatableFormComponent from '../ValidatableFormComponent'

type Nullable<T> = { [P in keyof T]: T[P] | null }
type CreateEventFormModelProperties = Exclude<keyof IEventCreate, 'regionId'>
type CreateEventFormModel = Nullable<Pick<IEventCreate, CreateEventFormModelProperties>>

export default class CreateEventForm extends ValidatableFormComponent<CreateEventFormModel> {
  protected fields = ['startTime', 'endTime', 'name', 'description']
  protected labelsByField: { [field: string]: string; } = {
    'name': 'Name',
    'startTime': 'Start',
    'endTime': 'End',
    'description': 'Description'
  }
  protected validatorConfig = {
    name: validators.required,
    startTime: [validators.required, validators.future],
    endTime: [validators.required, validators.future, validators.after('startTime')],
    description: [validators.required]
  }

  protected parseValue(field: string, value: string) {
    switch (field) {
      case 'name':
      case 'description':
        return value ? value : null
      case 'startTime':
      case 'endTime':
        return value ? new Date(value) : null
      default: throw new Error('Unhandled')
    }
  }

  render() {
    return (
      <form noValidate={true} autoComplete="off">
        <Grid container={true}>
          <Grid item={true} sm={12}>
            {this.renderTextField('name')}
          </Grid>
          <Grid item={true} sm={12}>
            <Grid container={true} spacing={32}>
              <Grid item={true} sm={6}>
                {this.renderDateTimePicker('startTime')}
              </Grid>
              <Grid item={true} sm={6}>
                {this.renderDateTimePicker('endTime')}
              </Grid>
            </Grid>
          </Grid>
          <Grid item={true} sm={12}>
            {this.renderTextField('description', { multiline: true, rows: 8 })}
          </Grid>
        </Grid>
      </form>
    )
  }

  private renderTextField<K extends CreateEventFormModelProperties>(field: K, extraProps?: Partial<TextFieldProps>) {
    const fieldState = this.state[field]
    return <TextField
      id={field}
      label={this.labelsByField[field]}
      value={fieldState.value}
      onBlur={this.createOnBlurHandler(field)}
      onChange={this.createOnChangeHandler(field)}
      error={!this.isValid(field)}
      helperText={this.getValidationMessage(field)}
      margin="normal"
      fullWidth={true}
      {...(extraProps || {})}
    />
  }

  private renderDateTimePicker<K extends CreateEventFormModelProperties>(key: K, extraProps?: Partial<TextFieldProps>) {
    return this.renderTextField(key, {
      type: 'datetime-local',
      InputLabelProps: {
        shrink: true
      }
    })
  }

  private createOnChangeHandler<K extends CreateEventFormModelProperties>(field: K) {
    return (ev: React.ChangeEvent<HTMLInputElement>) => {
      this.setFieldValue(field, ev.currentTarget.value)
    }
  }

  private createOnBlurHandler<K extends CreateEventFormModelProperties>(key: K) {
    return () => {
      this.setFieldTouched(key)
    }
  }
}
