using Shouldly;
using System.Threading.Tasks;
using Xunit;
using datntdev.MyCodebase.Authorization.Users.Dto;
using Abp.Application.Services.Dto;

namespace datntdev.MyCodebase.Web.Tests.Application.Authorization
{
    public class UsersAppService_Tests : MyCodebaseWebTestBase
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
            var userDto = new CreateUserDto
            {
                UserName = "TestUser",
                Name = "Test User",
                Surname = "Surname",
                EmailAddress = "testuser@example.com",
                Password = "123qwe",
                RoleNames = ["Admin"],
            };

            // Act
            var response = await PostAsync<UserDto>("/api/services/app/users", userDto);

            // Assert
            var user = await GetAsync<UserDto>($"/api/services/app/users/{response.Id}");
            user.UserName.ShouldBe(userDto.UserName);
            user.EmailAddress.ShouldBe(userDto.EmailAddress);
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
            var userDto = new CreateUserDto
            {
                UserName = "TestUser",
                Name = "Test User",
                Surname = "Surname",
                EmailAddress = "testuser@example.com",
                Password = "123qwe",
            };
            var newUser = await PostAsync<UserDto>("/api/services/app/users", userDto);

            // Act
            await DeleteAsync($"/api/services/app/users/{newUser.Id}");

            // Assert
            var users = await GetAsync<PagedResultDto<UserDto>>("/api/services/app/users");
            users.TotalCount.ShouldBe(1);
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
            var response = await GetAsync<PagedResultDto<UserDto>>("/api/services/app/users");

            // Assert
            response.TotalCount.ShouldBe(1);
            response.Items[0].UserName.ShouldBe("admin");
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
            var userDto = new CreateUserDto
            {
                UserName = "TestUser",
                Name = "Test User",
                Surname = "Surname",
                EmailAddress = "testuser@example.com",
                Password = "123qwe",
            };
            var newUser = await PostAsync<UserDto>("/api/services/app/users", userDto);
            newUser.UserName = "UpdatedUser";
            newUser.Name = "Updated User";
            newUser.EmailAddress = "updateduser@example.com";

            // Act
            var response = await PutAsync<UserDto>("/api/services/app/users", newUser);

            // Assert
            var user = await GetAsync<UserDto>($"/api/services/app/users/{response.Id}");
            user.UserName.ShouldBe(newUser.UserName);
            user.EmailAddress.ShouldBe(newUser.EmailAddress);
        }

        [Fact]
        public async Task ResetPassword_Test()
        {
            // Arrange
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });
            var resetPasswordDto = new ResetUserPasswordDto
            {
                UserId = 1,
                AdminPassword = "123qwe",
                NewPassword = "newpassword123"
            };

            // Act
            await PostAsync<string>($"/api/services/app/users/1/password", resetPasswordDto);

            // Assert
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "newpassword123"
            });
        }

        [Fact]
        public async Task Activate_Test()
        {
            // Arrange
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            // Act
            await PostAsync<string>($"/api/services/app/users/1/activate", null);

            // Assert
            var user = await GetAsync<UserDto>($"/api/services/app/users/1");
            user.IsActive.ShouldBe(true);
        }

        [Fact]
        public async Task Deactivate_Test()
        {
            // Arrange
            await AuthenticateAsync(null, new()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            // Act
            await PostAsync<string>($"/api/services/app/users/1/deactivate", null);

            // Assert
            var user = await GetAsync<UserDto>($"/api/services/app/users/1");
            user.IsActive.ShouldBe(false);
        }
    }
}
