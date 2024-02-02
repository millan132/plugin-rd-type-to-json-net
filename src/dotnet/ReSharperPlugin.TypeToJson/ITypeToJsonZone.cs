using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Psi.CSharp;

namespace ReSharperPlugin.TypeToJson
{
    [ZoneDefinition]
    public interface ITypeToJsonZone : IZone, IRequire<ILanguageCSharpZone>
    {
    }
}
