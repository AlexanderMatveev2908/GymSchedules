import { AfterViewInit, Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Toast } from '@/layout/toast/toast';
import { WakeUp } from '@/layout/wake_up/wake-up';
import { Navbar } from '@/layout/navbar/navbar';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Toast, WakeUp, Navbar],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {}
