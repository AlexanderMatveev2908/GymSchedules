import { UseNavSvc } from '@/core/services/use_nav';
import { AuthSlice } from '@/features/auth/slice';
import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-user-layout',
  imports: [RouterOutlet],
  templateUrl: './user-layout.html',
  styleUrl: './user-layout.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserLayout implements OnInit {
  private readonly useNav: UseNavSvc = inject(UseNavSvc);
  private readonly authSlice: AuthSlice = inject(AuthSlice);

  ngOnInit(): void {
    this.useNav.ifPathStartsWith('/user', () => {
      if (this.authSlice.isLogged()) return;

      this.useNav.replace('/auth/login', { from: null });
    });
  }
}
