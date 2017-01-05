using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ark.StepRunner.CustomAttribute;
using Ark.StepRunner.Exceptions;
using Ark.StepRunner.ScenarioStepResult;

namespace Ark.StepRunner
{
    public class ScenarioRunner
    {
        private class ScenarioStepReturnVoid : ScenarioStepReturnBase
        {
            public ScenarioStepReturnVoid()
                : base(parameters: null)
            {

            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------    

        private readonly Dictionary<int, MethodInfo> _scenarioSteps;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ScenarioRunner()
        {
            _scenarioSteps = new Dictionary<int, MethodInfo>();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ScenarioResult RunScenario<TScenario>(params object[] parameters)
        {

            if (AssembleMethDataForScenario<TScenario>() == false)
            {
                return new ScenarioResult(isSuccessful: false, numberScenarioStepInvoked: 0, exception: new AScenarioMissingAttributeException());
            }

            var scenario = (TScenario)Activator.CreateInstance(typeof(TScenario), parameters);

            var result = RunScenario(scenario);
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        private ScenarioResult RunScenario<TScenario>(TScenario scenario)
        {
            var orderedMethods = _scenarioSteps.OrderBy(x => x.Key).ToList();

            int numberInvokedSteps = 0;
            object[] previousParameters = null;

            for (int index = 0; index < orderedMethods.Count;)
            {
                var method = orderedMethods[index].Value;
                try
                {
                    numberInvokedSteps++;
                    var scenarioStepResult = method.Invoke(scenario, parameters: previousParameters) as ScenarioStepReturnBase;

                    //TODO Refactoring.
                    if (scenarioStepResult == null)
                    {
                        scenarioStepResult = new ScenarioStepReturnVoid();
                    }

                    previousParameters = scenarioStepResult.Parameters;

                    var scenarioStepJumpToStep = scenarioStepResult as ScenarioStepJumpToStep;
                    if (scenarioStepJumpToStep != null)
                    {
                        for (int i = index + 1; i < orderedMethods.Count; i++)
                        {
                            if ( orderedMethods[i].Key == scenarioStepJumpToStep.IndexToJumpToStep)
                            {
                                index = i;
                                break;
                            }
                        }
                        //TODO elsewhere  throw exception.
                    }
                    else
                    {
                        index++;
                    }

                }
                catch (Exception exception)
                {
                    return new ScenarioResult(isSuccessful: false, numberScenarioStepInvoked: numberInvokedSteps, exception: exception);
                }
            }

            return new ScenarioResult(isSuccessful: true, numberScenarioStepInvoked: numberInvokedSteps, exception: null); ;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        private bool AssembleMethDataForScenario<TScenario>()
        {
            if (Attribute.IsDefined(typeof(TScenario), typeof(AScenarioAttribute)) == false)
            {
                return false;
            }

            foreach (var method in typeof(TScenario).GetMethods())
            {

                var attribute = method.GetCustomAttribute(typeof(AStepScenarioAttribute)) as AStepScenarioAttribute;

                if (attribute != null)
                {
                    try
                    {
                        _scenarioSteps[attribute.Index] = method;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
