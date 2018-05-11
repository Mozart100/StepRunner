using System;
using Ark.StepRunner.UnitTests.ScenarioMocks;
using Ark.StepRunner.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using Moq;

namespace Ark.StepRunner.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Ark.StepRunner.CustomAttribute;
    using Autofac;
    using Serilog;
    using Serilog.Core;
    using Serilog.Events;

    [TestClass]
    public class ScenarioRunnerTest
    {

        private Mock<ILogger> _logger;
        private ContainerBuilder _containerBuilder;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestInitialize]
        public void TestInitialize()
        {
            _logger = new Mock<ILogger>();
            _containerBuilder = new ContainerBuilder();

        }


        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_IsAScenarioAttributeDefine_ReturnFalse()
        {
            var scenarioRunner = new ScenarioRunner(_logger.Object, _containerBuilder.Build());

            Assert.IsFalse(scenarioRunner.RunScenario<ScnearioWithoutScenarioAttribute>().IsSuccessful);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_IsAStepScenarioAttribute_ReturnFalse()
        {
            var scenarioRunner = new ScenarioRunner(_logger.Object, _containerBuilder.Build());

            Assert.IsFalse(scenarioRunner.RunScenario<ScnearioWithoutScenarioAttribute>().IsSuccessful);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunOnlyPublicMethodsByOrder_ReturnSolely3Steps()
        {
            const int numberScenarioStepInvoked = 3;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();

            _containerBuilder.Register(x => queue).As<StepTrack<int>>();
            _containerBuilder.RegisterType<RunAllStepsWithoutScenarioStepResult>();


            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(_logger.Object, container);
            var result = scenarioRunner.RunScenario<RunAllStepsWithoutScenarioStepResult>();

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithoutScenarioStepResult.ScenarioSteps.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithoutScenarioStepResult.ScenarioSteps.Step2);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithoutScenarioStepResult.ScenarioSteps.Step3);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunScenarioWithScenarioStepJumpToNextStep()
        {
            const int numberScenarioStepInvoked = 2;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();

            _containerBuilder.Register(x => queue).As<StepTrack<int>>();
            _containerBuilder.RegisterType<RunAllStepsWithtScenarioStepJumpToNextStep>();


            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(_logger.Object, container);

            var result = scenarioRunner.RunScenario<RunAllStepsWithtScenarioStepJumpToNextStep>();

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithtScenarioStepJumpToNextStep.ScenarioSteps.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithtScenarioStepJumpToNextStep.ScenarioSteps.Step3);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllStepsAndPassinParametersBetweenWithAutofacSteps()
        {
            const int numberScenarioStepInvoked = 4;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();

            _containerBuilder.Register(x => queue).As<StepTrack<int>>();
            _containerBuilder.RegisterType<RunAllStepsAndPassingParametersBetweenSteps>()
                .WithParameter("magicNumForStep2", 1)
                .WithParameter("magicStringForStep2", "111")
                .WithParameter("magicNumForStep4", 2)
                .WithParameter("magicStringForStep4", "222");


            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(_logger.Object, container);

            var result = scenarioRunner.RunScenario<RunAllStepsAndPassingParametersBetweenSteps>();


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step2);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step3);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step4);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllStepsAndPassinParametersBetweenSteps()
        {
            const int numberScenarioStepInvoked = 4;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();

            _containerBuilder.Register(x => queue).As<StepTrack<int>>();
            _containerBuilder.RegisterType<RunAllStepsAndPassingParametersBetweenSteps>();


            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(_logger.Object, container);

            var result = scenarioRunner.RunScenarioWithParameters<RunAllStepsAndPassingParametersBetweenSteps>(1, "111", 2, "222");


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step2);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step3);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step4);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [TestMethod]
        public void ScenarioRunner_RunAllStepsWithATimeoutException()
        {
            const int numberScenarioStepInvoked = 2;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();

            _containerBuilder.Register(x => queue).As<StepTrack<int>>();
            _containerBuilder.RegisterType<RunStepsAndFaileDueToTimeout>();


            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(_logger.Object, container);

            var result = scenarioRunner.RunScenario<RunStepsAndFaileDueToTimeout>();

            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.Exceptions.First() is AScenarioStepTimeoutException);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step2);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_StopRunningWhenExceptionOccure()
        {
            const int numberScenarioStepInvoked = 2;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var expectedNullReferenceException = new NullReferenceException();

            var queue = new StepTrack<int>();

            _containerBuilder.Register(x => queue).As<StepTrack<int>>();
            _containerBuilder.RegisterType<ScenarioStopRunningAfterException>()
                .WithParameter("nullReferenceExceptio", expectedNullReferenceException);

            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(_logger.Object, container);

            var result = scenarioRunner.RunScenario<ScenarioStopRunningAfterException>();

            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.Exceptions.First() is NullReferenceException);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.ScenarioSteps.Step2);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_Logger_TracesPublishingIntoLogger()
        {
            const int numberScenarioStepInvoked = 2;
            var testLogger = new TestLogger();
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();

            _containerBuilder.Register(x => queue).As<StepTrack<int>>();
            _containerBuilder.RegisterType<RunAllStepsWithtScenarioStepJumpToNextStep>();


            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(testLogger, container);

            var result = scenarioRunner.RunScenario<RunAllStepsWithtScenarioStepJumpToNextStep>();


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithtScenarioStepJumpToNextStep.ScenarioSteps.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithtScenarioStepJumpToNextStep.ScenarioSteps.Step3);
            Assert.IsTrue(testLogger.Queue.ToArray().Length > 1);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [TestMethod]
        public void ScenarioRunner_RunAllSetupsAndBusinessSteps_NoCleanupsSteps()
        {
            const int numberScenarioStepInvoked = 6;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();

            _containerBuilder.Register(x => queue).As<StepTrack<ABusinessStepScenarioAttribute>>();
            _containerBuilder.RegisterType<RunAllSetupsAndBusinnesSteps>();


            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(_logger.Object, container);

            var result = scenarioRunner.RunScenario<RunAllSetupsAndBusinnesSteps>();


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.ScenarioSteps.SetupStep1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.ScenarioSteps.SetupStep2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.ScenarioSteps.SetupStep3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.ScenarioSteps.BusinessStep1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.ScenarioSteps.BusinessStep2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.ScenarioSteps.BusinessStep3);


        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllSetupsAndBusinessStepsAndCleanups()
        {
            const int numberScenarioStepInvoked = 9;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            _containerBuilder.Register(x => queue).As<StepTrack<ABusinessStepScenarioAttribute>>();
            _containerBuilder.RegisterType<RunAllSetupsAndBusinnesStepsAndCleanups>();


            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(_logger.Object, container);

            var result = scenarioRunner.RunScenario<RunAllSetupsAndBusinnesStepsAndCleanups>();


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.ScenarioSteps.Setup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.ScenarioSteps.Setup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.ScenarioSteps.Setup3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.ScenarioSteps.BusinessStep1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.ScenarioSteps.BusinessStep2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.ScenarioSteps.BusinessStep3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.ScenarioSteps.Cleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.ScenarioSteps.Cleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.ScenarioSteps.Cleanup3);


        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllSetupsAnd2BusinessStepsAndAllCleanups_BusinessStepThrowException()
        {
            const int numberScenarioStepInvoked = 8;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            _containerBuilder.Register(x => queue).As<StepTrack<ABusinessStepScenarioAttribute>>();
            _containerBuilder.RegisterType<RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups>();

            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(_logger.Object, container);

            var result = scenarioRunner.RunScenario<RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups>();


            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            //Setups
            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.ScenarioSteps.StepOrSetupOrCleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.ScenarioSteps.StepOrSetupOrCleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.ScenarioSteps.StepOrSetupOrCleanup3);

            //--------------------------------------------------------------------------------------------------------------------------------------
            //BusinessSteps
            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.ScenarioSteps.StepOrSetupOrCleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.ScenarioSteps.StepOrSetupOrCleanup2);

            //--------------------------------------------------------------------------------------------------------------------------------------
            //Cleanups

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.ScenarioSteps.StepOrSetupOrCleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.ScenarioSteps.StepOrSetupOrCleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.ScenarioSteps.StepOrSetupOrCleanup3);


        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllSetupsUntilExceptionAndJumpToRunAndAllCleanups_SetupThrowException()
        {
            const int numberScenarioStepInvoked = 5;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            _containerBuilder.Register(x => queue).As<StepTrack<ABusinessStepScenarioAttribute>>();
            _containerBuilder.RegisterType<ThrowExceptionInSetupsAndJumpToRunAllCleanups>();


            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(_logger.Object, container);

            var result = scenarioRunner.RunScenario<ThrowExceptionInSetupsAndJumpToRunAllCleanups>();


            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            //Setups
            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)ThrowExceptionInSetupsAndJumpToRunAllCleanups.ScenarioSteps.Setup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)ThrowExceptionInSetupsAndJumpToRunAllCleanups.ScenarioSteps.Setup2);


            //--------------------------------------------------------------------------------------------------------------------------------------

            //Cleanups

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)ThrowExceptionInSetupsAndJumpToRunAllCleanups.ScenarioSteps.Cleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)ThrowExceptionInSetupsAndJumpToRunAllCleanups.ScenarioSteps.Cleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)ThrowExceptionInSetupsAndJumpToRunAllCleanups.ScenarioSteps.Cleanup3);


        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllSetupAndBusinessStepsAndAllCleanups_SetupAndBusinessAndCleanThrowException()
        {
            const int numberScenarioStepInvoked = 9;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            _containerBuilder.Register(x => queue).As<StepTrack<ABusinessStepScenarioAttribute>>();
            _containerBuilder.RegisterType<RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException>();


            IContainer container = _containerBuilder.Build();

            var scenarioRunner = new ScenarioRunner(_logger.Object, container);

            var result = scenarioRunner.RunScenario<RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException>();


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.ScenarioSteps.Setup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.ScenarioSteps.Setup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.ScenarioSteps.Setup3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.ScenarioSteps.BusinessStep1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.ScenarioSteps.BusinessStep2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.ScenarioSteps.BusinessStep3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.ScenarioSteps.Cleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.ScenarioSteps.Cleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.ScenarioSteps.Cleanup3);


        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        //Paralellisam

        //[TestMethod]
        //public void ScenarioRunner_AStepScenarioParallelAttributeShouldBeWithTimeoutAttribute_ThrowExceptionWithoutTimeout()
        //{
        //    const int numberScenarioStepInvoked = 0;
        //    //--------------------------------------------------------------------------------------------------------------------------------------

        //    var queue = new StepTrack<ABusinessStepScenarioAttribute>();
        //    _containerBuilder.Register(x => queue).As<StepTrack<ABusinessStepScenarioAttribute>>();
        //    _containerBuilder.RegisterType<TimeoutAttributeMissingWhenParallelStepAttributeAppear>();


        //    IContainer container = _containerBuilder.Build();

        //    var scenarioRunner = new ScenarioRunner(_logger.Object, container);

        //    var result = scenarioRunner.RunScenario<TimeoutAttributeMissingWhenParallelStepAttributeAppear>();


        //    Assert.IsFalse(result.IsSuccessful);
        //    Assert.IsTrue(result.Exceptions.First() is AScenarioStepTimeoutAttributeMissinigException);
        //    Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

        //}

        ////--------------------------------------------------------------------------------------------------------------------------------------
        //[TestMethod]
        //public void ScenarioRunner_RunAllStepsParallel()
        //{
        //    const int numberScenarioStepInvoked = 9;
        //    //--------------------------------------------------------------------------------------------------------------------------------------
        //    Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        //    var queue = new StepTrack<ABusinessStepScenarioAttribute>();
        //    _containerBuilder.Register(x => queue).As<StepTrack<ABusinessStepScenarioAttribute>>();
        //    _containerBuilder.RegisterType<RunAllStepParallel>();


        //    IContainer container = _containerBuilder.Build();

        //    var scenarioRunner = new ScenarioRunner(_logger.Object, container);

        //    var result = scenarioRunner.RunScenario<RunAllStepParallel>();

        //    var index = 0;
        //    //Assert.IsTrue(result.IsSuccessful,(++index).ToString());
        //    Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

        //    //Setups
        //    var list = queue.ToList();
        //    var dictionary = new HashSet<int>();
        //    for (int i = 8; i > 0; i--)
        //    {

        //        Console.WriteLine("[{0}] Number is = [{1}]", i, list.ElementAt(i).Index);


        //    }
        //    //return;
        //    //cleaup
        //    for (int i = 6; i < 9; i++)
        //    {

        //        Console.WriteLine("number is = [{0}]", list.ElementAt(i).Index);
        //        if ((list.ElementAt(i).Index < 6) || (list.ElementAt(i).Index > 9))
        //        {
        //            throw new Exception("Should be between 6 and 9.");
        //        }

        //        Assert.IsTrue(dictionary.Add(list.ElementAt(i).Index));

        //    }

        //    //Business
        //    for (int i = 6; i < 3; i--)
        //    {
        //        Console.WriteLine("number is = [{0}]", list.ElementAt(i).Index);
        //        if ((list.ElementAt(i).Index < 3) || (list.ElementAt(i).Index > 6))
        //        {
        //            throw new Exception("Should be between 3 and 6.");
        //        }


        //        Assert.IsTrue(dictionary.Add(list.ElementAt(i).Index));

        //    }

        //    //Setup.
        //    for (int i = 0; i < 3; i++)
        //    {
        //        if (list.ElementAt(i).Index > 3)
        //        {
        //            throw new Exception("Should be less then 3");
        //        }


        //        Assert.IsTrue(dictionary.Add(list.ElementAt(i).Index));
        //    }


        //}

        ////--------------------------------------------------------------------------------------------------------------------------------------

        //[TestMethod]
        //public void ScenarioRunner_ParallelTimeout_ThrowTimeoutExceptionAndGoToCleanups()
        //{
        //    var tmp = 5 | 6;
        //    const int numberScenarioStepInvoked = 6;

        //    //--------------------------------------------------------------------------------------------------------------------------------------

        //    var queue = new StepTrack<ABusinessStepScenarioAttribute>();
        //    _containerBuilder.Register(x => queue).As<StepTrack<ABusinessStepScenarioAttribute>>();
        //    _containerBuilder.RegisterType<RunAllStepParallelAndInSetupThrowTimeoutException>();


        //    IContainer container = _containerBuilder.Build();
        //    var scenarioRunner = new ScenarioRunner(_logger.Object, container);

        //    var result = scenarioRunner.RunScenario<RunAllStepParallelAndInSetupThrowTimeoutException>();

        //    Assert.IsFalse(result.IsSuccessful);
        //    Assert.IsTrue(result.Exceptions.First() is AScenarioStepTimeoutException);
        //    Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

        //    //Setups
        //    var list = queue.ToList();
        //    var dictionary = new HashSet<int>();

        //    //cleaup
        //    for (int i = 3; i < 5; i++)
        //    {

        //        Console.WriteLine("number is = [{0}]", list.ElementAt(i).Index);
        //        if ((list.ElementAt(i).Index < 6) || (list.ElementAt(i).Index > 9))
        //        {
        //            throw new Exception("Should be between 6 and 9.");
        //        }

        //        Assert.IsTrue(dictionary.Add(list.ElementAt(i).Index));
        //    }

        //    //Setup.
        //    for (int i = 0; i < 3; i++)
        //    {
        //        if (list.ElementAt(i).Index > 3)
        //        {
        //            throw new Exception("Should be less then 3");
        //        }


        //        Assert.IsTrue(dictionary.Add(list.ElementAt(i).Index));
        //    }


        //}
    }


    internal class TestLogger : ILogger
    {
        private readonly ConcurrentQueue<string> _queue;
        private object _locker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public TestLogger()
        {
            _queue = new ConcurrentQueue<string>();
            _locker = new object();
        }

        public ConcurrentQueue<string> Queue => _queue;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public void Debug(string message)
        {
            _queue.Enqueue(message);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        public void Log(string message)
        {
            _queue.Enqueue(message);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        public void Error(string message)
        {
            _queue.Enqueue(message);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        public void Warning(string message)
        {
            _queue.Enqueue(message);
        }

        public ILogger ForContext(ILogEventEnricher enricher)
        {
            throw new NotImplementedException();
        }

        public ILogger ForContext(IEnumerable<ILogEventEnricher> enrichers)
        {
            throw new NotImplementedException();
        }

        public ILogger ForContext(string propertyName, object value, bool destructureObjects = false)
        {
            throw new NotImplementedException();
        }

        public ILogger ForContext<TSource>()
        {
            throw new NotImplementedException();
        }

        public ILogger ForContext(Type source)
        {
            throw new NotImplementedException();
        }

        public void Write(LogEvent logEvent)
        {
            throw new NotImplementedException();
        }

        public void Write(LogEventLevel level, string messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void Write<T>(LogEventLevel level, string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Write<T0, T1>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Write<T0, T1, T2>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Write(LogEventLevel level, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Write(LogEventLevel level, Exception exception, string messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void Write<T>(LogEventLevel level, Exception exception, string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Write<T0, T1>(LogEventLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Write<T0, T1, T2>(LogEventLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Write(LogEventLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogEventLevel level)
        {
            throw new NotImplementedException();
        }

        public void Verbose(string messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void Verbose<T>(string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Verbose(Exception exception, string messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void Verbose<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Verbose<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Verbose<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Debug<T>(string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            _queue.Enqueue(messageTemplate);
        }

        public void Debug(Exception exception, string messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void Debug<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Information(string messageTemplate)
        {
            _queue.Enqueue(messageTemplate);
        }

        public void Information<T>(string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Information(Exception exception, string messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void Information<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Warning<T>(string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Warning(Exception exception, string messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void Warning<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Warning<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Warning<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Error<T>(string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            _queue.Enqueue(messageTemplate);

        }

        public void Error(Exception exception, string messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void Fatal<T>(string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception exception, string messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            throw new NotImplementedException();
        }

        public void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            throw new NotImplementedException();
        }

        public void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public bool BindMessageTemplate(string messageTemplate, object[] propertyValues, out MessageTemplate parsedTemplate, out IEnumerable<LogEventProperty> boundProperties)
        {
            throw new NotImplementedException();
        }

        public bool BindProperty(string propertyName, object value, bool destructureObjects, out LogEventProperty property)
        {
            throw new NotImplementedException();
        }
    }
}





