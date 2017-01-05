using System;
using System.Collections.Generic;
using Ark.StepRunner;
using Ark.StepRunner.UnitTests.ScenarioMocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ark.StepRunner.UnitTests
{
    [TestClass]
    public class ScenarioRunnerTest
    {
        [TestMethod]
        public void ScenarioRunner_IsAScenarioAttributeDefine_ReturnFalse()
        {
            var scenarioRunner = new ScenarioRunner();

            Assert.IsFalse(scenarioRunner.RunScenario<ScnearioWithoutScenarioAttribute>().IsSuccessful);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_IsAStepScenarioAttribute_ReturnFalse()
        {
            var scenarioRunner = new ScenarioRunner();

            Assert.IsFalse(scenarioRunner.RunScenario<ScnearioWithoutScenarioAttribute>().IsSuccessful);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunOnlyPublicMethodsByOrder_ReturnSolely3Steps()
        {
            const int numberScenarioStepInvoked = 3;

            //--------------------------------------------------------------------------------------------------------------------------------------

            var queue = new StepTrack<int>();
            var scenarioRunner = new ScenarioRunner();

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
            var scenarioRunner = new ScenarioRunner();

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
            var scenarioRunner = new ScenarioRunner();

            var result = scenarioRunner.RunScenario<RunAllStepsAndPassingParametersBetweenSteps>(queue , 1,"111",2,"222");

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(numberScenarioStepInvoked, result.NumberScenarioStepInvoked);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step1);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step2);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step3);
            Assert.IsTrue(queue.Dequeue() == (int)RunAllStepsAndPassingParametersBetweenSteps.StepsForRunAllStepsAndPassingParametersBetweenSteps.Step4);


        }

    }

    public class StepTrack<TType>
    {
        private readonly Queue<TType> _queue;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public StepTrack()
        {
            _queue = new Queue<TType>();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public TType Dequeue()
        {
             return _queue.Dequeue(); 
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        public void Enqueue(TType item)
        {
              _queue.Enqueue(item); 
        }

        //--------------------------------------------------------------------------------------------------------------------------------------    

    }
}





