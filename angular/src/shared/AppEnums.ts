import { TenantStatus } from '@shared/service-proxies/service-proxies';


export class AppTenantAvailabilityState {
    static NotFound: number = TenantStatus._0;
    static Available: number = TenantStatus._1;
    static InActive: number = TenantStatus._2;
}
