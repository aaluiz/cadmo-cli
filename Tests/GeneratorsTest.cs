using System.Collections.Immutable;
using Contracts.Interfaces;
using NUnit.Framework;
using Services;
using Services.Coder;

namespace Tests
{
    [TestFixture]
    public class GeneratorsTest
    {
        IClassDefinition? _classDefiner;
        IMethodDefinition? _methodDefinition;
        [SetUp]
        public void Setup()
        {
            _classDefiner = new ClassDefinition(new BuilderClassDefinition());
            _methodDefinition = new MethodDefinition(new BuilderMethodDefinition());
        }


        [Test]
        public void ClassBusinnesLayer_ReturnStringClassCode()
        {
            //arrange
            var imports = new string[] { "Contracts", "System" }.ToImmutableList();
            var inheritages = new string[] { "Test", "IInter<Generic>" }.ToImmutableList();
            var businnessLayerService = _classDefiner!.Builder
                .Imports(imports)
                .Namespace("Services")
                .Name("Test")
                .Inheritance(inheritages)
                .Create();

            //act
            string result = businnessLayerService.ClassCode;

            //assert
            Assert.IsTrue(result.Contains("class"));
            Assert.IsTrue(result.Contains("}"));
            Assert.IsTrue(result.Contains("using Contracts"));
        }
        
    }
}