namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    using System.Reflection;
    using Ark.StepRunner.CustomAttribute;

    [AScenario(description: "test scenario")]
    internal class RunAllSetupsAndBusinnesStepsAndCleanups
    {

        internal enum StepsForRunAllSetupsAndBusinnesStepsAndCleanups
        {
            StepOrSetupOrCleanup1,
            StepOrSetupOrCleanup2,
            StepOrSetupOrCleanup3
            
        }

        private readonly StepTrack<AStepScenarioAttribute> _stepTracker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public RunAllSetupsAndBusinnesStepsAndCleanups(StepTrack<AStepScenarioAttribute> stepTracker)
        {
            _stepTracker = stepTracker;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenario(index: (int)StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup1, description: "RunScenario Method")]
        public void Setup1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenario(index: (int)StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup2, description: "RunScenario Method")]
        public void Setup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenario(index: (int)StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup3, description: "RunScenario Method")]
        public void Setup3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepScenario(index: (int)StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup1, description: "RunScenario Method")]
        public void RunMethod1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepScenario(index: (int)StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [AStepScenario(index: (int)StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup3, description: "RunScenario  15 Method")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepCleanupScenario(index: (int)StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup1, description: "RunScenario Method")]
        public void Cleanup1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepCleanupScenario(index: (int)StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup2, description: "RunScenario Method")]
        public void Cleanup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepCleanupScenario(index: (int)StepsForRunAllSetupsAndBusinnesStepsAndCleanups.StepOrSetupOrCleanup3, description: "RunScenario Method")]
        public void Cleanup3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

    }
}