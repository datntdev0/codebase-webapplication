import { NgModule } from '@angular/core';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.IdentityServiceProxy,
        ApiServiceProxies.RolesServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantsServiceProxy,
        ApiServiceProxies.UsersServiceProxy,
    ]
})
export class ServiceProxyModule { }
