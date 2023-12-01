using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl.DataContext;
using ReSharperPlugin.TypeToJson.Resources;
using ReSharperPlugin.TypeToJson.Serialization;

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

        var serializedInstance = TypeToJsonSerializer.Serialize(classType);
    }
}