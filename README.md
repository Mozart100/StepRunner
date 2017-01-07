# StepRunner

This Utility is suited for Integration Tests and smaull Utilities.

Basically, It has an easy API which the user create generic Scenario and just luanch it.

The scenario comprised of steps - there are 3 different type of steps SetupStep BusinessStep and cleanupStep.

fon each method a user should provide an Attribute such as [AStepSetupScenarioAttribute/]

 internal class DemoSchemaCreatorScenario
    {

        private enum StepsForDemoSchemaCreatorScenario // this enum solely for step indexing.
        {
            Setup1,
            Setup2,
            Setup3,
            
            Business1,
            Business2,
                       
            Cleanup1,
            Cleanup2,
            Cleanup3,
            
        }

        private readonly IDatabaseMunipolator _dataBaseManipulator;
        

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ScenarioWithNotNullAttributeInConstructor(IDatabaseMunipolator dataBaseManipulator)
        {
            _dataBaseManipulator = dataBaseManipulator;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepSetupScenarioAttribute(index: (int)StepsForDemoSchemaCreatorScenario.Setup1, description: "RunScenario Method")]
        public void RunMethod1()
        {
           _dataBaseManipulator.CreateRemoteSchema(); // Creating scehma

        }
        
        //--------------------------------------------------------------------------------------------------------------------------------------
        
        [AStepSetupScenarioAttribute(index: (int)StepsForDemoSchemaCreatorScenario.Setup2, description: "RunScenario Method")]
        public void RunSetup2()
        {
            _dataBaseManipulator.CreateScripts(); // Creating Scripts

        }
        
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------


        [ABusinessStepScenarioAttribute(index: (int)StepsForDemoSchemaCreatorScenario.Business1, description: "RunMethod1 Method")]
        public void RunMethod1()
        {
          _dataBaseManipulator.Copy();

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        [ABusinessStepScenarioAttribute(index: (int)StepsForScenarioWithNotNullAttributeInConstructor.Business2, description: "RunMethod2 Method")]
        public void RunMethod2()
        {
            _dataBaseManipulator.RunDeltaScripts();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------


        [AStepCleanupScenarioAttribute(index: (int)StepsForScenarioWithNotNullAttributeInConstructor.Cleanup1, description: "RunMethodCleanup1 Method")]
        public void RunMethodCleanup1()
        {
           ....
        }
        
        //--------------------------------------------------------------------------------------------------------------------------------------

        [AStepCleanupScenarioAttribute(index: (int)StepsForScenarioWithNotNullAttributeInConstructor.Cleanup2, description: "RunMethodCleanup2 Method")]
        public void RunMethodCleanup2()
        {
            ...
        }

        
    }



