namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    using System.Reflection;
    using Ark.StepRunner.CustomAttribute;

    [AScenario(description: "test scenario")]
    internal class RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException
    {

        internal enum ScenarioSteps
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

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        private readonly StepTrack<ABusinessStepScenarioAttribute> _stepTracker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException(StepTrack<ABusinessStepScenarioAttribute> stepTracker)
        {
            _stepTracker = stepTracker;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [AStepSetupScenario(index: (int)ScenarioSteps.Setup1, description: "RunScenario Method")]
        public void Setup1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [AStepSetupScenario(index: (int)ScenarioSteps.Setup2, description: "RunScenario Method")]
        public void Setup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [AStepSetupScenario(index: (int)ScenarioSteps.Setup3, description: "RunScenario Method")]
        public void Setup3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [ABusinessStepScenario(index: (int)ScenarioSteps.BusinessStep1, description: "RunScenario Method")]
        public void RunMethod1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [ABusinessStepScenario(index: (int)ScenarioSteps.BusinessStep2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [ABusinessStepScenario(index: (int)ScenarioSteps.BusinessStep3, description: "RunScenario  15 Method")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [AStepCleanupScenario(index: (int)ScenarioSteps.Cleanup1, description: "RunScenario Method")]
        public void Cleanup1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [AStepCleanupScenario(index: (int)ScenarioSteps.Cleanup2, description: "RunScenario Method")]
        public void Cleanup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [AStepCleanupScenario(index: (int)ScenarioSteps.Cleanup3, description: "RunScenario Method")]
        public void Cleanup3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();
        }

    }
}