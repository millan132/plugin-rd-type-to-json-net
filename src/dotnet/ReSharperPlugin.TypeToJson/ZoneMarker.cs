using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Psi.CSharp;

namespace ReSharperPlugin.TypeToJson;

[ZoneMarker]
public class ZoneMarker : IRequire<ILanguageCSharpZone> { }