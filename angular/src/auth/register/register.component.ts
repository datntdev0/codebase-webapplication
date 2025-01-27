import { Component, Injector, Input } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
  IdentityServiceProxy,
  RegisterRequestDto,
} from '@shared/service-proxies/service-proxies';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
  templateUrl: './register.component.html',
  animations: [accountModuleAnimation()]
})
export class RegisterComponent extends AppComponentBase {
  model: RegisterRequestDto = new RegisterRequestDto();
  saving = false;

  constructor(
    injector: Injector,
    private _router: Router,
    private _authService: AppAuthService,
    private _identityService: IdentityServiceProxy
  ) {
    super(injector);
  }

  save(): void {
    this.saving = true;
    this._identityService
      .register(this.model)
      .pipe(finalize(() => this.saving = false))
      .subscribe((result) => {
        if (!result.canLogin) {
          this.notify.success(this.l('SuccessfullyRegistered'));
          this._router.navigate(['/login']);
          return;
        }

        // Autheticate
        this.saving = true;
        this._authService.loginRequestDto.userNameOrEmailAddress = this.model.userName;
        this._authService.loginRequestDto.password = this.model.password;
        this._authService.authenticate(() => this.saving = false);
      });
  }
}
