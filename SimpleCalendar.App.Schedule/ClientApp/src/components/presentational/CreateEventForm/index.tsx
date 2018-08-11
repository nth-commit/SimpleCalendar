import * as React from 'react'
import TextField, { TextFieldProps } from '@material-ui/core/TextField'
import Grid from '@material-ui/core/Grid'
import { IEventCreate } from 'src/services/Api'

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

type ModelValidators = {
  [P in keyof CreateEventFormModel]: IValidator<CreateEventFormModelPropertyValues> | Array<IValidator<CreateEventFormModelPropertyValues>>
}

interface IValidator<T = any> {
  name: string
  format(label: string): string
  validate(value: T): boolean
}

interface Validators {
  [key: string]: IValidator<any>
}

const validators: Validators = {
  required: {
    name: 'required',
    validate: value => !!value,
    format: label => `${label} is required`
  },
  future: {
    name: 'future',
    validate: (value: Date | null) => !!value && new Date().getTime() < value.getTime(),
    format: label => `${label} must be a date in the future`
  }
}

const modelValidators: Partial<ModelValidators> = {
  name: validators.required,
  startTime: [validators.required, validators.future]
}

interface ValidationResult {
  success: boolean
  failedValidators: string[]
}

const validate = (key: keyof CreateEventFormModel, value: CreateEventFormModelPropertyValues): ValidationResult => {
  const validatorForProp = modelValidators[key]

  const validatorsForProp = 
    !validatorForProp ? [] :
    Array.isArray(validatorForProp) ? validatorForProp :
    [validatorForProp]

  const failedValidators = validatorsForProp.filter(v => !v.validate(value))
  return {
    success: failedValidators.length === 0,
    failedValidators: failedValidators.map(v => v.name)
  }
}

interface CreateEventFormProps {
  onUpdate(event: CreateEventFormModel | null): void
  isSubmitted: boolean 
}

export default class CreateEventForm extends React.PureComponent<CreateEventFormProps> {

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
      keys.forEach(k => result[k] = state[k].value)
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
            {this.renderTextField('name', 'Name')}
          </Grid>
          <Grid item={true} sm={12}>
            <Grid container={true} spacing={32}>
              <Grid item={true} sm={6}>
                {this.renderDateTimePicker('startTime', 'Start')}
              </Grid>
              <Grid item={true} sm={6}>
                {this.renderDateTimePicker('endTime', 'End')}
              </Grid>
            </Grid>
          </Grid>
          <Grid item={true} sm={12}>
            {this.renderTextField('description', 'Description', { multiline: true, rows: 8 })}
          </Grid>
        </Grid>
      </form>
    )
  }

  private renderTextField<K extends CreateEventFormModelProperties>(key: K, label: string, extraProps?: Partial<TextFieldProps>) {
    const fieldState = this.state[key]
    const isTouched = this.props.isSubmitted || fieldState.isTouched

    return <TextField
      id={key}
      label={label}
      value={fieldState.value}
      onBlur={this.createOnBlurHandler(key)}
      onChange={this.createOnChangeHandler(key)}
      error={isTouched && !fieldState.validationResult.success}
      helperText={isTouched && this.getValidationMessage(label, fieldState.validationResult)}
      margin="normal"
      fullWidth={true}
      {...(extraProps || {})}
    />
  }

  private renderDateTimePicker<K extends CreateEventFormModelProperties>(key: K, label: string, extraProps?: Partial<TextFieldProps>) {
    return this.renderTextField(key, label, {
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
        validationResult: validate(key, valueParsed)
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
      validationResult: validate(key, valueParsed)
    }
  }

  private getValidationMessage(label: string, validationResult: ValidationResult): string | undefined {
    if (validationResult.success) {
      return
    }
    return validators[validationResult.failedValidators[0]].format(label)
  }
}
