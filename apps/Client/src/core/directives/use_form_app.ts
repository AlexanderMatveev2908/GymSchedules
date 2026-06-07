import { Directive, Signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup } from '@angular/forms';
import { startWith } from 'rxjs';
import { UseInjCtxHk } from '../hooks/use_inj_ctx';

@Directive()
export abstract class UseFormAppDir extends UseInjCtxHk {
  public abstract form: FormGroup;

  public data!: Signal<unknown>;

  protected initDataSignal(): void {
    this.usePlatform.inGlobalCtx(() => {
      this.data = toSignal(this.form.valueChanges.pipe(startWith(this.form.getRawValue())), {
        initialValue: this.form.getRawValue(),
      });
    });
  }

  public getCtrl(name: string): FormControl {
    return this.form.get(name) as FormControl;
  }
}
