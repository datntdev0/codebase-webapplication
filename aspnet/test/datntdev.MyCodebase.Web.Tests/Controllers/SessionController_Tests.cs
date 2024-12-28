using datntdev.MyCodebase.Session.Dto;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace datntdev.MyCodebase.Web.Tests.Controllers;

public class SessionController_Tests : MyCodebaseWebTestBase
{
    [Fact]
    public async Task GetSession_Host_Test()
    {
        // Arrange
        await AuthenticateAsync(null, new()
        {
            UserNameOrEmailAddress = "admin",
            Password = "123qwe"
        });

        // Act
        var response = await GetAsync<SessionDto>("/api/session");

        // Assert
        response.Tenant.ShouldBeNull();
    }

    [Fact]
    public async Task GetSession_Tenant_Test()
    {
        // Arrange
        await AuthenticateAsync("Default", new()
        {
            UserNameOrEmailAddress = "admin",
            Password = "123qwe"
        });

        // Act
        var response = await GetAsync<SessionDto>("/api/session");

        // Assert
        response.Tenant.ShouldNotBeNull();
    }
}