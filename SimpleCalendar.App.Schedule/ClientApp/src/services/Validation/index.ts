

export interface ValidationResult {
  success: boolean
  errors: FormatValidationErrorFn[] 
}

export type ValidatorConfig<T> = {
  [P in keyof T]: IValidator | IValidator[]
}

export class ValidationService<T> {

  constructor(private config: ValidatorConfig<T>) { }

  validate(key: string, value: any, model: T | null): ValidationResult {
    const keyConfig = this.config[key]

    const keyValidators = 
      !keyConfig ? [] :
      Array.isArray(keyConfig) ? keyConfig :
      [keyConfig]

    const failedValidators = keyValidators.filter(v => !v.validate(value, model))
    return {
      success: failedValidators.length === 0,
      errors: failedValidators.map(v => v.format)
    }
  }
}

type FormatValidationErrorFn = (key: string, labels: { [key: string]: string }) => string

interface IValidator {
  name: string
  format: FormatValidationErrorFn
  validate(value: any, model: any | null): boolean
}

export class Validators {

  readonly required: IValidator = {
    name: 'required',
    validate: value => !!value,
    format: (key, labels) => `${labels[key]} is required`
  }

  readonly future: IValidator = {
    name: 'future',
    validate: (value: any) => {
      if (!value) {
        return true
      }

      if (!(value instanceof Date)) {
        throw new Error('Expected value to be Date')
      }

      return new Date().getTime() < value.getTime() 
    },
    format: (key, labels) => `${labels[key]} must be a date in the future`
  }

  after = (afterKey: string): IValidator => ({
    name: 'after',
    validate: (value: Date | null, model: any) => {
      if (!model) {
        return true
      }

      const before = model[afterKey]
      if (!before) {
        return true
      }

      if (!(before instanceof Date)) {
        throw new Error('Expected date')
      }

      if (!value) {
        return true
      }

      return value > before
    },
    format: (key, labels) => `${labels[key]} must be after ${labels[afterKey]}`
  })
}

export const validators = new Validators()
