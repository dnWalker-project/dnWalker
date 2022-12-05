﻿using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter
{
    public interface ITestFramework
    {
        TestProject CreateTestProject(string name);

        // should add the created class into the test group
        // TODO add some context for the test class - necessary to generate name and proper attributes...
        TestClass CreateTestClass(TestProject testProject, TestGroup testGroup);

        // should add the created method into the test class
        // TODO add some context for the test class - necessary to generate name and proper attributes...
        TestMethod CreateTestMethod(TestClass testClass, ITestSchema testSchema);
    }
}
