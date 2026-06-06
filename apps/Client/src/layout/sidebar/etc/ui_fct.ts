import { LinkSidebar } from '@/common/types/dom';
import { v4 } from 'uuid';

export class LinksNonLoggedUiFct {
  public static register: LinkSidebar = {
    id: v4(),
    label: 'Register',
    url: '/auth/register',
  };
  public static login: LinkSidebar = {
    id: v4(),
    label: 'Login',
    url: '/auth/login',
  };

  public static links: LinkSidebar[] = [this.register, this.login];
}
