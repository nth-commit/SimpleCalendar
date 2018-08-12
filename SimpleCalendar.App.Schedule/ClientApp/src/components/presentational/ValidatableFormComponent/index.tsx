import * as React from 'react'
import { ValidationResult, ValidatorConfig, ValidationService } from 'src/services/Validation'

export interface ValidatableFormComponentProps<TModel> {
  onValidChange?: (model: TModel | null) => void
  onChange?: (model: TModel) => void
  forceTouched?: boolean
}

export interface ModelPropertyState<P> {
  value: string
  valueParsed: P
  validationResult: ValidationResult
  isTouched: boolean
}

export type ValidatableFormComponentState<TModel> = {
  [P in keyof TModel]: ModelPropertyState<TModel[P]>
}

export type ModelKey<TModel> = Array<keyof TModel>

export default abstract class ValidatableFormComponent<TModel> extends React.PureComponent<ValidatableFormComponentProps<TModel>, ValidatableFormComponentState<TModel>> {
  protected abstract fields: string[]
  protected abstract labelsByField: { [field: string]: string }
  protected abstract validatorConfig: ValidatorConfig<TModel>
  protected abstract parseValue(field: string, value: string): any
  
  private validationService: ValidationService<TModel> | null = null

  componentWillMount() {
    const result = {}
    this.fields.forEach(field => result[field] = this.initializeField(field))
    this.setState(result)

    this.validationService = new ValidationService<TModel>(this.validatorConfig)
    setTimeout(() => {
      this.fields.forEach(field => {
        const fieldState = this.state[field]
        this.setFieldState(field, {
          validationResult: this.validate(field, fieldState.valueParsed)
        })
      })
    })
  }

  componentDidUpdate() {
    const { state, props } = this

    const fields = Object.keys(state)
    const result = {}
    fields.forEach(k => result[k] = state[k].valueParsed)
    
    if (props.onChange) {
      props.onChange(result as TModel)
    }

    if (props.onValidChange) {
      const isValid = fields.every(k => state[k].validationResult.success)
      props.onValidChange(isValid ? result as TModel : null)
    }
  }

  getValidationMessage(field: string): string | undefined {
    if (this.isValid(field)) {
      return
    }
    return this.state[field].validationResult.errors[0](field, this.labelsByField)
  }

  setFieldTouched(field: string) {
    this.setFieldState(field, {
      isTouched: true
    })
  }

  setFieldValue(field: string, value: string) {
    const valueParsed = this.parseValue(field, value)
    this.setFieldState(field, {
      value,
      valueParsed,
      validationResult: this.validate(field, valueParsed)
    })
  }

  isTouched(field: string): boolean {
    const fieldState = this.state[field]
    return this.props.forceTouched || fieldState.isTouched
  }

  isValid(field: string): boolean  {
    return !this.isTouched(field) || this.state[field].validationResult.success
  }

  private setFieldState(field: string, partialFieldState: Partial<ModelPropertyState<TModel>>) {
    this.setState({
      [field]: { ...this.state[field], ...partialFieldState }
    } as any)
  }

  private initializeField(field: string) {
    const value = ''
    const valueParsed = this.parseValue(field, '')
    return {
      value,
      valueParsed,
      isTouched: false,
      validationResult: this.validate(field, valueParsed)
    }
  }

  private getModel(): TModel | null {
    const { state } = this
    if (!state) {
      return null
    }

    const fields = Object.keys(state)

    const result = {}
    fields.forEach(k => result[k] = state[k].valueParsed)
    return result as TModel
  }

  private validate(field: string, valueParsed: any): ValidationResult {
    return this.validationService ?
      this.validationService.validate(field, valueParsed, this.getModel()) :
      { success: true, errors: [] }
  }
}
