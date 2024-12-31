using Shouldly;
using System.Threading.Tasks;
using Xunit;
using datntdev.MyCodebase.Authorization.Roles.Dto;
using Abp.Application.Services.Dto;
using datntdev.MyCodebase.Authorization.Roles;

namespace datntdev.MyCodebase.Web.Tests.Application.Authorization
{
    public class RolesAppService_Tests : MyCodebaseWebTestBase
    {
        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });
            var roleDto = new CreateRoleDto
            {
                Name = "TestRole",
                DisplayName = "Test Role",
                GrantedPermissions = ["Permission1", "Permission2"]
            };

            // Act
            var response = await PostAsync<RoleDto>("/api/services/app/roles", roleDto);

            // Assert
            var role = await GetAsync<RoleDto>($"/api/services/app/roles/{response.Id}");
            role.Name.ShouldBe(roleDto.Name);
            role.DisplayName.ShouldBe(roleDto.DisplayName);
        }

        [Fact]
        public async Task Delete_Test()
        {
            // Arrange
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });
            var roleDto = new CreateRoleDto
            {
                Name = "TestRole",
                DisplayName = "Test Role",
                GrantedPermissions = ["Permission1", "Permission2"]
            };
            var newRole = await PostAsync<RoleDto>("/api/services/app/roles", roleDto);

            // Act
            await DeleteAsync($"/api/services/app/roles/{newRole.Id}");

            // Assert
            var roles = await GetAsync<PagedResultDto<RoleDto>>("/api/services/app/roles");
            roles.TotalCount.ShouldBe(1);
        }

        [Fact]
        public async Task GetAll_Test()
        {
            // Arrange
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            // Act
            var response = await GetAsync<PagedResultDto<RoleDto>>("/api/services/app/roles");

            // Assert
            response.TotalCount.ShouldBe(1);
            response.Items[0].Name.ShouldBe(StaticRoleNames.Host.Admin);
        }

        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });
            var roleDto = new CreateRoleDto
            {
                Name = "TestRole",
                DisplayName = "Test Role",
                GrantedPermissions = ["Permission1", "Permission2"]
            };
            var newRole = await PostAsync<RoleDto>("/api/services/app/roles", roleDto);
            newRole.Name = "UpdatedRole";
            newRole.DisplayName = "Updated Role";

            // Act
            var response = await PutAsync<RoleDto>("/api/services/app/roles", newRole);

            // Assert
            var role = await GetAsync<RoleDto>($"/api/services/app/roles/{response.Id}");
            role.Name.ShouldBe(newRole.Name);
            role.DisplayName.ShouldBe(newRole.DisplayName);
        }
    }
}
