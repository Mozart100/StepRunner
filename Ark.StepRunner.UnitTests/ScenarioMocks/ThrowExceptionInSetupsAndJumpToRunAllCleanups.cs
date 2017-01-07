namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    using System.Reflection;
    using Ark.StepRunner.CustomAttribute;

    [AScenario(description: "test scenario")]
    internal class ThrowExceptionInSetupsAndJumpToRunAllCleanups
    {

        internal enum StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups
        {
            Setup1,
            Setup2,
            Setup3,
            BusinessStep1,
            BusinessStep2,
            BusinessStep3,
            Cleanup1,
            Cleanup2,
            Cleanup3,

        }

        private readonly StepTrack<ABusinessStepScenarioAttribute> _stepTracker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ThrowExceptionInSetupsAndJumpToRunAllCleanups(StepTrack<ABusinessStepScenarioAttribute> stepTracker)
        {
            _stepTracker = stepTracker;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenario(index: (int)StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.Setup1, description: "RunScenario Method")]
        public void Setup1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenario(index: (int)StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.Setup2, description: "RunScenario Method")]
        public void Setup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

            throw new System.Exception();

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenario(index: (int)StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.Setup3, description: "RunScenario Method")]
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

        [ABusinessStepScenario(index: (int)StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.BusinessStep1, description: "RunScenario Method")]
        public void RunMethod1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.BusinessStep2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [ABusinessStepScenario(index: (int)StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.BusinessStep3, description: "RunScenario  15 Method")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepCleanupScenario(index: (int)StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.Cleanup1, description: "RunScenario Method")]
        public void Cleanup1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepCleanupScenario(index: (int)StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.Cleanup2, description: "RunScenario Method")]
        public void Cleanup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepCleanupScenario(index: (int)StepsForThrowExceptionInSetupsAndJumpToRunAllCleanups.Cleanup3, description: "RunScenario Method")]
        public void Cleanup3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

    }
}