//namespace Ark.StepRunner.UnitTests.ScenarioMocks
//{
//    using System;
//    using System.Diagnostics;
//    using System.Reflection;
//    using System.Threading;
//    using Ark.StepRunner.CustomAttribute;

//    [AScenario(description: "test scenario")]
//    internal class RunAllStepParallelAndInSetupThrowTimeoutException
//    {

//        internal enum ScenarioSteps
//        {
//            SetupStep1,
//            SetupStep2,
//            SetupStep3,
//            //SetupStep4,
//            //SetupStep5,
//            BusinessStep1,
//            BusinessStep2,
//            BusinessStep3,

//            Cleanup1,
//            Cleanup2,
//            Cleanup3

//        }

//        //--------------------------------------------------------------------------------------------------------------------------------------
//        //--------------------------------------------------------------------------------------------------------------------------------------

//        private readonly StepTrack<ABusinessStepScenarioAttribute> _stepTracker;
//        private readonly Stopwatch _stopWatch;

//        //--------------------------------------------------------------------------------------------------------------------------------------
//        //--------------------------------------------------------------------------------------------------------------------------------------

//        public RunAllStepParallelAndInSetupThrowTimeoutException(StepTrack<ABusinessStepScenarioAttribute> stepTracker)
//        {
//            _stepTracker = stepTracker;
//            _stopWatch = new Stopwatch();
//            _stopWatch.Start();
//        }

//        //--------------------------------------------------------------------------------------------------------------------------------------
//        //--------------------------------------------------------------------------------------------------------------------------------------
//        [AScenarioStepTimeout(seconds: 20)]
//        [AScenarioStepParallel]
//        [AStepSetupScenario(index: (int)ScenarioSteps.SetupStep1, description: "RunScenario Method")]
//        public void Setup1()
//        {
//            var method = MethodBase.GetCurrentMethod();
//            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
            
//            var elapse = _stopWatch.Elapsed;
//            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine("time [{0}] | Start attribute.Index = [{1}]", elapsedTime,attribute.Index );


//            _stepTracker.Enqueue(attribute);
//            Thread.Sleep(TimeSpan.FromSeconds(1));
//            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine(" Finish Time [{0}] attribute.Index = [{1}]", elapsedTime, attribute.Index);
//        }

//        //--------------------------------------------------------------------------------------------------------------------------------------
//        [AScenarioStepTimeout(seconds: 1)]
//        [AScenarioStepParallel]
//        [AStepSetupScenario(index: (int)ScenarioSteps.SetupStep2, description: "RunScenario Method")]
//        public void Setup2()
//        {
//            var method = MethodBase.GetCurrentMethod();
//            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
//            var elapse = _stopWatch.Elapsed;
//            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine("time [{0}] | Start attribute.Index = [{1}]", elapsedTime, attribute.Index);


//            _stepTracker.Enqueue(attribute);
//            Thread.Sleep(TimeSpan.FromSeconds(5));
//            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine(" Finish Time [{0}] attribute.Index = [{1}]", elapsedTime, attribute.Index);
//        }

//        //--------------------------------------------------------------------------------------------------------------------------------------

//        [AScenarioStepTimeout(seconds: 20)]
//        [AScenarioStepParallel]
//        [AStepSetupScenario(index: (int)ScenarioSteps.SetupStep3, description: "RunScenario Method")]
//        public void Setup3()
//        {
//            var method = MethodBase.GetCurrentMethod();
//            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
//            var elapse = _stopWatch.Elapsed;
//            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine("time [{0}] | Start attribute.Index = [{1}]", elapsedTime, attribute.Index);


//            _stepTracker.Enqueue(attribute);
//            Thread.Sleep(TimeSpan.FromSeconds(1));
//            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine(" Finish Time [{0}] attribute.Index = [{1}]", elapsedTime, attribute.Index);

//        }

//        //     [AScenarioStepTimeout(seconds: 20)]
//        //     [AScenarioStepParallelAttribute]
//        //     [AStepSetupScenario(index: (int)StepsForScenario.SetupStep4, description: "RunScenario Method")]
//        //     public void Setup4()
//        //     {
//        //         var method = MethodBase.GetCurrentMethod();
//        //         var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
//        //var elapse = _stopWatch.Elapsed;
//        //var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//        //Console.WriteLine("time [{0}] | Start attribute.Index = [{1}]", elapsedTime, attribute.Index);

//        //         _stepTracker.Enqueue(attribute);
//        //         Thread.Sleep(TimeSpan.FromSeconds(1));
//        //elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//        //    Console.WriteLine(" Finish Time [{0}] attribute.Index = [{1}]",elapsedTime, attribute.Index);


//        //     }
//        //[AScenarioStepTimeout(seconds: 20)]
//        //     [AScenarioStepParallelAttribute]
//        //     [AStepSetupScenario(index: (int)StepsForScenario.SetupStep5, description: "RunScenario Method")]
//        //     public void Setup5()
//        //     {
//        //         var method = MethodBase.GetCurrentMethod();
//        //         var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
//        //var elapse = _stopWatch.Elapsed;
//        //var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//        //Console.WriteLine("time [{0}] | Start attribute.Index = [{1}]", elapsedTime, attribute.Index);


//        //         _stepTracker.Enqueue(attribute);
//        //         Thread.Sleep(TimeSpan.FromSeconds(1));
//        //elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//        //    Console.WriteLine(" Finish Time [{0}] attribute.Index = [{1}]",elapsedTime, attribute.Index);

