using System.Reflection;
using Ark.StepRunner.CustomAttribute;

namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    [AScenario(description: "test scenario")]
    internal class RunAllStepsWithoutScenarioStepResult
    {

        internal enum StepsForRunAllStepsWithoutScenarioStepResult
        {
            Step1,
            Step2,
            Step3,
            Step4
        }

        private readonly StepTrack<int> _stepTracker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public RunAllStepsWithoutScenarioStepResult(StepTrack<int> stepTracker)
        {
            _stepTracker = stepTracker;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)StepsForRunAllStepsWithoutScenarioStepResult.Step1, description: "RunScenario Method")]
        public void RunMethod1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)StepsForRunAllStepsWithoutScenarioStepResult.Step2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [ABusinessStepScenario(index: (int)StepsForRunAllStepsWithoutScenarioStepResult.Step3, description: "RunScenario  15 Method")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        [ABusinessStepScenario(index: (int)StepsForRunAllStepsWithoutScenarioStepResult.Step4, description: "RunScenario Method")]
        private void RunMethod4()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);
        }
    }
}