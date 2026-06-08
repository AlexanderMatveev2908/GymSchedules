import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { BgBlack } from '../bg_black/bg-black';
import { SidebarSlice } from '@/features/sidebar/slice';
import { LinksNonLoggedUiFct } from './etc/ui_fct';
import { RouterLink } from '@angular/router';
import { NgClass } from '@angular/common';
import { AuthSlice } from '@/features/auth/slice';

@Component({
  selector: 'app-sidebar',
  imports: [BgBlack, RouterLink, NgClass],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Sidebar {
  public readonly authSlice: AuthSlice = inject(AuthSlice);

  public readonly sidebarSlice: SidebarSlice = inject(SidebarSlice);
  public readonly linksNonLoggedUiFct = LinksNonLoggedUiFct;
}
