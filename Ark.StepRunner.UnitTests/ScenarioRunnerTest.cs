using System;
using Ark.StepRunner.UnitTests.ScenarioMocks;
using Ark.StepRunner.Exceptions;
using Ark.StepRunner.TraceLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
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

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunAllSetupsAndBusinnesSteps>(queue);


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.SetupStep1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.SetupStep2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.SetupStep3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.BusinessStep1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.BusinessStep2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesSteps.StepsForRunAllSetupsAndBusinnesSteps.BusinessStep3);


        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllSetupsAndBusinessStepsAndCleanups()
        {
            const int numberScenarioStepInvoked = 9;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunAllSetupsAndBusinnesStepsAndCleanups>(queue);


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.Setup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.Setup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.Setup3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.BusinessStep1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.BusinessStep2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.BusinessStep3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.Cleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.Cleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndBusinnesStepsAndCleanups.StepsForRunAllSetupsAndBusinnesStepsAndCleanups.Cleanup3);


        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllSetupsAnd2BusinessStepsAndAllCleanups_BusinessStepThrowException()
        {
            const int numberScenarioStepInvoked = 8;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups>(queue);


            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            //Setups
            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepsForRunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepOrSetupOrCleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepsForRunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepOrSetupOrCleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepsForRunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepOrSetupOrCleanup3);

            //--------------------------------------------------------------------------------------------------------------------------------------
            //BusinessSteps
            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepsForRunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepOrSetupOrCleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepsForRunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepOrSetupOrCleanup2);

            //--------------------------------------------------------------------------------------------------------------------------------------
            //Cleanups

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepsForRunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepOrSetupOrCleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepsForRunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepOrSetupOrCleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepsForRunAllSetupsAndInBusinnesStepsUntilExceptionOccureAndRunAllCleanups.StepOrSetupOrCleanup3);


        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllSetupsUntilExceptionAndJumpToRunAndAllCleanups_SetupThrowException()
        {
            const int numberScenarioStepInvoked = 5;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<ThrowExceptionInSetupsAndJumpToRunAllCleanups>(queue);


            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            //Setups
            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)ThrowExceptionInSetupsAndJumpToRunAllCleanups.StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.Setup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)ThrowExceptionInSetupsAndJumpToRunAllCleanups.StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.Setup2);


            //--------------------------------------------------------------------------------------------------------------------------------------

            //Cleanups

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)ThrowExceptionInSetupsAndJumpToRunAllCleanups.StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.Cleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)ThrowExceptionInSetupsAndJumpToRunAllCleanups.StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.Cleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)ThrowExceptionInSetupsAndJumpToRunAllCleanups.StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.Cleanup3);


        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunAllSetupAndBusinessStepsAndAllCleanups_SetupAndBusinessAndCleanThrowException()
        {
            const int numberScenarioStepInvoked = 9;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException>(queue);


            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            var attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Setup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Setup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Setup3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.BusinessStep1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.BusinessStep2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.BusinessStep3);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Cleanup1);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Cleanup2);

            attribute = queue.Dequeue();
            Assert.IsTrue(attribute.Index == (int)RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Cleanup3);


        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        //Paralellisam

        [TestMethod]
        public void ScenarioRunner_AStepScenarioParallelAttributeShouldBeWithTimeoutAttribute_ThrowExceptionWithoutTimeout()
        {
            const int numberScenarioStepInvoked = 0;
            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<TimeoutAttributeMissingWhenParallelStepAttributeAppear>(queue);


            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.Exceptions.First() is AScenarioStepTimeoutAttributeMissinigException);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void ScenarioRunner_RunAllStepsParallel()
        {
            const int numberScenarioStepInvoked = 9;
            //--------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunAllStepParallel>(queue);

            var index = 0;
            //Assert.IsTrue(result.IsSuccessful,(++index).ToString());
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            //Setups
            var list = queue.ToList();
            var dictionary = new HashSet<int>();
            for (int i = 8; i > 0; i--)
            {

                Console.WriteLine("[{0}] Number is = [{1}]", i, list.ElementAt(i).Index);


            }
            //return;
            //cleaup
            for (int i = 6; i < 9; i++)
            {

                Console.WriteLine("number is = [{0}]", list.ElementAt(i).Index);
                if ((list.ElementAt(i).Index < 6) || (list.ElementAt(i).Index > 9))
                {
                    throw new Exception("Should be between 6 and 9.");
                }

                Assert.IsTrue(dictionary.Add(list.ElementAt(i).Index));

            }

            //Business
            for (int i = 6; i < 3; i--)
            {
                Console.WriteLine("number is = [{0}]", list.ElementAt(i).Index);
                if ((list.ElementAt(i).Index < 3) || (list.ElementAt(i).Index > 6))
                {
                    throw new Exception("Should be between 3 and 6.");
                }


                Assert.IsTrue(dictionary.Add(list.ElementAt(i).Index));

            }

            //Setup.
            for (int i = 0; i < 3; i++)
            {
                if (list.ElementAt(i).Index > 3)
                {
                    throw new Exception("Should be less then 3");
                }


                Assert.IsTrue(dictionary.Add(list.ElementAt(i).Index));
            }


        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [Ignore]
        [TestMethod]
        public void ScenarioRunner_ParallelTimeout_ThrowTimeoutExceptionAndGoToCleanups()
        {
            const int numberScenarioStepInvoked = 9;
            //--------------------------------------------------------------------------------------------------------------------------------------
            //Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

            var queue = new StepTrack<ABusinessStepScenarioAttribute>();
            var scenarioRunner = new ScenarioRunner(_publisherLogger.Object);

            var result = scenarioRunner.RunScenario<RunAllStepParallel>(queue);

            var index = 0;
            //Assert.IsTrue(result.IsSuccessful,(++index).ToString());
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);

            //Setups
            var list = queue.ToList();
            var dictionary = new HashSet<int>();
            for (int i = 8; i > 0; i--)
            {

                Console.WriteLine("[{0}] Number is = [{1}]", i, list.ElementAt(i).Index);


            }
            //return;
            //cleaup
            for (int i = 6; i < 9; i++)
            {

                Console.WriteLine("number is = [{0}]", list.ElementAt(i).Index);
                if ((list.ElementAt(i).Index < 6) || (list.ElementAt(i).Index > 9))
                {
                    throw new Exception("Should be between 6 and 9.");
                }

                Assert.IsTrue(dictionary.Add(list.ElementAt(i).Index));
            }

            //Business
            for (int i = 6; i < 3; i--)
            {
                Console.WriteLine("number is = [{0}]", list.ElementAt(i).Index);
                if ((list.ElementAt(i).Index < 3) || (list.ElementAt(i).Index > 6))
                {
                    throw new Exception("Should be between 3 and 6.");
                }


                Assert.IsTrue(dictionary.Add(list.ElementAt(i).Index));

            }

            //Setup.
            for (int i = 0; i < 3; i++)
            {
                if (list.ElementAt(i).Index > 3)
                {
                    throw new Exception("Should be less then 3");
                }


                Assert.IsTrue(dictionary.Add(list.ElementAt(i).Index));
            }


        }
    }


    internal class TestLogger : IStepPublisherLogger
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
    }
}





