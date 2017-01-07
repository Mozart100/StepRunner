using Ark.StepRunner.CustomAttribute;

namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    [AScenario(description: "test scenario")]
    internal class SchenarioWithoutAnyAStepScenarioAttribute
    {
        internal SchenarioWithoutAnyAStepScenarioAttribute()
        {

        }

        //[ABusinessStepScenarioAttribute(index: 1, description: "RunScenario Method")]
        public void RunMethod()
        {

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        public void RunMethod2()
        {

        }


    }
}