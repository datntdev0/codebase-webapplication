using System;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.AspNetCore.Configuration;
using Abp.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using Abp.Collections.Extensions;
using Abp.Web.Api.ProxyScripting.Generators;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using datntdev.MyCodebase.Helpers;
using Abp;

namespace datntdev.MyCodebase;

public class MyCodebaseServiceConvention : IApplicationModelConvention
{
    private static readonly string[] CommonVerbPrefixes = ["Create", "Update", "Delete", "Get", "Patch"];

    private readonly Lazy<AbpAspNetCoreConfiguration> _configuration;

    public MyCodebaseServiceConvention(IServiceCollection services)
    {
        _configuration = new Lazy<AbpAspNetCoreConfiguration>(() =>
        {
            return services
                .GetSingletonService<AbpBootstrapper>()
                .IocManager
                .Resolve<AbpAspNetCoreConfiguration>();
        }, true);
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            var type = controller.ControllerType.AsType();
            var configuration = GetControllerSettingOrNull(type);

            if (typeof(IApplicationService).GetTypeInfo().IsAssignableFrom(type))
            {
                controller.ControllerName = controller.ControllerName
                    .RemovePostFix(ApplicationService.CommonPostfixes);
                ConfigureSelector(controller, configuration);
            }
        }
    }

    private void ConfigureSelector(ControllerModel controller, [CanBeNull] AbpControllerAssemblySetting configuration)
    {
        RemoveEmptySelectors(controller.Selectors);

        if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
        {
            return;
        }

        var moduleName = GetModuleNameOrDefault(controller.ControllerType.AsType());

        foreach (var action in controller.Actions)
        {
            ConfigureSelector(moduleName, controller, action, configuration);
        }
    }

    private void ConfigureSelector(
        string moduleName,
        ControllerModel controller,
        ActionModel action,
        [CanBeNull] AbpControllerAssemblySetting configuration)
    {
        RemoveEmptySelectors(action.Selectors);

        var remoteServiceAtt = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(action.ActionMethod);
        if (remoteServiceAtt != null && !remoteServiceAtt.IsEnabledFor(action.ActionMethod))
        {
            return;
        }

        if (!action.Selectors.Any())
        {
            AddAbpServiceSelector(moduleName, controller, action, configuration);
        }
        else
        {
            NormalizeSelectorRoutes(moduleName, controller, action, configuration);
        }
    }

    private static void AddAbpServiceSelector(
        string moduleName,
        ControllerModel controller,
        ActionModel action,
        [CanBeNull] AbpControllerAssemblySetting configuration)
    {
        var abpServiceSelectorModel = new SelectorModel
        {
            AttributeRouteModel = CreateAbpServiceAttributeRouteModel(moduleName, controller, action)
        };

        var httpMethod = SelectHttpMethod(action, configuration);

        abpServiceSelectorModel.ActionConstraints.Add(new HttpMethodActionConstraint([httpMethod]));

        action.Selectors.Add(abpServiceSelectorModel);
    }

    private static string SelectHttpMethod(ActionModel action, AbpControllerAssemblySetting configuration)
    {
        return configuration?.UseConventionalHttpVerbs == true
            ? ProxyScriptingHelper.GetConventionalVerbForMethodName(action.ActionName)
            : ProxyScriptingHelper.DefaultHttpVerb;
    }

    private static void NormalizeSelectorRoutes(
        string moduleName,
        ControllerModel controller,
        ActionModel action,
        [CanBeNull] AbpControllerAssemblySetting configuration)
    {
        foreach (var selector in action.Selectors)
        {
            if (!selector.ActionConstraints.OfType<HttpMethodActionConstraint>().Any())
            {
                var httpMethod = SelectHttpMethod(action, configuration);
                selector.ActionConstraints.Add(new HttpMethodActionConstraint([httpMethod]));
            }

            if (selector.AttributeRouteModel != null)
            {
                var controllerName = controller.ControllerName.ToKebabCase();
                var template = selector.AttributeRouteModel.Template;
                selector.AttributeRouteModel.Template = $"api/services/{moduleName}/{controllerName}/{template}";
            } 
            else
            {
                selector.AttributeRouteModel = CreateAbpServiceAttributeRouteModel(moduleName, controller, action);
            }

        }
    }

    private string GetModuleNameOrDefault(Type controllerType)
    {
        return GetControllerSettingOrNull(controllerType)?.ModuleName ??
               AbpControllerAssemblySetting.DefaultServiceModuleName;
    }

    [CanBeNull]
    private AbpControllerAssemblySetting GetControllerSettingOrNull(Type controllerType)
    {
        var settings = _configuration.Value.ControllerAssemblySettings.GetSettings(controllerType);
        return settings.FirstOrDefault(setting => setting.TypePredicate(controllerType));
    }

    private static AttributeRouteModel CreateAbpServiceAttributeRouteModel(
        string moduleName, ControllerModel controller, ActionModel action)
    {
        var controllerName = controller.ControllerName.ToKebabCase();
        if (((string[])["GetAllAsync", "CreateAsync", "UpdateAsync"]).Contains(action.ActionMethod.Name))
        {
            return new AttributeRouteModel(
                new RouteAttribute(
                    $"api/services/{moduleName}/{controllerName}"
                )
            );
        }

        if (((string[])["GetAsync", "DeleteAsync"]).Contains(action.ActionMethod.Name))
        {
            return new AttributeRouteModel(
                new RouteAttribute(
                    $"api/services/{moduleName}/{controllerName}/{{id}}"
                )
            );
        }

        var actionName = action.ActionName.RemovePreFix(CommonVerbPrefixes).ToKebabCase();
        return new AttributeRouteModel(
            new RouteAttribute(
                $"api/services/{moduleName}/{controllerName}/{actionName}"
            )
        );
    }

    private static void RemoveEmptySelectors(IList<SelectorModel> selectors)
    {
        selectors
            .Where(IsEmptySelector)
            .ToList()
            .ForEach(s => selectors.Remove(s));
    }

    private static bool IsEmptySelector(SelectorModel selector)
    {
        return selector.AttributeRouteModel == null
               && selector.ActionConstraints.IsNullOrEmpty()
               && selector.EndpointMetadata.IsNullOrEmpty();
    }

}