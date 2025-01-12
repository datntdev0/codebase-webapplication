using Abp.AspNetCore.TestBase;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Json;
using Abp.MultiTenancy;
using Abp.Web.Models;
using datntdev.MyCodebase.EntityFrameworkCore;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Hosting;
using Shouldly;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using datntdev.MyCodebase.Helpers;
using datntdev.MyCodebase.Web.Host.Startup;
using datntdev.MyCodebase.Identity.Dto;
using System.Net.Http.Json;

namespace datntdev.MyCodebase.Web.Tests;

public abstract class MyCodebaseWebTestBase : AbpAspNetCoreIntegratedTestBase<Startup>
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    protected static readonly Lazy<string> ContentRootFolder;

    static MyCodebaseWebTestBase()
    {
        ContentRootFolder = new Lazy<string>(WebContentDirectoryFinder.CalculateContentRootFolder, true);
    }

    protected override IWebHostBuilder CreateWebHostBuilder()
    {
        return base
            .CreateWebHostBuilder()
            .UseContentRoot(ContentRootFolder.Value)
            .UseSetting(WebHostDefaults.ApplicationKey, typeof(MyCodebaseWebHostModule).Assembly.FullName);
    }

    #region Get response

    protected async Task<T> GetAsync<T>(
        string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        var response = await Client.GetAsync(url);
        response.StatusCode.ShouldBe(expectedStatusCode);
        var strResponse = await response.Content.ReadAsStringAsync();
        var ajaxResponse = JsonSerializer.Deserialize<AjaxResponse<T>>(strResponse, JsonSerializerOptions);
        return ajaxResponse.Result;
    }

    protected async Task<T> PostAsync<T>(
        string url, object content, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        var response = await Client.PostAsJsonAsync(url, content);
        response.StatusCode.ShouldBe(expectedStatusCode);
        var strResponse = await response.Content.ReadAsStringAsync();
        var ajaxResponse = JsonSerializer.Deserialize<AjaxResponse<T>>(strResponse, JsonSerializerOptions);
        return ajaxResponse.Result;
    }

    protected async Task<T> PutAsync<T>(
        string url, object content, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        var response = await Client.PutAsJsonAsync(url, content);
        response.StatusCode.ShouldBe(expectedStatusCode);
        var strResponse = await response.Content.ReadAsStringAsync();
        var ajaxResponse = JsonSerializer.Deserialize<AjaxResponse<T>>(strResponse, JsonSerializerOptions);
        return ajaxResponse.Result;
    }

    protected async Task<T> PatchAsync<T>(
        string url, object content, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        var response = await Client.PatchAsJsonAsync(url, content);
        response.StatusCode.ShouldBe(expectedStatusCode);
        var strResponse = await response.Content.ReadAsStringAsync();
        var ajaxResponse = JsonSerializer.Deserialize<AjaxResponse<T>>(strResponse, JsonSerializerOptions);
        return ajaxResponse.Result;
    }

    protected async Task<HttpResponseMessage> DeleteAsync(
        string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        var response = await Client.DeleteAsync(url);
        response.StatusCode.ShouldBe(expectedStatusCode);
        return response;
    }

    #endregion

    #region Authenticate

    /// <summary>
    /// /api/TokenAuth/Authenticate
    /// TokenAuthController
    /// </summary>
    /// <param name="tenancyName"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    protected async Task AuthenticateAsync(string tenancyName, LoginRequestDto input)
    {
        if (!tenancyName.IsNullOrWhiteSpace())
        {
            var tenant = UsingDbContext(context => context.Tenants.FirstOrDefault(t => t.TenancyName == tenancyName));
            if (tenant != null)
            {
                AbpSession.TenantId = tenant.Id;
                Client.DefaultRequestHeaders.Add("Abp.TenantId", tenant.Id.ToString());  //Set TenantId
            }
        }

        var response = await Client.PostAsync("/api/identity/accounts/login",
            new StringContent(input.ToJsonString(), Encoding.UTF8, "application/json"));
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = JsonSerializer.Deserialize<AjaxResponse<LoginResultDto>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Result.AccessToken);

        AbpSession.UserId = result.Result.UserId;
    }

    #endregion

    #region Login

    protected void LoginAsHostAdmin()
    {
        LoginAsHost(AbpUserBase.AdminUserName);
    }

    protected void LoginAsDefaultTenantAdmin()
    {
        LoginAsTenant(AbpTenantBase.DefaultTenantName, AbpUserBase.AdminUserName);
    }

    protected void LoginAsHost(string userName)
    {
        AbpSession.TenantId = null;

        var user =
            UsingDbContext(
                context =>
                    context.Users.FirstOrDefault(u => u.TenantId == AbpSession.TenantId && u.UserName == userName));
        if (user == null)
        {
            throw new Exception("There is no user: " + userName + " for host.");
        }

        AbpSession.UserId = user.Id;
    }

    protected void LoginAsTenant(string tenancyName, string userName)
    {
        var tenant = UsingDbContext(context => context.Tenants.FirstOrDefault(t => t.TenancyName == tenancyName));
        if (tenant == null)
        {
            throw new Exception("There is no tenant: " + tenancyName);
        }

        AbpSession.TenantId = tenant.Id;

        var user =
            UsingDbContext(
                context =>
                    context.Users.FirstOrDefault(u => u.TenantId == AbpSession.TenantId && u.UserName == userName));
        if (user == null)
        {
            throw new Exception("There is no user: " + userName + " for tenant: " + tenancyName);
        }

        AbpSession.UserId = user.Id;
    }

    #endregion

    #region UsingDbContext

    protected void UsingDbContext(Action<MyCodebaseDbContext> action)
    {
        using (var context = IocManager.Resolve<MyCodebaseDbContext>())
        {
            action(context);
            context.SaveChanges();
        }
    }

    protected T UsingDbContext<T>(Func<MyCodebaseDbContext, T> func)
    {
        T result;

        using (var context = IocManager.Resolve<MyCodebaseDbContext>())
        {
            result = func(context);
            context.SaveChanges();
        }

        return result;
    }

    protected async Task UsingDbContextAsync(Func<MyCodebaseDbContext, Task> action)
    {
        using (var context = IocManager.Resolve<MyCodebaseDbContext>())
        {
            await action(context);
            await context.SaveChangesAsync(true);
        }
    }

    protected async Task<T> UsingDbContextAsync<T>(Func<MyCodebaseDbContext, Task<T>> func)
    {
        T result;

        using (var context = IocManager.Resolve<MyCodebaseDbContext>())
        {
            result = await func(context);
            await context.SaveChangesAsync(true);
        }

        return result;
    }

    #endregion

    #region ParseHtml

    protected IHtmlDocument ParseHtml(string htmlString)
    {
        return new HtmlParser().ParseDocument(htmlString);
    }

    #endregion
}
