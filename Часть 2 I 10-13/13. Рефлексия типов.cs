// Практика «Документация»

using System;
using System.Linq;
using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
    private readonly Type type = typeof(T);

    public string GetApiDescription()
    {
        return type.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;
    }

    public string[] GetApiMethodNames()
    {
        return type.GetMethods()
            .Where(m => m.GetCustomAttribute<ApiMethodAttribute>() != null)
            .Where(m => m.GetCustomAttribute<ApiDescriptionAttribute>() != null)
            .Select(m => m.Name)
            .ToArray();
    }

    public string GetApiMethodDescription(string methodName)
    {
        var method = type.GetMethod(methodName);
        return method?.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;
    }

    public string[] GetApiMethodParamNames(string methodName)
    {
        var method = type.GetMethod(methodName);
        return method?.GetParameters()
            .Select(p => p.Name)
            .ToArray();
    }

    public string GetApiMethodParamDescription(string methodName, string paramName)
    {
        var method = type.GetMethod(methodName);
        var parameter = method?.GetParameters()
            .FirstOrDefault(p => p.Name == paramName);
        return parameter?.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;
    }

    public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
    {
        var method = type.GetMethod(methodName);
        if (method == null) return CreateDefaultParamDescription(paramName);

        var parameter = method.GetParameters()
            .FirstOrDefault(p => p.Name == paramName);
        if (parameter == null) return CreateDefaultParamDescription(paramName);

        return CreateParamDescription(parameter);
    }

    public ApiMethodDescription GetApiMethodFullDescription(string methodName)
    {
        var method = type.GetMethod(methodName);
        if (method?.GetCustomAttribute<ApiMethodAttribute>() == null)
            return null;

        var description = new ApiMethodDescription
        {
            MethodDescription = new CommonDescription(
                methodName,
                method.GetCustomAttribute<ApiDescriptionAttribute>()?.Description),
            ParamDescriptions = method.GetParameters()
                .Select(p => GetApiMethodParamFullDescription(methodName, p.Name))
                .ToArray(),
            ReturnDescription = GetReturnDescription(method)
        };

        return description;
    }

    private ApiParamDescription GetReturnDescription(MethodInfo method)
    {
        var returnParam = method.ReturnParameter;

        // возвращаем значение null, если нет атрибутов возвращаемого значения
        if (returnParam.GetCustomAttribute<ApiRequiredAttribute>() == null &&
            returnParam.GetCustomAttribute<ApiIntValidationAttribute>() == null)
        {
            return null;
        }

        var description = new ApiParamDescription
        {
            ParamDescription = new CommonDescription(),
            Required = returnParam.GetCustomAttribute<ApiRequiredAttribute>()?.Required ?? false
        };

        var intValidation = returnParam.GetCustomAttribute<ApiIntValidationAttribute>();
        if (intValidation != null)
        {
            description.MinValue = intValidation.MinValue;
            description.MaxValue = intValidation.MaxValue;
        }

        return description;
    }

    private ApiParamDescription CreateParamDescription(ParameterInfo parameter)
    {
        var description = new ApiParamDescription
        {
            ParamDescription = new CommonDescription(
                parameter.Name,
                parameter.GetCustomAttribute<ApiDescriptionAttribute>()?.Description),
            Required = parameter.GetCustomAttribute<ApiRequiredAttribute>()?.Required ?? false
        };

        var intValidation = parameter.GetCustomAttribute<ApiIntValidationAttribute>();
        if (intValidation != null)
        {
            description.MinValue = intValidation.MinValue;
            description.MaxValue = intValidation.MaxValue;
        }

        return description;
    }

    private ApiParamDescription CreateDefaultParamDescription(string paramName)
    {
        return new ApiParamDescription
        {
            ParamDescription = new CommonDescription(paramName),
            Required = false
        };
    }
}