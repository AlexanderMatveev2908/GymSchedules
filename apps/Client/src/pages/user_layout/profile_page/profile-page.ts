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
import { UseUserApiSvc } from '@/features/user/api';
import { FormParser } from '@/core/lib/forms/form_parser';
import { UserSlice } from '@/features/user/slice';
import { PageWrapper } from '@/common/components/hoc/page_wrapper/page-wrapper';
import { UserT } from '@/features/user/reducer';
import { Nullable } from '@/common/types/general';

@Component({
  selector: 'app-profile-page',
  imports: [TitlePage, ThumbInput, ReactiveFormsModule, TxtInput, PageWrapper],
  providers: [UseApiTrackerHk],
  templateUrl: './profile-page.html',
  styleUrl: './profile-page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfilePage extends UseFormAppDir {
  public readonly form: FormGroup = ProfileFormMng.form();
  public readonly ProfileUiFct = ProfileUiFct;
  public readonly schema = ProfileFormMng.schema;

  private readonly userSlice: UserSlice = inject(UserSlice);

  private userApi: UseUserApiSvc = inject(UseUserApiSvc);
  public readonly apiTracker: UseApiTrackerHk = inject(UseApiTrackerHk);

  ngOnInit(): void {
    this.setupForm();

    this.useEffect(() => {
      const user: Nullable<UserT> = this.userSlice.user();
      if (!user) return;

      this.form.markAllAsTouched();
      this.form.markAllAsDirty();
      this.form.patchValue({
        firstName: user?.firstName ?? '',
        lastName: user?.lastName ?? '',
        imgFile: user?.thumbnail?.url ?? '',
      });
    });
  }

  public handleSubmit(): void {
    LibRootForm.handleSubmit({
      form: this.form,
      schema: this.schema,
      onValid: (data) => {
        const formData: FormData = FormParser.genFormData(data);

        this.apiTracker.track(this.userApi.putProfile(formData)).subscribe();
      },
      onInvalid: (errs) => LibLog.main('errors', errs),
    });
  }
}
