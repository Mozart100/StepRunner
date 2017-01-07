namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    using System.Reflection;
    using Ark.StepRunner.CustomAttribute;

    [AScenario(description: "test scenario")]
    internal class ScenarioWithNotNullAttributeInConstructor
    {

        internal enum StepsForScenarioWithNotNullAttributeInConstructor
        {
            Step1,
            Step2,
            Step3,
        }

        private readonly StepTrack<int> _stepTracker;
        private readonly string _magicString;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ScenarioWithNotNullAttributeInConstructor(StepTrack<int> stepTracker, [NotNull] string magicString)
        {
            _stepTracker = stepTracker;
            _magicString = magicString;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)StepsForScenarioWithNotNullAttributeInConstructor.Step1, description: "RunScenario Method")]
        public void RunMethod1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)StepsForScenarioWithNotNullAttributeInConstructor.Step2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [ABusinessStepScenario(index: (int)StepsForScenarioWithNotNullAttributeInConstructor.Step3, description: "RunScenario  15 Method")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);
        }

        
    }
}