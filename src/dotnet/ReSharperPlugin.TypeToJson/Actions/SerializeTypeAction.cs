using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.TextControl.DataContext;
using Newtonsoft.Json;
using ReSharperPlugin.TypeToJson.Resources;

namespace ReSharperPlugin.TypeToJson.Actions;

[Action(
    ResourceType: typeof(SerializeType),
    TextResourceName: nameof(SerializeType.Text),
    DescriptionResourceName = nameof(SerializeType.Description))]
public class SerializeTypeAction : IExecutableAction
{
    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
        return true;
    }
    
    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
        var solution = context.GetData(ProjectModelDataConstants.SOLUTION);
        if (solution == null)
            return;

        var textControl = context.TextControl();
        if (textControl == null)
            return;
        
        var classDeclaration = TextControlToPsi.GetElementFromCaretPosition<IClassDeclaration>(solution, textControl);
        var classType = classDeclaration?.DeclaredElement;
        var classProperties = classType?.Properties.ToList() ?? new List<IProperty>();
        
        var generatedObject = new ExpandoObject() as IDictionary<string, object>;

        foreach (var property in classProperties)
        {
            var propertyType = property.ContainingType;
            if (!propertyType.IsObjectClass())
            {
                var clrName = property.ContainingType?.GetClrName();
                var clrType = Type.GetType(clrName?.FullName ?? "");
                var instance = Activator.CreateInstance(clrType);
                generatedObject.Add(property.ShortName, instance);
            }
        }

        var objectJson = JsonConvert.SerializeObject(generatedObject);
    }
        
}