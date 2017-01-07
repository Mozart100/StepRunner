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

        internal enum StepsForRunAllStepsAndFaileDueToTimeout
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
            

        [ABusinessStepScenario(index: (int)StepsForRunAllStepsAndFaileDueToTimeout.Step1, description: "RunScenario Method")]
        public void RunMethod1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AScenarioStepTimeout(seconds: 1)]
        [ABusinessStepScenario(index: (int)StepsForRunAllStepsAndFaileDueToTimeout.Step2, description: "RunScenario 5 Method")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            _stepTracker.Enqueue(attribute.Index);

            var task  = Task.Run(() => RunMethod4());
            task.Wait();

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)StepsForRunAllStepsAndFaileDueToTimeout.Step3, description: "RunScenario  15 Method")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        private void RunMethod4()
        {
            Thread.Sleep(TimeSpan.FromMinutes(1));
        }
    }
}