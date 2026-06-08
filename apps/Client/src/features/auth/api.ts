import { inject, Injectable } from '@angular/core';
import { AuthSlice } from './slice';
import { ObsResT, ResApiT } from '@/common/types/api';
import { UserT } from '../user/reducer';
import { UseApiSvc } from '@/core/api';
import { LibApiArgs } from '@/core/lib/api/args';
import { UseStorageSvc } from '@/core/services/use_storage';
import { tap } from 'rxjs';
import { RegisterFormT } from './register/paperwork/register_form_mng';
import { UseNavSvc } from '@/core/services/use_nav';

type RegisterDataT = {
  accessToken: string;
  newUser: UserT;
};

@Injectable({
  providedIn: 'root',
})
export class UseAuthApiSvc {
  public readonly authSlice: AuthSlice = inject(AuthSlice);
  public readonly api: UseApiSvc = inject(UseApiSvc);
  public readonly useStorage: UseStorageSvc = inject(UseStorageSvc);
  public readonly useNav: UseNavSvc = inject(UseNavSvc);

  public register(body: RegisterFormT): ObsResT<RegisterDataT> {
    return this.api
      .post<
        RegisterDataT,
        RegisterFormT
      >(LibApiArgs.withURL<RegisterFormT>('/auth/register').body(body).toastOnFulfilled())
      .pipe(
        tap((res: ResApiT<RegisterDataT>) => {
          this.useStorage.setItem('accessToken', res.data.accessToken);
          this.authSlice.setLogged(true);
          this.useNav.replace('/', { from: null });
        }),
      );
  }
}
