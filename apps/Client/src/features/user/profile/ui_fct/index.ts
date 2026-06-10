import { FormFieldT } from '@/common/types/dom';

export class ProfileUiFct {
  public static readonly imgFile: FormFieldT = {
    name: 'imgFile',
    label: 'Profile Picture',
    type: 'file',
  };

  public static readonly firstName: FormFieldT = {
    name: 'firstName',
    label: 'First Name',
    type: 'text',
  };

  public static readonly lastName: FormFieldT = {
    name: 'lastName',
    label: 'Last Name',
    type: 'text',
  };
}
