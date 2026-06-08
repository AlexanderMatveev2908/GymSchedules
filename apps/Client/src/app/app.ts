import { AfterViewInit, Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Toast } from '@/layout/toast/toast';
import { WakeUp } from '@/layout/wake_up/wake-up';
import { Navbar } from '@/layout/navbar/navbar';
import { Sidebar } from '@/layout/sidebar/sidebar';
import { UseLogOnMountSvc } from './etc/log_on_mount';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Toast, WakeUp, Navbar, Sidebar],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App implements OnInit {
  private readonly logOnMount: UseLogOnMountSvc = inject(UseLogOnMountSvc);

  ngOnInit(): void {
    this.logOnMount.main();
  }
}
