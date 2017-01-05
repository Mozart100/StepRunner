using System;

namespace Ark.StepRunner
{
    public class ScenarioResult
    {
        private readonly bool _isSuccessful;
        private readonly int _numberScenarioStepInvoked;
        private readonly Exception _exception;


        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ScenarioResult(bool isSuccessful, int numberScenarioStepInvoked, Exception exception)
        {
            _isSuccessful = isSuccessful;
            _numberScenarioStepInvoked = numberScenarioStepInvoked;
            _exception = exception;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------


        public bool IsSuccessful => _isSuccessful;

        //--------------------------------------------------------------------------------------------------------------------------------------

        public int NumberScenarioStepInvoked => _numberScenarioStepInvoked;
    }


}