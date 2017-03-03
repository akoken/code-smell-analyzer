using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System.Linq;

namespace CodeSmellAnalyzer
{
    public class CodeSmellAnalyzer
    {
        public string SolutionPath { get; private set; }
        public CodeSmellAnalyzer(string solutionPath)
        {
            SolutionPath = solutionPath;
        }

        public Solution OpenSolution()
        {
            using (var workspace = MSBuildWorkspace.Create())
            {
                return workspace.OpenSolutionAsync(SolutionPath).Result;
            }
        }

        public IEnumerable<Document> GetDocuments(Solution solution)
        {
            foreach (var projectId in solution.ProjectIds)
            {
                var project = solution.GetProject(projectId);
                foreach (Document document in project.Documents)
                {
                    if (document.SupportsSyntaxTree)
                        yield return document;
                }
            }
        }

        public IEnumerable<MethodDeclarationSyntax> GetMethods(IEnumerable<Document> documents)
        {
            return documents.SelectMany(p => p.GetSyntaxRootAsync().Result.DescendantNodes().OfType<MethodDeclarationSyntax>());
        }

        public IDictionary<string, int> FindMethodParameterCountSmell(IEnumerable<MethodDeclarationSyntax> methods)
        {
            var smellyClasses = new Dictionary<string, int>();
            foreach (MethodDeclarationSyntax method in methods)
            {
                ParameterListSyntax parameterList = method.ParameterList;
                if (parameterList.Parameters.Count > 5)
                    smellyClasses.Add(method.Identifier.ValueText, parameterList.Parameters.Count);
            }

            return smellyClasses;
        }
    }
}
