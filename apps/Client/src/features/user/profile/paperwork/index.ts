import { FormControl, FormGroup } from '@angular/forms';
import z from 'zod';

export class ProfileFormMng {
  public static readonly schema = z.object({
    imgFile: z.union([
      z.url(),
      z.instanceof(File).refine((file) => file.type.startsWith('image/'), {
        message: 'File must be an image',
      }),
    ]),
    firstName: z.string().min(3),
    lastName: z.string().min(3),
  });

  public static form(): FormGroup {
    return new FormGroup({
      imgFile: new FormControl('', { nonNullable: false }),
      firstName: new FormControl('', { nonNullable: true }),
      lastName: new FormControl('', { nonNullable: true }),
    });
  }
}

export type ProfileFormT = z.infer<typeof ProfileFormMng.schema>;
