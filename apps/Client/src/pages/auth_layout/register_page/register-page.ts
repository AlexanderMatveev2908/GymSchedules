import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { TitlePage } from '@/common/components/general/title_page/title-page';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { RegisterFormMng } from '@/features/auth/register/paperwork/register_form_mng';
import { UseFormAppDir } from '@/core/directives/use_form_app';
import { TxtInput } from '@/common/components/forms/txt_input/txt-input';
import { RegisterUiFct } from '@/features/auth/register/paperwork/register_ui_fct';
import { LibRootForm } from '@/core/lib/forms/root_form';
import { LibLog } from '@/core/lib/log';
import { PwdInput } from '@/common/components/forms/pwd_input/pwd-input';
import { RadioInput } from '@/common/components/forms/radio_input/radio-input';

@Component({
  selector: 'app-register-page',
  imports: [TitlePage, TxtInput, ReactiveFormsModule, PwdInput, RadioInput],
  templateUrl: './register-page.html',
  styleUrl: './register-page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterPage extends UseFormAppDir implements OnInit {
  public readonly form: FormGroup = RegisterFormMng.form();
  public readonly RegisterUiFct = RegisterUiFct;
  public readonly schema = RegisterFormMng.schema;

  ngOnInit(): void {
    this.setupForm();
  }

  public handleSubmit(): void {
    LibRootForm.handleSubmit({
      form: this.form,
      schema: RegisterFormMng.schema,
      onValid: (data) => LibLog.main('success', data),
      onInvalid: (errs) => LibLog.main('issues', errs),
    });
  }
}
