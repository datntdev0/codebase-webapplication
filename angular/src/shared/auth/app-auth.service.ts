import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { TokenService, LogService, UtilsService } from 'abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { IdentityServiceProxy, LoginRequestDto, LoginResultDto } from '@shared/service-proxies/service-proxies';

@Injectable()
export class AppAuthService {
    loginRequestDto: LoginRequestDto;
    loginResultDto: LoginResultDto;
    rememberMe: boolean;

    constructor(
        private _identityService: IdentityServiceProxy,
        private _router: Router,
        private _utilsService: UtilsService,
        private _tokenService: TokenService,
        private _logService: LogService
    ) {
        this.clear();
    }

    logout(reload?: boolean): void {
        abp.auth.clearToken();
        abp.utils.deleteCookie(AppConsts.authorization.encryptedAuthTokenName);
        
        if (reload !== false) {
            location.href = AppConsts.appBaseUrl;
        }
    }

    authenticate(finallyCallback?: () => void): void {
        finallyCallback = finallyCallback || (() => { });

        this._identityService
            .login(this.loginRequestDto)
            .pipe(
                finalize(() => {
                    finallyCallback();
                })
            )
            .subscribe((result) => {
                this.processAuthenticateResult(result);
            });
    }

    private processAuthenticateResult(
        loginResultDto: LoginResultDto
    ) {
        this.loginResultDto = loginResultDto;

        if (loginResultDto.accessToken) {
            // Successfully logged in
            this.login(
                loginResultDto.accessToken,
                loginResultDto.encryptedAccessToken,
                loginResultDto.expireInSeconds,
                this.rememberMe
            );
        } else {
            // Unexpected result!

            this._logService.warn('Unexpected authenticateResult!');
            this._router.navigate(['account/login']);
        }
    }

    private login(
        accessToken: string,
        encryptedAccessToken: string,
        expireInSeconds: number,
        rememberMe?: boolean
    ): void {
        const tokenExpireDate = rememberMe
            ? new Date(new Date().getTime() + 1000 * expireInSeconds)
            : undefined;

        this._tokenService.setToken(accessToken, tokenExpireDate);

        this._utilsService.setCookieValue(
            AppConsts.authorization.encryptedAuthTokenName,
            encryptedAccessToken,
            tokenExpireDate,
            abp.appPath
        );

        let initialUrl = UrlHelper.initialUrl;
        if (initialUrl.indexOf('/login') > 0) {
            initialUrl = AppConsts.appBaseUrl;
        }

        location.href = initialUrl;
    }

    private clear(): void {
        this.loginRequestDto = new LoginRequestDto();
        this.loginRequestDto.rememberClient = false;
        this.loginResultDto = null;
        this.rememberMe = false;
    }
}
