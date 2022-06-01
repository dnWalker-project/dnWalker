﻿using dnWalker.TestGenerator.TestClasses;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.XunitFramework
{
    internal class XunitTestClassGenerator : ITestClassGenerator
    {
        private readonly XunitTestClassTemplate _template = new XunitTestClassTemplate();

        public void GenerateTestClass(ITestClassContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            string fileName = string.IsNullOrWhiteSpace(context.TestClassName) ? "test_class.cs" : context.TestClassName + ".cs";

            string testContent = GenerateTestClassFileContent(context);

            File.WriteAllText(fileName, testContent);
        }

        public string GenerateTestClassFileContent(ITestClassContext context)
        {
            return _template.GenerateContent(context);
        }
    }
}
