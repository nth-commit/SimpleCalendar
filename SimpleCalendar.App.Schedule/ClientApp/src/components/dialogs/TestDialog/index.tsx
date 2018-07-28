import * as React from 'react'
import { dialogRegistration } from 'src/services/DialogRegistration'

export const TEST_DIALOG_ID = 'test'

dialogRegistration.register(
  TEST_DIALOG_ID,
  (() => <div>Test dialog!</div>) as any
)