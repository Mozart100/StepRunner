using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System;


namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    using System.Reflection;
    using Ark.StepRunner.CustomAttribute;

    [AScenario(description: "test scenario")]
    internal class RunAllStepParallel
    {

        internal enum StepsForScenario
        {
            SetupStep1,
            SetupStep2,
            SetupStep3,
            SetupStep4,
            SetupStep5,
            BusinessStep1,
            BusinessStep2,
            BusinessStep3,
            Cleanup1,
            Cleanup2,
            Cleanup3

        }

        private readonly StepTrack<ABusinessStepScenarioAttribute> _stepTracker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public RunAllStepParallel(StepTrack<ABusinessStepScenarioAttribute> stepTracker)
        {
            _stepTracker = stepTracker;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        [AScenarioStepTimeout(seconds: 20)]
        [AScenarioStepParallelAttribute]
        [AStepSetupScenario(index: (int)StepsForScenario.SetupStep1, description: "RunScenario Method")]
        public void Setup1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            Console.WriteLine("Start attribute.Index = [{0}]", attribute.Index);

            _stepTracker.Enqueue(attribute);
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        [AScenarioStepTimeout(seconds: 20)]
        [AScenarioStepParallel]
        [AStepSetupScenario(index: (int)StepsForScenario.SetupStep2, description: "RunScenario Method")]
        public void Setup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            Console.WriteLine("Start attribute.Index = [{0}]", attribute.Index);

            _stepTracker.Enqueue(attribute);
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AScenarioStepTimeout(seconds: 20)]
        [AScenarioStepParallelAttribute]
        [AStepSetupScenario(index: (int)StepsForScenario.SetupStep3, description: "RunScenario Method")]
        public void Setup3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            Console.WriteLine("Start attribute.Index = [{0}]", attribute.Index);

            _stepTracker.Enqueue(attribute);
            Thread.Sleep(TimeSpan.FromSeconds(1));

        }

        //     [AScenarioStepTimeout(seconds: 20)]
        //     [AScenarioStepParallelAttribute]
        //     [AStepSetupScenario(index: (int)StepsForScenario.SetupStep4, description: "RunScenario Method")]
        //     public void Setup4()
        //     {
        //         var method = MethodBase.GetCurrentMethod();
        //         var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
        //         Console.WriteLine("Start attribute.Index = [{0}]", attribute.Index);

        //         _stepTracker.Enqueue(attribute);
        //         Thread.Sleep(TimeSpan.FromSeconds(1));

        //     }
        //[AScenarioStepTimeout(seconds: 20)]
        //     [AScenarioStepParallelAttribute]
        //     [AStepSetupScenario(index: (int)StepsForScenario.SetupStep5, description: "RunScenario Method")]
        //     public void Setup5()
        //     {
        //         var method = MethodBase.GetCurrentMethod();
        //         var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
        //         Console.WriteLine("Start attribute.Index = [{0}]", attribute.Index);

        //         _stepTracker.Enqueue(attribute);
        //         Thread.Sleep(TimeSpan.FromSeconds(1));

        //     }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AScenarioStepTimeout(seconds: 20)]
        [AScenarioStepParallelAttribute]
        [ABusinessStepScenario(index: (int)StepsForScenario.BusinessStep1, description: "RunMethod1")]
        public void RunMethod1()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            Console.WriteLine("Start attribute.Index = [{0}]", attribute.Index);

            _stepTracker.Enqueue(attribute);
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AScenarioStepTimeout(seconds: 20)]
        [AScenarioStepParallelAttribute]
        [ABusinessStepScenario(index: (int)StepsForScenario.BusinessStep2, description: "RunMethod2")]
        public void RunMethod2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            Console.WriteLine("Start attribute.Index = [{0}]", attribute.Index);

            _stepTracker.Enqueue(attribute);
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [AScenarioStepTimeout(seconds: 20)]
        [AScenarioStepParallelAttribute]
        [ABusinessStepScenario(index: (int)StepsForScenario.BusinessStep3, description: "RunMethod3")]
        public void RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            Console.WriteLine("Start attribute.Index = [{0}]", attribute.Index);

            _stepTracker.Enqueue(attribute);
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AScenarioStepTimeout(seconds: 20)]
        [AScenarioStepParallelAttribute]
        [AStepCleanupScenario(index: (int)StepsForScenario.Cleanup1, description: "RunScenario  15 Method")]
        public void Cleanup1()
        {

            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            Console.WriteLine("Start attribute.Index = [{0}]", attribute.Index);

            _stepTracker.Enqueue(attribute);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.WriteLine("Finish attribute.Index = [{0}]", attribute.Index);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [AScenarioStepTimeout(seconds: 20)]
        [AScenarioStepParallelAttribute]
        [AStepCleanupScenario(index: (int)StepsForScenario.Cleanup2, description: "RunScenario  15 Method")]
        public void Cleanup2()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            Console.WriteLine("Start attribute.Index = [{0}]", attribute.Index);

            _stepTracker.Enqueue(attribute);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.WriteLine("Finish attribute.Index = [{0}]", attribute.Index);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        [AScenarioStepTimeout(seconds: 20)]
        [AScenarioStepParallelAttribute]
        [AStepCleanupScenario(index: (int)StepsForScenario.Cleanup3, description: "RunScenario  15 Method")]
        public void Cleanup3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            Console.WriteLine("Start attribute.Index = [{0}]", attribute.Index);

            _stepTracker.Enqueue(attribute);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.WriteLine("Finish attribute.Index = [{0}]", attribute.Index);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

    }
}