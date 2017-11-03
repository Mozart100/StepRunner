using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System;
using Ark.StepRunner.CustomAttribute;

namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    

    [AScenario(description: "test scenario")]
    internal class RunStepsAndFaileDueToTimeout 
    {

        internal enum ScenarioSteps
        {
            Step1,
            Step2,
            Step3,
            Step4
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        private readonly StepTrack<int> _stepTracker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public RunStepsAndFaileDueToTimeout(StepTrack<int> stepTracker)
        {
            _stepTracker = stepTracker;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
            

        [ABusinessStepScenario(index: (int)ScenarioSteps.Step1, description: "RunScenario Method")]
        public void RunMethod1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AScenarioStepTimeout(seconds: 1)]
        [ABusinessStepScenario(index: (int)ScenarioSteps.Step2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            _stepTracker.Enqueue(attribute.Index);

            //var task  = Task.Run(() => RunMethod4());
            //task.Wait();

            RunMethod4();

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)ScenarioSteps.Step3, description: "RunScenario  15 Method")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);
        }

        //[AStepCleanupScenario(index: (int)ScenarioSteps.Step3, description: "Cleanup")]
        //public void Cleanup()
        //{
        //    //var method = MethodBase.GetCurrentMethod();
        //    //var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

        //    //_stepTracker.Enqueue(attribute.Index);
        //}

        //--------------------------------------------------------------------------------------------------------------------------------------
        private void RunMethod4()
        {
            int loops = 60;
            while (loops-- > 0)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}