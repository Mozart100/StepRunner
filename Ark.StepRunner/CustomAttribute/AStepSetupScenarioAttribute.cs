namespace Ark.StepRunner.CustomAttribute
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class AStepSetupScenarioAttribute : ABusinessStepScenarioAttribute
    {
        //private int _index;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        //public AStepSetupScenarioAttribute(string description)
        //    : this(index: -1, description: description)
        //{

        //}

        //--------------------------------------------------------------------------------------------------------------------------------------

        public AStepSetupScenarioAttribute(int index, string description)
            : base(index: index, description: description)
        {
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        //public new int Index
        //{
        //    get { return _index; }
        //    set { _index = value; }
        //}
    }
}