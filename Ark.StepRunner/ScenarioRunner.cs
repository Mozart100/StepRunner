﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ark.StepRunner.CustomAttribute;
using Ark.StepRunner.Exceptions;
using Ark.StepRunner.ScenarioStepResult;
using System.Threading;

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
        private readonly MethodInvoker _methodInvoker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ScenarioRunner()
        {
            _methodInvoker = new MethodInvoker();
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
                var timeout = ExtractTimeout(method);

                numberInvokedSteps++;
                ScenarioStepReturnBase scenarioStepResult = null;
                try
                {
                    scenarioStepResult = _methodInvoker.MethodInvoke<TScenario>(
                        scenario,
                        method,
                        timeout,
                        previousParameters);
                }
                catch (AScenarioStepTimeoutException timeoutException)
                {
                    return new ScenarioResult(isSuccessful: false, numberScenarioStepInvoked: numberInvokedSteps, exception: timeoutException); ;
                }
                catch (Exception exception)
                {
                    return new ScenarioResult(isSuccessful: false, numberScenarioStepInvoked: numberInvokedSteps, exception: exception); ;
                }

                previousParameters = scenarioStepResult.Parameters;

                var scenarioStepJumpToStep = scenarioStepResult as ScenarioStepJumpToStep;

                if (scenarioStepJumpToStep != null)
                {
                    for (int i = index + 1; i < orderedMethods.Count; i++)
                    {
                        if (orderedMethods[i].Key == scenarioStepJumpToStep.IndexToJumpToStep)
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

        //--------------------------------------------------------------------------------------------------------------------------------------

        private TimeSpan ExtractTimeout(MethodInfo method)
        {
            var attribute = method.GetCustomAttribute(typeof(AScenarioStepTimeoutAttribute)) as AScenarioStepTimeoutAttribute;

            if (attribute != null)
            {
                return attribute.Timeout;
            }

            return TimeSpan.Zero;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        private class MethodInvoker
        {
            private readonly ManualResetEvent _manuelResetEvent;

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------

            public MethodInvoker()
            {
                _manuelResetEvent = new ManualResetEvent(initialState: false);
            }

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------

            public ScenarioStepReturnBase MethodInvoke<TScenario>(
                TScenario scenario,
                MethodInfo methodInfo,
                TimeSpan timeout,
                params object[] parameters)
            {
                if (timeout == TimeSpan.Zero)
                {
                    _manuelResetEvent.Set();
                }
                else
                {
                    _manuelResetEvent.Reset();
                }


                var task = Invoke<TScenario>(scenario, methodInfo, parameters);

                if (_manuelResetEvent.WaitOne(timeout: timeout) == false)
                {
                    try
                    {
                        task.Dispose();
                    }
                    catch (Exception)
                    {
                        throw new AScenarioStepTimeoutException();
                    }
                }


                return task.Result;
            }

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------    

            private async Task<ScenarioStepReturnBase> Invoke<TScenario>(
              TScenario scenario,
              MethodInfo method,
              params object[] parameters)
            {
                ScenarioStepReturnBase methodResult = null;


                var task = Task.Run(() =>
                 {
                     methodResult = method.Invoke(scenario, parameters: parameters) as ScenarioStepReturnBase;
                     _manuelResetEvent.Set();
                 });
                await task;


                return methodResult ?? new ScenarioStepReturnVoid();
            }

        }
    }
}
