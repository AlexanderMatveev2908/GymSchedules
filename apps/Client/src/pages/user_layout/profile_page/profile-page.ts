import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { TitlePage } from '@/common/components/general/title_page/title-page';
import { ImgInput } from '@/common/components/forms/img_input/img-input';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ProfileFormMng } from '@/features/user/profile/paperwork';
import { ProfileUiFct } from '@/features/user/profile/ui_fct';
import { UseFormAppDir } from '@/core/directives/use_form_app';
import { UseApiTrackerHk } from '@/core/hooks/use_api_tracker';

@Component({
  selector: 'app-profile-page',
  imports: [TitlePage, ImgInput, ReactiveFormsModule],
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
}
