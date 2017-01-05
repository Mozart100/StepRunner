namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Ark.StepRunner.CustomAttribute;

    [AScenario(description: "test scenario")]
    internal class ScenarioStopRunningAfterException
    {

        internal enum StepsForScenarioStopRunningAfterException
        {
            Step1,
            Step2,
            Step3,
        }

        private readonly StepTrack<int> _stepTracker;
        private readonly NullReferenceException _nullReferenceExceptio;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ScenarioStopRunningAfterException(StepTrack<int> stepTracker, NullReferenceException nullReferenceExceptio)
        {
           
            _stepTracker = stepTracker;
            _nullReferenceExceptio = nullReferenceExceptio;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepScenario(index: (int)StepsForScenarioStopRunningAfterException.Step1, description: "RunScenario Method")]
        public void RunMethod1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepScenario(index: (int)StepsForScenarioStopRunningAfterException.Step2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

            throw _nullReferenceExceptio;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [AStepScenario(index: (int)StepsForScenarioStopRunningAfterException.Step3, description: "RunScenario  15 Method")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);
        }

        
    }
}