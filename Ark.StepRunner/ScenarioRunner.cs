using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ark.StepRunner.CustomAttribute;

namespace Ark.StepRunner
{
    public class ScenarioRunner
    {
        private readonly Dictionary<int, MethodInfo> _scenarioSteps;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ScenarioRunner()
        {
            _scenarioSteps = new Dictionary<int, MethodInfo>();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public bool RunScenario<TScenario>(params object[] parameters)
        {

            if (AssembleMethDataForScenario<TScenario>() == false)
            {
                return false;
            }

            var scenario = (TScenario)Activator.CreateInstance(typeof(TScenario), parameters);

            var result = RunScenario(scenario);
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        private bool RunScenario<TScenario>(TScenario scenario)
        {
            var orderedMethods = _scenarioSteps.OrderBy(x => x.Key).Select(x => x.Value).ToList();

            foreach (var method in orderedMethods)
            {
                try
                {
                    method.Invoke(scenario, parameters: null);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
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
