import { AuthLayout } from '@/pages/auth_layout/auth-layout';
import { LoginPage } from '@/pages/auth_layout/login_page/login-page';
import { RegisterPage } from '@/pages/auth_layout/register_page/register-page';
import { HomePage } from '@/pages/home_page/home-page';
import { NotFoundPage } from '@/pages/not_found_page/not-found-page';
import { NoticePage } from '@/pages/notice_page/notice-page';
import { ProtectedPage } from '@/pages/protected_page/protected-page';
import { ProfilePage } from '@/pages/user_layout/profile_page/profile-page';
import { UserLayout } from '@/pages/user_layout/user-layout';
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    component: HomePage,
  },
  {
    path: 'notice',
    component: NoticePage,
  },
  {
    path: 'auth',
    component: AuthLayout,
    children: [
      {
        path: 'register',
        component: RegisterPage,
      },
      {
        path: 'login',
        component: LoginPage,
      },
    ],
  },
  {
    path: 'user',
    component: UserLayout,
    children: [
      {
        path: 'profile',
        component: ProfilePage,
      },
    ],
  },
  {
    path: 'protected',
    component: ProtectedPage,
  },
  {
    path: '**',
    component: NotFoundPage,
  },
];
