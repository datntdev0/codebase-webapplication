using datntdev.MyCodebase.Models.TokenAuth;
using datntdev.MyCodebase.Web.Controllers;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace datntdev.MyCodebase.Web.Tests.Controllers;

public class HomeController_Tests : MyCodebaseWebTestBase
{
    [Fact]
    public async Task Index_Test()
    {
        await AuthenticateAsync(null, new AuthenticateModel
        {
            UserNameOrEmailAddress = "admin",
            Password = "123qwe"
        });

        //Act
        var response = await GetResponseAsStringAsync(
            GetUrl<HomeController>(nameof(HomeController.Index))
        );

        //Assert
        response.ShouldNotBeNullOrEmpty();
    }
}