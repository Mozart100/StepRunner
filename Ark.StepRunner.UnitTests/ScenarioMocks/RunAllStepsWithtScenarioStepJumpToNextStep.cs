using System.Reflection;
using Ark.StepRunner.CustomAttribute;
using Ark.StepRunner.ScenarioStepResult;

namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    [AScenario(description: "test scenario")]
    internal class RunAllStepsWithtScenarioStepJumpToNextStep
    {
        internal enum ScenarioSteps
        {
            Step1,
            Step2,
            Step3,
            Step4

        }

        private readonly StepTrack<int> _stepTracker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public RunAllStepsWithtScenarioStepJumpToNextStep(StepTrack<int> stepTracker)
        {
            _stepTracker = stepTracker;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)ScenarioSteps.Step1, description: "RunScenario Method")]
        public ScenarioStepReturnBase RunMethod()
        {
            //typeof(MyClass).GetRuntimeMethod(nameof(MyClass.MyMethod)), new Type[] { });
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);
            return new ScenarioStepJumpToStep((int) ScenarioSteps.Step3);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------


        [ABusinessStepScenario(index: (int)ScenarioSteps.Step2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [ABusinessStepScenario(index: (int)ScenarioSteps.Step3, description: "RunScenario  15 Method")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        
    }
}