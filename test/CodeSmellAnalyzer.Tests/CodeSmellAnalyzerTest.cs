using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace CodeSmellAnalyzer.Tests
{
    public class CodeSmellAnalyzerTest : IClassFixture<CodeSmellAnalyzerFixture>
    {
        private readonly CodeSmellAnalyzer _analyzer;

        public CodeSmellAnalyzerTest(CodeSmellAnalyzerFixture fixture)
        {
            _analyzer = fixture.Analyzer;
        }

        [Theory]
        [InlineData(@"C: \Users\akoke\Documents\Visual Studio 2015\Projects\TransactionWrapper\TransactionWrapper.sl")]
        public void OpenSolution_InvalidSolutionPathGive_ShouldThrowException(string solutionPath)
        {
            var analyzer = new CodeSmellAnalyzer(solutionPath);
            Assert.Throws<AggregateException>(()=>analyzer.OpenSolution());
        }

        [Theory]
        [InlineData(@"C: \Users\akoke\Documents\Visual Studio 2015\Projects\TransactionWrapper\TransactionWrapper.sln")]
        public void OpenSolution_ValidSolutionPathGiven_ShouldReturnSolution(string solutionPath)
        {
            var analyzer = new CodeSmellAnalyzer(solutionPath);
            Solution solution = analyzer.OpenSolution();

            Assert.NotNull(solution);
        }

        [Fact]
        public void GetDocuments_ExistedDocumentsGiven_ShouldReturnAllDocuments()
        {
            Solution solution = _analyzer.OpenSolution();
            IEnumerable<Document> documents = _analyzer.GetDocuments(solution);

            Assert.NotNull(documents);
        }

        [Fact]
        public void GetMethods_ExistedFiveMethodsGiven_ShouldReturnFiveMethods()
        {
            Solution solution = _analyzer.OpenSolution();
            IEnumerable<Document> documents = _analyzer.GetDocuments(solution);
            IEnumerable<MethodDeclarationSyntax> methods =_analyzer.GetMethods(documents);
            int actual = methods.ToList().Count();

            Assert.Equal(5, actual);
        }

        [Fact]
        public void FindMethodParameterCountSmell_OneCodeSmellParameterGiven_ShouldReturnOneCodeSmell()
        {
            Solution solution = _analyzer.OpenSolution();
            IEnumerable<Document> documents = _analyzer.GetDocuments(solution);
            IEnumerable<MethodDeclarationSyntax> methods = _analyzer.GetMethods(documents);
            IDictionary<string, int> parameterCountSmells = _analyzer.FindMethodParameterCountSmell(methods);

            Assert.Equal(1, parameterCountSmells.Count);
        }

        [Fact]
        public void FindMethodParameterCountSmell_OneCodeSmellWithSevenParameterGiven_ShouldReturnOneCodeSmellWithSevenParameter()
        {
            Solution solution = _analyzer.OpenSolution();
            IEnumerable<Document> documents = _analyzer.GetDocuments(solution);
            IEnumerable<MethodDeclarationSyntax> methods = _analyzer.GetMethods(documents);
            IDictionary<string, int> parameterCountSmells = _analyzer.FindMethodParameterCountSmell(methods);

            Assert.Equal(7, parameterCountSmells.First().Value);
        }
    }
}
