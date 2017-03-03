using System;

namespace CodeSmellAnalyzer.Tests
{
    public class CodeSmellAnalyzerFixture
    {
        public CodeSmellAnalyzer Analyzer { get; set; }

        public CodeSmellAnalyzerFixture()
        {
            string solutionPath = @"C: \Users\akoke\Documents\Visual Studio 2015\Projects\TransactionWrapper\TransactionWrapper.sln";

            Analyzer = new CodeSmellAnalyzer(solutionPath);
        }
    }
}
