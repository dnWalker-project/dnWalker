﻿using dnWalker.TestGenerator.TestProjects;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.XunitFramework
{
    internal class XunitTestProjectGenerator : ITestProjectGenerator
    {
        private readonly XunitTestProjectTemplate _template = new XunitTestProjectTemplate();

        public void GenerateProject(ITestProjectContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            string solutionDirectory = Path.GetFullPath(context.SolutionDirectory ?? Environment.CurrentDirectory);

            string projectName = context.ProjectName ?? GenerateTestProjectName(solutionDirectory);

            string projectDir = Path.Combine(solutionDirectory, projectName);

            Directory.CreateDirectory(projectDir);

            string projectFile = Path.Combine(projectDir, projectName + ".csproj");

            File.WriteAllText(projectFile, GenerateProjectFileContent(context));

        }

        public string GenerateProjectFileContent(ITestProjectContext context)
        {
            return _template.GenerateContent(context);
        }

        private static string GenerateTestProjectName(string solutionDirectory)
        {
            const string BaseProjectName = "XunitTestProject";

            string[] dirs = Directory.GetDirectories(solutionDirectory).Where(d => d.StartsWith(BaseProjectName)).ToArray();

            if (dirs.Length == 0)
            {
                return BaseProjectName;
            }
            else
            {
                return BaseProjectName + dirs.Length + 1;
            }
        }
    }
}
