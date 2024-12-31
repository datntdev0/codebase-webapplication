import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ResetUserPasswordDto, UsersServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html'
})
export class ResetPasswordDialogComponent extends AppComponentBase
  implements OnInit {
  public isLoading = false;
  public resetUserPasswordDto: ResetUserPasswordDto;
  id: number;

  constructor(
    injector: Injector,
    private _userService: UsersServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit() {
    this.isLoading = true;
    this.resetUserPasswordDto = new ResetUserPasswordDto();
    this.resetUserPasswordDto.userId = this.id;
    this.resetUserPasswordDto.newPassword = Math.random()
      .toString(36)
      .substr(2, 10);
    this.isLoading = false;
  }

  public resetPassword(): void {
    this.isLoading = true;
    this._userService.resetPassword(this.id, this.resetUserPasswordDto).subscribe(
      () => {
        this.notify.info('Password Reset');
        this.bsModalRef.hide();
      },
      () => {
        this.isLoading = false;
      }
    );
  }
}
