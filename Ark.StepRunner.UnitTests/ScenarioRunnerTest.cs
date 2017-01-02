using System;
using System.Collections.Generic;
using System.Reflection;
using Ark.StepRunner;
using Ark.StepRunner.CustomAttribute;
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

            Assert.IsFalse(scenarioRunner.RunScenario<ScnearioWithouScenarioAttribute>());
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_IsAStepScenarioAttribute_ReturnFalse()
        {
            var scenarioRunner = new ScenarioRunner();

            Assert.IsFalse(scenarioRunner.RunScenario<ScnearioWithouScenarioAttribute>());
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScenarioRunner_RunOnlyPublicMethodsByOrder_ReturnSolely3Steps()
        {
            var queue = new StepTrack();
            var scenarioRunner = new ScenarioRunner();

            scenarioRunner.RunScenario<ValidScenario>(queue);

            Assert.IsTrue(queue.Queue.Dequeue() == 1);
            Assert.IsTrue(queue.Queue.Dequeue() == 5);
            Assert.IsTrue(queue.Queue.Dequeue() == 15);
            Assert.IsTrue(queue.Queue.Count == 0);
        }
    }

    public class StepTrack
    {
        private Queue<int> _queue;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public StepTrack()
        {
            _queue = new Queue<int>();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public Queue<int> Queue
        {
            get { return _queue; }
            set { _queue = value; }
        }

    }

    [AScenario(description: "test scenario")]
    public class ValidScenario
    {
        private readonly StepTrack _stepTracker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ValidScenario(StepTrack stepTracker)
        {
            _stepTracker = stepTracker;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepScenario(index: 1, description: "RunScenario Method")]
        public void RunMethod()
        {
            //typeof(MyClass).GetRuntimeMethod(nameof(MyClass.MyMethod)), new Type[] { });
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Queue.Enqueue(attribute.Index);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------


        [AStepScenario(index: 5, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Queue.Enqueue(attribute.Index);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [AStepScenario(index: 15, description: "RunScenario  15 Method")]
        public void RunMethod15()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Queue.Enqueue(attribute.Index);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepScenario(index: 20, description: "RunScenario Method")]
        private void RunMethod20()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Queue.Enqueue(attribute.Index);
        }
    }

    [AScenario(description: "test scenario")]
    public class SchenarioWithoutAnyAStepScenarioAttribute
    {
        public SchenarioWithoutAnyAStepScenarioAttribute()
        {

        }

        //[AStepScenarioAttribute(index: 1, description: "RunScenario Method")]
        public void RunMethod()
        {

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        public void RunMethod2()
        {

        }


    }

    public class ScnearioWithouScenarioAttribute
    {
        public ScnearioWithouScenarioAttribute()
        {

        }

        [AStepScenario(index: 1, description: "RunScenario Method")]
        public void RunMethod()
        {

        }
    }


}





