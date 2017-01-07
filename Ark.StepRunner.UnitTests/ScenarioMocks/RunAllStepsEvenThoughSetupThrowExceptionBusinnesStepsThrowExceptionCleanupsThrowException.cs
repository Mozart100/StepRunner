namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    using System.Reflection;
    using Ark.StepRunner.CustomAttribute;

    [AScenario(description: "test scenario")]
    internal class RunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException
    {

        internal enum StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException
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
        [AStepSetupScenario(index: (int)StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Setup1, description: "RunScenario Method")]
        public void Setup1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
                throw new System.Exception();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [AStepSetupScenario(index: (int)StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Setup2, description: "RunScenario Method")]
        public void Setup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [AStepSetupScenario(index: (int)StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Setup3, description: "RunScenario Method")]
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
        [ABusinessStepScenario(index: (int)StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.BusinessStep1, description: "RunScenario Method")]
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
        [ABusinessStepScenario(index: (int)StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.BusinessStep2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [ABusinessStepScenario(index: (int)StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.BusinessStep3, description: "RunScenario  15 Method")]
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
        [AStepCleanupScenario(index: (int)StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Cleanup1, description: "RunScenario Method")]
        public void Cleanup1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [AStepCleanupScenario(index: (int)StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Cleanup2, description: "RunScenario Method")]
        public void Cleanup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AExceptionIgnore]
        [AStepCleanupScenario(index: (int)StepsForRunAllStepsEvenThoughSetupThrowExceptionBusinnesStepsThrowExceptionCleanupsThrowException.Cleanup3, description: "RunScenario Method")]
        public void Cleanup3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute);
            throw new System.Exception();
        }

    }
}