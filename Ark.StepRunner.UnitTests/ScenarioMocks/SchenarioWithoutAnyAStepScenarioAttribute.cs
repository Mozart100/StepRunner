using Ark.StepRunner.CustomAttribute;

namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    [AScenario(description: "test scenario")]
    public class SchenarioWithoutAnyAStepScenarioAttribute
    {
        public SchenarioWithoutAnyAStepScenarioAttribute()
        {

        }

        //[AStepScenarioAttribute(index: 1, description: "RunScenario Method")]
        public void RunMethod()
        {

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        public void RunMethod2()
        {

        }


    }
}