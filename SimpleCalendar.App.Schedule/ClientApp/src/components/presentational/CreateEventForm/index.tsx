import * as React from 'react'
import TextField, { TextFieldProps } from '@material-ui/core/TextField'
import Grid from '@material-ui/core/Grid'
import { IEventCreate } from 'src/services/Api'
import { ValidationService, ValidationResult, validators } from 'src/services/Validation'

type Nullable<T> = { [P in keyof T]: T[P] | null }

type CreateEventFormModelProperties = Exclude<keyof IEventCreate, 'regionId'>
type CreateEventFormModel = Nullable<Pick<IEventCreate, CreateEventFormModelProperties>>
type CreateEventFormModelPropertyValues = string | Date | null

interface CreateEventFormFieldState<P> {
  value: string
  valueParsed: P
  validationResult: ValidationResult
  isTouched: boolean
}

type CreateEventFormState = {
  [P in keyof CreateEventFormModel]: CreateEventFormFieldState<CreateEventFormModel[P]>
}

interface CreateEventFormProps {
  onUpdate(event: CreateEventFormModel | null): void
  isSubmitted: boolean 
}

const validationService = new ValidationService<CreateEventFormModel>({
  name: validators.required,
  startTime: [validators.required, validators.future],
  endTime: [validators.required, validators.future, validators.after('startTime')] 
})

export default class CreateEventForm extends React.PureComponent<CreateEventFormProps> {

  private labelsByKey = {
    'name': 'Name',
    'startTime': 'Start',
    'endTime': 'End',
    'description': 'Description'
  }

  state: CreateEventFormState = {
    name: this.initializeField('name'),
    description: this.initializeField('description'),
    startTime: this.initializeField('startTime'),
    endTime: this.initializeField('endTime')
  }

  componentDidUpdate() {
    const { state, props } = this
    const keys = Object.keys(state) as Array<keyof CreateEventFormModel>

    const isValid = keys.every(k => state[k].validationResult.success)
    if (isValid) {
      const result = {}
      keys.forEach(k => result[k] = state[k].valueParsed)
      props.onUpdate(result as CreateEventFormModel)
    } else {
      props.onUpdate(null)
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

  private renderTextField<K extends CreateEventFormModelProperties>(key: K, extraProps?: Partial<TextFieldProps>) {
    const fieldState = this.state[key]
    const isTouched = this.props.isSubmitted || fieldState.isTouched

    return <TextField
      id={key}
      label={this.labelsByKey[key]}
      value={fieldState.value}
      onBlur={this.createOnBlurHandler(key)}
      onChange={this.createOnChangeHandler(key)}
      error={isTouched && !fieldState.validationResult.success}
      helperText={isTouched && this.getValidationMessage(key, fieldState.validationResult)}
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

  private createOnChangeHandler<K extends CreateEventFormModelProperties>(key: K) {
    return (ev: React.ChangeEvent<HTMLInputElement>) => {
      const { value } = ev.currentTarget
      const valueParsed = this.parseValue(key, value)
      this.setFieldState(key, {
        value,
        valueParsed,
        validationResult: validationService.validate(key, valueParsed, this.getModel())
      })
    }
  }

  private createOnBlurHandler<K extends CreateEventFormModelProperties>(key: K) {
    return () => {
      this.setFieldState(key, {
        isTouched: true
      })
    }
  }

  private setFieldState<K extends CreateEventFormModelProperties>(key: K, partialFieldState: Partial<CreateEventFormFieldState<any>>) {
    const x = this.state[key]
    this.setState({
      [key]: { ...(x as any), ...partialFieldState }
    })
  }

  private getModel(): CreateEventFormModel | null {
    const { state } = this
    if (!state) {
      return null
    }

    const keys = Object.keys(state) as Array<keyof CreateEventFormModel>

    const result = {}
    keys.forEach(k => result[k] = state[k].valueParsed)
    return result as CreateEventFormModel
  }

  private parseValue<K extends CreateEventFormModelProperties>(key: K, value: string): CreateEventFormModelPropertyValues {
    switch (key) {
      case 'name':
      case 'description':
        return value ? value : null
      case 'startTime':
      case 'endTime':
        return value ? new Date(value) : null
      default: throw new Error('Unhandled')
    }
  }

  private initializeField(key: 'startTime' | 'endTime'): CreateEventFormFieldState<Date | null>
  private initializeField(key: 'name' | 'description'): CreateEventFormFieldState<string | null>
  private initializeField<K extends CreateEventFormModelProperties>(key: K): CreateEventFormFieldState<Date | string | null> {
    const value = ''
    const valueParsed = this.parseValue(key, '')
    return {
      value,
      valueParsed,
      isTouched: false,
      validationResult: validationService.validate(key, valueParsed, this.getModel())
    }
  }

  private getValidationMessage(key: string, validationResult: ValidationResult): string | undefined {
    if (validationResult.success) {
      return
    }
    return validationResult.errors[0](key, this.labelsByKey)
  }
}
