namespace Ark.StepRunner.CustomAttribute
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class AStepCleanupScenarioAttribute : AStepScenarioAttribute
    {
     
        public AStepCleanupScenarioAttribute(int index, string description)
            : base(index: index, description: description)
        {
        }

        
    }
}