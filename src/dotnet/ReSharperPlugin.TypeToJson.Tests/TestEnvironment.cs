using System.Threading;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

[assembly: Apartment(ApartmentState.STA)]

namespace ReSharperPlugin.TypeToJson.Tests
{
    [ZoneDefinition]
    public class TypeToJsonTestEnvironmentZone : ITestsEnvZone, IRequire<PsiFeatureTestZone>, IRequire<ITypeToJsonZone> { }

    [ZoneMarker]
    public class ZoneMarker : IRequire<ICodeEditingZone>, IRequire<ILanguageCSharpZone>, IRequire<TypeToJsonTestEnvironmentZone> { }

    [SetUpFixture]
    public class TypeToJsonTestsAssembly : ExtensionTestEnvironmentAssembly<TypeToJsonTestEnvironmentZone> { }
}
