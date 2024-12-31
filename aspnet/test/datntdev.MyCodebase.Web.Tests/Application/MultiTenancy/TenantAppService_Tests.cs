using Shouldly;
using System.Threading.Tasks;
using datntdev.MyCodebase.MultiTenancy.Dto;
using Abp.Application.Services.Dto;
using Abp.MultiTenancy;
using datntdev.MyCodebase.Tests;

namespace datntdev.MyCodebase.Web.Tests.Application.MultiTenancy
{
    public class TenantAppService_Tests : MyCodebaseWebTestBase
    {
        [MultiTenantFact]
        public async Task Create_Test()
        {
            // Arrange
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });
            var tenantDto = new CreateTenantDto
            {
                TenancyName = "TestTenant",
                Name = "Test Tenant",
                AdminEmailAddress = "admin@testtenant.com"
            };

            // Act
            var response = await PostAsync<TenantDto>("/api/services/app/tenants", tenantDto);

            // Assert
            var tenant = await GetAsync<TenantDto>($"/api/services/app/tenants/{response.Id}");
            tenant.TenancyName.ShouldBe(tenantDto.TenancyName);
        }

        [MultiTenantFact]
        public async Task Delete_Test()
        {
            // Arrange
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            // Act
            await DeleteAsync($"/api/services/app/tenants/1");

            // Assert
            var tenants = await GetAsync<PagedResultDto<TenantDto>>("/api/services/app/tenants");
            tenants.TotalCount.ShouldBe(0);
        }

        [MultiTenantFact]
        public async Task GetAll_Test()
        {
            // Arrange
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            // Act
            var response = await GetAsync<PagedResultDto<TenantDto>>("/api/services/app/tenants");

            // Assert
            response.TotalCount.ShouldBe(1);
            response.Items[0].TenancyName.ShouldBe(AbpTenantBase.DefaultTenantName);
        }

        [MultiTenantFact]
        public async Task Update_Test()
        {
            // Arrange
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            var tenantDto = new TenantDto
            {
                Id = 1,
                TenancyName = "TestTenant",
                Name = "Test Tenant",
            };

            // Act
            var response = await PutAsync<TenantDto>("/api/services/app/tenants", tenantDto);

            // Assert
            var tenant = await GetAsync<TenantDto>($"/api/services/app/tenants/{response.Id}");
            tenant.TenancyName.ShouldBe(tenantDto.TenancyName);
        }
    }
}