//        //     }

//        //--------------------------------------------------------------------------------------------------------------------------------------
//        //--------------------------------------------------------------------------------------------------------------------------------------
//        //--------------------------------------------------------------------------------------------------------------------------------------
//        //--------------------------------------------------------------------------------------------------------------------------------------

//        [AScenarioStepTimeout(seconds: 20)]
//        [AScenarioStepParallel]
//        [ABusinessStepScenario(index: (int)ScenarioSteps.BusinessStep1, description: "RunMethod1")]
//        public void RunMethod1()
//        {
//            var method = MethodBase.GetCurrentMethod();
//            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

//            var elapse = _stopWatch.Elapsed;
//            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine("time [{0}] | Start attribute.Index = [{1}]", elapsedTime, attribute.Index);


//            _stepTracker.Enqueue(attribute);
//            Thread.Sleep(TimeSpan.FromSeconds(1));
//            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine(" Finish Time [{0}] attribute.Index = [{1}]", elapsedTime, attribute.Index);
//        }

//        //--------------------------------------------------------------------------------------------------------------------------------------

//        [AScenarioStepTimeout(seconds: 20)]
//        [AScenarioStepParallel]
//        [ABusinessStepScenario(index: (int)ScenarioSteps.BusinessStep2, description: "RunMethod2")]
//        public void RunMethod2()
//        {
//            var method = MethodBase.GetCurrentMethod();
//            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
//            var elapse = _stopWatch.Elapsed;
//            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine("time [{0}] | Start attribute.Index = [{1}]", elapsedTime, attribute.Index);


//            _stepTracker.Enqueue(attribute);
//            Thread.Sleep(TimeSpan.FromSeconds(1));
//            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine(" Finish Time [{0}] attribute.Index = [{1}]", elapsedTime, attribute.Index);
//        }

//        //--------------------------------------------------------------------------------------------------------------------------------------


//        [AScenarioStepTimeout(seconds: 20)]
//        [AScenarioStepParallel]
//        [ABusinessStepScenario(index: (int)ScenarioSteps.BusinessStep3, description: "RunMethod3")]
//        public void RunMethod3()
//        {
//            var method = MethodBase.GetCurrentMethod();
//            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
//            var elapse = _stopWatch.Elapsed;
//            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine("time [{0}] | Start attribute.Index = [{1}]", elapsedTime, attribute.Index);


//            _stepTracker.Enqueue(attribute);
//            Thread.Sleep(TimeSpan.FromSeconds(2));
//            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine(" Finish Time [{0}] attribute.Index = [{1}]", elapsedTime, attribute.Index);
//        }

//        //--------------------------------------------------------------------------------------------------------------------------------------
//        //--------------------------------------------------------------------------------------------------------------------------------------

//        [AScenarioStepTimeout(seconds: 20)]
//        [AScenarioStepParallel]
//        [AStepCleanupScenario(index: (int)ScenarioSteps.Cleanup1, description: "Cleanup")]
//        public void Cleanup1()
//        {

//            var method = MethodBase.GetCurrentMethod();
//            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

//            var elapse = _stopWatch.Elapsed;
//            Console.WriteLine("time [{0}] | Start attribute.Index = [{1}]", _stopWatch.Elapsed, attribute.Index);
//            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds,elapse.Milliseconds / 10);

//            _stepTracker.Enqueue(attribute);
//            Thread.Sleep(TimeSpan.FromSeconds(1));
//            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine(" Finish Time [{0}] attribute.Index = [{1}]", elapsedTime, attribute.Index);
//        }

//        //--------------------------------------------------------------------------------------------------------------------------------------

//        [AScenarioStepTimeout(seconds: 20)]
//        [AScenarioStepParallel]
//        [AStepCleanupScenario(index: (int)ScenarioSteps.Cleanup2, description: "RunScenario  15 Method")]
//        public void Cleanup2()
//        {
//            var method = MethodBase.GetCurrentMethod();
//            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
//            var elapse = _stopWatch.Elapsed;
//            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine("time [{0}] | Start attribute.Index = [{1}]", elapsedTime, attribute.Index);


//            _stepTracker.Enqueue(attribute);
//            Thread.Sleep(TimeSpan.FromSeconds(1));
//            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine(" Finish Time [{0}] attribute.Index = [{1}]",elapsedTime, attribute.Index);
//        }

//        //--------------------------------------------------------------------------------------------------------------------------------------
//        [AScenarioStepTimeout(seconds: 20)]
//        [AScenarioStepParallel]
//        [AStepCleanupScenario(index: (int)ScenarioSteps.Cleanup3, description: "RunScenario  15 Method")]
//        public void Cleanup3()
//        {
//            var method = MethodBase.GetCurrentMethod();
//            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];
//            var elapse = _stopWatch.Elapsed;
//            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine("time [{0}] | Start attribute.Index = [{1}]", elapsedTime, attribute.Index);


//            _stepTracker.Enqueue(attribute);
//            Thread.Sleep(TimeSpan.FromSeconds(1));
//            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapse.Hours, elapse.Minutes, elapse.Seconds, elapse.Milliseconds / 10);
//            Console.WriteLine(" Finish Time [{0}] attribute.Index = [{1}]", elapsedTime, attribute.Index);
//        }

//        //--------------------------------------------------------------------------------------------------------------------------------------

//    }
//}