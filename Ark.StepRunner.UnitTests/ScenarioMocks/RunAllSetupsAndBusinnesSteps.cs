namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    using System.Reflection;
    using Ark.StepRunner.CustomAttribute;

    [AScenario(description: "test scenario")]
    internal class RunAllSetupsAndBusinnesSteps
    {

        internal enum StepsForRunAllSetupsAndBusinnesSteps
        {
            StepOrSetup1,
            StepOrSetup2,
            StepOrSetup3
            
        }

        private readonly StepTrack<AStepScenarioAttribute> _stepTracker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public RunAllSetupsAndBusinnesSteps(StepTrack<AStepScenarioAttribute> stepTracker)
        {
            _stepTracker = stepTracker;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenarioAttribute(index: (int)StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup1, description: "RunScenario Method")]
        public void Setup1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenarioAttribute(index: (int)StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup2, description: "RunScenario Method")]
        public void Setup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenarioAttribute(index: (int)StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup3, description: "RunScenario Method")]
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

        [AStepScenario(index: (int)StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup1, description: "RunScenario Method")]
        public void RunMethod1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepScenario(index: (int)StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [AStepScenario(index: (int)StepsForRunAllSetupsAndBusinnesSteps.StepOrSetup3, description: "RunScenario  15 Method")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (AStepScenarioAttribute)method.GetCustomAttributes(typeof(AStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
       
    }
}