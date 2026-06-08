import { FormFieldT } from '@/common/types/dom';
import { Nullable, OrNone } from '@/common/types/general';
import { UseFormFieldDir } from '@/core/directives/use_form_field_dir';
import { LibLog } from '@/core/lib/log';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  input,
  InputSignal,
  OnInit,
  Signal,
} from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-img-input',
  imports: [ReactiveFormsModule],
  templateUrl: './img-input.html',
  styleUrl: './img-input.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ImgInput extends UseFormFieldDir implements OnInit {
  public readonly field: InputSignal<FormFieldT> = input.required();

  private isFile(val: unknown): val is File {
    return val instanceof File;
  }

  public readonly srcImg: Signal<Nullable<string>> = computed(() => {
    const val = this.val?.();

    if (typeof val === 'string') {
      return val;
    }

    if (this.isFile(val)) {
      return URL.createObjectURL(val);
    }

    return null;
  });

  public handleFileChange(e: Event): void {
    const input = e.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;

    if (!file) return;

    const c = this.ctrl();
    c.markAsDirty();
    c.markAsTouched();
    c.setValue(file);

    console.log(file);
    LibLog.main('banana', this.val?.());
  }

  ngOnInit(): void {
    this.setupField();
  }
}
