import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { TitlePage } from '@/common/components/general/title_page/title-page';
import { ThumbInput } from '@/common/components/forms/thumb_input/thumb-input';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ProfileFormMng } from '@/features/user/profile/paperwork';
import { ProfileUiFct } from '@/features/user/profile/ui_fct';
import { UseFormAppDir } from '@/core/directives/use_form_app';
import { UseApiTrackerHk } from '@/core/hooks/use_api_tracker';
import { TxtInput } from '@/common/components/forms/txt_input/txt-input';
import { LibRootForm } from '@/core/lib/forms/root_form';
import { LibLog } from '@/core/lib/log';

@Component({
  selector: 'app-profile-page',
  imports: [TitlePage, ThumbInput, ReactiveFormsModule, TxtInput],
  providers: [UseApiTrackerHk],
  templateUrl: './profile-page.html',
  styleUrl: './profile-page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfilePage extends UseFormAppDir {
  public readonly form: FormGroup = ProfileFormMng.form();
  public readonly ProfileUiFct = ProfileUiFct;
  public readonly schema = ProfileFormMng.schema;

  public readonly apiTracker: UseApiTrackerHk = inject(UseApiTrackerHk);

  ngOnInit(): void {
    this.setupForm();
  }

  public handleSubmit(): void {
    LibRootForm.handleSubmit({
      form: this.form,
      schema: this.schema,
      onValid: (data) => LibLog.main('success', data),
      onInvalid: (errs) => LibLog.main('errors', errs),
    });
  }
}
