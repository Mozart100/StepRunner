using System;
using System.Reflection;
using Ark.StepRunner.CustomAttribute;
using Ark.StepRunner.ScenarioStepResult;
using System.Collections.Generic;
using Shouldly;

namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    [AScenario(description: "test scenario")]
    internal class RunAllStepsAndPassingParametersBetweenSteps
    {
        internal class ExpectedData

        {

        }


        internal enum ScenarioSteps
        {
            SetupPassingIEnumerable,
            SetupAcceptingParameter,
            Step1,
            Step2,
            Step3,
            Step4
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        private readonly StepTrack<int> _stepTracker;
        private readonly int _magicNumForStep2;
        private readonly string _magicStringForStep2;
        private readonly int _magicNumForStep4;
        private readonly string _magicStringForStep4;
        private List<string> _days;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public RunAllStepsAndPassingParametersBetweenSteps(StepTrack<int> stepTracker,
            int magicNumForStep2,
            string magicStringForStep2,
            int magicNumForStep4,
            string magicStringForStep4)
        {
            _stepTracker = stepTracker;
            _magicNumForStep2 = magicNumForStep2;
            _magicStringForStep2 = magicStringForStep2;
            _magicNumForStep4 = magicNumForStep4;
            _magicStringForStep4 = magicStringForStep4;

            _days = new List<string> { "Sunday", "Monday" };
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenario(index: (int)ScenarioSteps.SetupPassingIEnumerable, description: "Pass IEnumerable")]
        public ScenarioStepReturnNextStep PassingIEnumerable()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);


            return new ScenarioStepReturnNextStep(_days);

        }


        [AStepSetupScenario(index: (int)ScenarioSteps.SetupAcceptingParameter, description: "Pass IEnumerable")]
        public ScenarioStepReturnNextStep PassingIEnumerable(IList<string> list)
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

            list.ShouldNotBeNull();

            foreach (var item in list)
            {
                _days.Contains(item).ShouldBeTrue();
            }

            return new ScenarioStepReturnNextStep(_days);
        }


        [ABusinessStepScenario(index: (int)ScenarioSteps.Step1, description: "RunScenario Method")]
        //public ScenarioStepReturnNextStep RunMethod1()
        public ScenarioStepReturnNextStep RunMethod1()
        {

            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

            return new ScenarioStepReturnNextStep(_magicNumForStep2, _magicStringForStep2);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)ScenarioSteps.Step2, description: "RunScenario 5 Method")]
        public void RunMethod2(int magicNumForStep2, string magicStringForStep2)
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

            if ((_magicNumForStep2 != magicNumForStep2) || (_magicStringForStep2 != magicStringForStep2))
            {
                throw new Exception("dasf");
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        [ABusinessStepScenario(index: (int)ScenarioSteps.Step3, description: "RunScenario  15 Method")]
        public ScenarioStepReturnNextStep RunMethod3()
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

            return new ScenarioStepReturnNextStep(_magicNumForStep4, _magicStringForStep4);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenario(index: (int)ScenarioSteps.Step4, description: "RunScenario  15 Method")]
        public void RunMethod4(int magicNumForStep4, string magicStringForStep4)
        {
            var method = MethodBase.GetCurrentMethod();
            var attribute = (ABusinessStepScenarioAttribute)method.GetCustomAttributes(typeof(ABusinessStepScenarioAttribute), true)[0];

            _stepTracker.Enqueue(attribute.Index);

            if ((_magicNumForStep4 != magicNumForStep4) || (_magicStringForStep4 != magicStringForStep4))
            {
                throw new Exception("xxx");
            }
        }
    }
}