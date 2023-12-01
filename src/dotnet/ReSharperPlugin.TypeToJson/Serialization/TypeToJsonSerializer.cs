using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Util;
using Newtonsoft.Json;

namespace ReSharperPlugin.TypeToJson.Serialization;

public static class TypeToJsonSerializer
{
    public static string Serialize(ITypeElement typeElement)
    {
        if (!typeElement.IsClassLike())
        {
            return string.Empty;
        }

        var pocoInstance = CreateInstanceAsPoco(typeElement, new Stack<string>());

        return JsonConvert.SerializeObject(pocoInstance);
    }

    private static Dictionary<string, object> CreateInstanceAsPoco(ITypeElement typeElement, Stack<string> nestedTypes)
    {
        var clrTypeName = GetClrName(typeElement);
        if (nestedTypes.Contains(clrTypeName))
        {
            return null;
        }

        nestedTypes.Push(clrTypeName);
        
        var properties = typeElement.Properties.ToList();
        var flattenTypeInstance = new Dictionary<string, object>(properties.Count);
        
        foreach (var property in properties)
        {
            var propertyType = property.ReturnType.GetTypeElement();
            if (propertyType is null || property.ReturnType.IsNullable())
            {
                continue;
            }
            
            var propertyInstance = property.ReturnType.IsReferenceType()
                ? CreateInstanceAsPoco(propertyType, nestedTypes)
                : GetInstanceOfValueType(propertyType);

            flattenTypeInstance.Add(property.ShortName, propertyInstance);
        }

        nestedTypes.Pop();
        
        return flattenTypeInstance;
    }

    private static object GetInstanceOfValueType(ITypeElement typeElement)
    {
        var clrTypeName = GetClrName(typeElement);
        var type = Type.GetType(clrTypeName);
        return type is null ? null : Activator.CreateInstance(type);
    }

    private static string GetClrName(ITypeElement typeElement) => typeElement.GetClrName().FullName;
}
