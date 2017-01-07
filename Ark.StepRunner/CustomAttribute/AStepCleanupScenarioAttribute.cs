namespace Ark.StepRunner.CustomAttribute
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class AStepCleanupScenarioAttribute : ABusinessStepScenarioAttribute
    {
     
        public AStepCleanupScenarioAttribute(int index, string description)
            : base(index: index, description: description)
        {
        }

        
    }
}