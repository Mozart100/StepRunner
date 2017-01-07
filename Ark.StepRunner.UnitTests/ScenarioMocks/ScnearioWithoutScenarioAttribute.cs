using Ark.StepRunner.CustomAttribute;

namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    internal class ScnearioWithoutScenarioAttribute
    {
        internal ScnearioWithoutScenarioAttribute()
        {

        }

        [ABusinessStepScenario(index: 1, description: "RunScenario Method")]
        public void RunMethod()
        {

        }
    }
}