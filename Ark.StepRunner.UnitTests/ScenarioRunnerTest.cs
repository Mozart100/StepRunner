using System;
using Ark.StepRunner.UnitTests.ScenarioMocks;
using Ark.StepRunner.Exceptions;
using Ark.StepRunner.TraceLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ark.StepRunner.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Ark.StepRunner.CustomAttribute;


    [TestClass]
    public class ScenarioRunnerTest
    {

        private Mock<IStepPublisherLogger> _publisherLogger;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestInitialize]
        public void TestInitialize()
        {
            _publisherLogger = new Mock<IStepPublisherLogger>();
        }


        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_IsAScenarioAttributeDefine_ReturnFalse()
        {
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            Assert.IsFalse(scenarioRunner.RunScenario<ScnearioWithoutScenarioAttribute>().IsSuccessful);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_IsAStepScenarioAttribute_ReturnFalse()
        {
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            Assert.IsFalse(scenarioRunner.RunScenario<ScnearioWithoutScenarioAttribute>().IsSuccessful);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunOnlyPublicMethodsByOrder_ReturnSolely3Steps()
        {
            const int numberScenarioStepInvoked = 3;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunAllStepsWithoutScenarioStepResult>(queue);

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithoutScenarioStepResult.StepsForRunAllStepsWithoutScenarioStepResult.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithoutScenarioStepResult.StepsForRunAllStepsWithoutScenarioStepResult.Step2);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithoutScenarioStepResult.StepsForRunAllStepsWithoutScenarioStepResult.Step3);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunScenarioWithScenarioStepJumpToNextStep()
        {
            const int numberScenarioStepInvoked = 2;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunAllStepsWithtScenarioStepJumpToNextStep>(queue);

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithtScenarioStepJumpToNextStep.StepsForRunAllStepsWithtScenarioStepJumpToNextStep.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithtScenarioStepJumpToNextStep.StepsForRunAllStepsWithtScenarioStepJumpToNextStep.Step3);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllStepsAndPassinParametersBetweenSteps()
        {
            const int numberScenarioStepInvoked = 4;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunAllStepsAndPassingParametersBetweenSteps>(queue, 1, "111", 2, "222");

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step2);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step3);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step4);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [TestMethod]
        public void ScenarioRunner_RunAllStepsWithATimeoutException()
        {
            const int numberScenarioStepInvoked = 2;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunStepsAndFaileDueToTimeout>(queue);

            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.Exceptions.First() is AScenarioStepTimeoutException);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step2);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_StopRunningWhenExceptionOccure()
        {
            const int numberScenarioStepInvoked = 2;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var expectedNullReferenceException = new NullReferenceException();

            var queue = new StepTrack<int>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<ScenarioStopRunningAfterException>(queue, expectedNullReferenceException);

            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.Exceptions.First() is NullReferenceException);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step2);
        }


        [TestMethod]
        public void ScenarioRunner_ValidateNotNullAttribute_ReturnExceptionNotNull()
        {
            const int numberScenarioStepInvoked = 0;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);
            string magicString = null;

            var result = scenarioRunner.RunScenario<ScenarioWithNotNullAttributeInConstructor>(queue, magicString);
            var exception = result.Exceptions.First() as AScenarioConstructorParameterNullException;
            Assert.IsTrue(exception.ParameterName == "magicString");
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_Logger_TracesPublishingIntoLogger()
        {
            const int numberScenarioStepInvoked = 2;
            var testLogger = new TestLogger();
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();
            var scenarioRunner = new ScenarioRunner(testLogger);

            var result = scenarioRunner.RunScenario<RunAllStepsWithtScenarioStepJumpToNextStep>(queue);


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithtScenarioStepJumpToNextStep.StepsForRunAllStepsWithtScenarioStepJumpToNextStep.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsWithtScenarioStepJumpToNextStep.StepsForRunAllStepsWithtScenarioStepJumpToNextStep.Step3);
            Assert.IsTrue(testLogger.Queue.ToArray().Length > 1);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [TestMethod]
        public void ScenarioRunner_RunAllSetupsAndBusinessSteps_NoCleanupsSteps()
        {
            const int numberScenarioStepInvoked = 6;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<AStepScenarioAttribute>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunAllSetupsAndBusinnesSteps>(queue);


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup3);


        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllSetupsAndBusinessStepsAndCleanups()
        {
            const int numberScenarioStepInvoked = 9;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<AStepScenarioAttribute>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunAllSetupsAndBusinnesStepsAndCleanups>(queue);


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup3);


        }

    }


    internal class TestLogger : IStepPublisherLogger
    {
        private readonly Queue<string> _queue;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public TestLogger()
        {
            _queue = new Queue<string>();
        }

        public Queue<string> Queue => _queue;

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
    }
}





