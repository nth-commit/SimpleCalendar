import * as React from 'react'
import DialogTrigger from '../DialogTrigger'

export default ({ children }) => (
  <div>
    {children}
    <DialogTrigger />
  </div>
)