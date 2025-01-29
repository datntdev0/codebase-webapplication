import {
  ChangeDetectionStrategy,
  Component,
  Injector
} from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
  selector: 'sidebar-user-panel',
  templateUrl: './sidebar-user-panel.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SidebarUserPanelComponent extends AppComponentBase {
  constructor(injector: Injector, private _authService: AppAuthService) {
    super(injector);
  }

  logout(): void {
    this._authService.logout();
  }
}
