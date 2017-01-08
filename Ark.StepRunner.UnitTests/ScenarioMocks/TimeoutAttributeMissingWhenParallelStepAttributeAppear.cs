namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    using System.Reflection;
    using Ark.StepRunner.CustomAttribute;

    [AScenario(description: "test scenario")]
    internal class TimeoutAttributeMissingWhenParallelStepAttributeAppear
    {

        internal enum StepsForScenario
        {
            SetupStep1,
            SetupStep2,
            SetupStep3,
            BusinessStep1,
            BusinessStep2,
            BusinessStep3

        }

        private readonly StepTrack<ABusinessStepScenarioAttribute> _stepTracker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public TimeoutAttributeMissingWhenParallelStepAttributeAppear(StepTrack<ABusinessStepScenarioAttribute> stepTracker)
        {
            _stepTracker = stepTracker;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenario(index: (int)StepsForScenario.SetupStep1, description: "RunScenario Method")]
        public void Setup1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        [AScenarioStepTimeout(seconds: 20)]
        [AScenarioStepParallelAttribute]
        [AStepSetupScenario(index: (int)StepsForScenario.SetupStep2, description: "RunScenario Method")]
        public void Setup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AScenarioStepParallelAttribute]
        [AStepSetupScenario(index: (int)StepsForScenario.SetupStep3, description: "RunScenario Method")]
        public void Setup3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)StepsForScenario.BusinessStep1, description: "RunScenario Method")]
        public void RunMethod1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)StepsForScenario.BusinessStep2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [ABusinessStepScenario(index: (int)StepsForScenario.BusinessStep3, description: "RunScenario  15 Method")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

    }
}