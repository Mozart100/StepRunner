using Ark.StepRunner.CustomAttribute;

namespace Ark.StepRunner.UnitTests.ScenarioMocks
{
    public class ScnearioWithoutScenarioAttribute
    {
        public ScnearioWithoutScenarioAttribute()
        {

        }

        [AStepScenario(index: 1, description: "RunScenario Method")]
        public void RunMethod()
        {

        }
    }
}