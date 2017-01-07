using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ark.StepRunner.CustomAttribute;
using Ark.StepRunner.Exceptions;
using Ark.StepRunner.ScenarioStepResult;
using System.Threading;
using Ark.StepRunner.TraceLogger;

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

        private class StepAndAttributeBundle
        {
            private readonly MethodInfo _methodInfo;
            private readonly AStepScenarioAttribute _stepScenario;

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------

            public StepAndAttributeBundle(MethodInfo methodInfo, AStepScenarioAttribute stepScenario)
            {
                _methodInfo = methodInfo;
                _stepScenario = stepScenario;
            }

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------
            public MethodInfo MethodInfo => _methodInfo;

            //--------------------------------------------------------------------------------------------------------------------------------------

            public AStepScenarioAttribute StepScenario
            {
                get { return _stepScenario; }
            }

            //--------------------------------------------------------------------------------------------------------------------------------------
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        private readonly IStepPublisherLogger _stepPublisherLogger;

        //--------------------------------------------------------------------------------------------------------------------------------------

        private readonly Dictionary<int, StepAndAttributeBundle> _scenarioSteps;
        private readonly Dictionary<int, StepAndAttributeBundle> _scenarioSetups;
        private readonly Dictionary<int, StepAndAttributeBundle> _scenarioCleanups;

        //--------------------------------------------------------------------------------------------------------------------------------------

        private readonly MethodInvoker _methodInvoker;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ScenarioRunner(IStepPublisherLogger stepPublisherLogger)
        {
            _stepPublisherLogger = stepPublisherLogger;
            _methodInvoker = new MethodInvoker();

            _scenarioSteps = new Dictionary<int, StepAndAttributeBundle>();
            _scenarioSetups = new Dictionary<int, StepAndAttributeBundle>();
            _scenarioCleanups = new Dictionary<int, StepAndAttributeBundle>();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ScenarioResult RunScenario<TScenario>(params object[] parameters)
        {

            if (AssembleMethDataForScenario<TScenario>() == false)
            {
                return new ScenarioResult(isSuccessful: false, numberScenarioStepInvoked: 0, exceptions: new AScenarioMissingAttributeException());
            }


            var scenario = (TScenario)Activator.CreateInstance(typeof(TScenario), parameters);

            var notNullList = NotNullLocator(scenario, parameters).ToList();

            if (notNullList.Any())
            {
                return new ScenarioResult(isSuccessful: false,
                    numberScenarioStepInvoked: 0,
                    exceptions: notNullList.First());
            }


            var result = RunScenario(scenario);
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        private ScenarioResult RunScenario<TScenario>(TScenario scenario)
        {
            var setupStepsResult = RunScenarioStep(scenario, _scenarioSetups);
            ScenarioResult businessStepsResult =  new EmptyScenarioResult();

            if (setupStepsResult.IsSuccessful)
            {
                businessStepsResult = RunScenarioStep(scenario, _scenarioSteps);
            }

            var cleanupStepsResult = RunScenarioStep(scenario, _scenarioCleanups);

            return setupStepsResult + businessStepsResult + cleanupStepsResult;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        private ScenarioResult RunScenarioStep<TScenario>(TScenario scenario,
            IDictionary<int, StepAndAttributeBundle> steps)
        {
            var orderedMethods = steps.OrderBy(x => x.Key).ToList();

            var numberInvokedSteps = 0;
            object[] previousParameters = null;

            for (int index = 0; index < orderedMethods.Count;)
            {
                var method = orderedMethods[index].Value.MethodInfo;
                var timeout = ExtractTimeout(method);

                numberInvokedSteps++;
                ScenarioStepReturnBase scenarioStepResult = null;
                try
                {
                    _stepPublisherLogger.Log(string.Format("[{0}] Step was started.",
                        orderedMethods[index].Value.StepScenario.Description));
                    scenarioStepResult = _methodInvoker.MethodInvoke<TScenario>(
                        scenario,
                        method,
                        timeout,
                        previousParameters);

                    _stepPublisherLogger.Log(string.Format("[{0}] Step was finished successfully.",
                        orderedMethods[index].Value.StepScenario.Description));
                }
                catch (AScenarioStepTimeoutException timeoutException)
                {
                    _stepPublisherLogger.Error(string.Format("[{0}] Step was finished usuccessfully due to timeout.",
                        orderedMethods[index].Value.StepScenario.Description));
                    return new ScenarioResult(isSuccessful: false, numberScenarioStepInvoked: numberInvokedSteps,
                        exceptions: timeoutException);

                }
                catch (Exception exception)
                {
                    _stepPublisherLogger.Error(
                        string.Format("[{0}] Step was finished usuccessfully due to the following exceptions [{1}].",
                            orderedMethods[index].Value.StepScenario.Description, exception));
                    return new ScenarioResult(isSuccessful: false, numberScenarioStepInvoked: numberInvokedSteps,
                        exceptions: exception);
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
                    //TODO elsewhere  throw exceptions.
                }
                else
                {
                    index++;
                }
            }

            return new ScenarioResult(isSuccessful: true, numberScenarioStepInvoked: numberInvokedSteps, exceptions: null);
            ;
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
                var setupAttribute = method.GetCustomAttribute(typeof(AStepSetupScenarioAttribute)) as AStepSetupScenarioAttribute;
                if (setupAttribute != null)
                {
                    try
                    {
                        _scenarioSetups.Add(setupAttribute.Index, new StepAndAttributeBundle(methodInfo: method, stepScenario: setupAttribute));
                        continue;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                var cleanupAttribute = method.GetCustomAttribute(typeof(AStepCleanupScenarioAttribute)) as AStepCleanupScenarioAttribute;
                if (cleanupAttribute != null)
                {
                    try
                    {
                        _scenarioCleanups.Add(cleanupAttribute.Index, new StepAndAttributeBundle(methodInfo: method, stepScenario: cleanupAttribute));
                        continue;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }


                var attribute = method.GetCustomAttribute(typeof(AStepScenarioAttribute)) as AStepScenarioAttribute;
                if (attribute != null)
                {
                    try
                    {
                        _scenarioSteps.Add(attribute.Index, new StepAndAttributeBundle(methodInfo: method, stepScenario: attribute));
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

        private IEnumerable<AScenarioConstructorParameterNullException> NotNullLocator<TScenario>(TScenario scenario, params object[] parameters)
        {
            var index = 0;
            foreach (var parameter in typeof(TScenario).GetConstructors().First().GetParameters())
            {
                bool hasNotNullAttribute = parameter.CustomAttributes.Any(x => x.AttributeType == typeof(NotNullAttribute));
                if (hasNotNullAttribute == true)
                {
                    if (parameters[index] == null)
                    {
                        yield return new AScenarioConstructorParameterNullException(parameterName: parameter.Name);
                    }
                }
                index++;
            }
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



                ScenarioStepReturnBase result = null;

                try
                {
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
                    result = task.Result;
                }
                catch (Exception exception)
                {
                    throw exception.InnerException ?? exception;
                }


                return result;
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
                    //methodResult = methodInfo.Invoke(scenario, parameters: parameters) as ScenarioStepReturnBase;
                    try
                    {
                        methodResult = method.Invoke(scenario, parameters: parameters) as ScenarioStepReturnBase;
                    }
                    catch (Exception exception)
                    {
                        throw exception.InnerException;
                    }
                    _manuelResetEvent.Set();
                });

                await task;

                return methodResult ?? new ScenarioStepReturnVoid();
            }

        }
    }
}
