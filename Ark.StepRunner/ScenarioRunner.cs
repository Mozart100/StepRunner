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
            private readonly ABusinessStepScenarioAttribute _businessStepScenario;
            private readonly AExceptionIgnoreAttribute _exceptionIgnoreAttribute;

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------

            public StepAndAttributeBundle(
                MethodInfo methodInfo,
                ABusinessStepScenarioAttribute businessStepScenario,
                AExceptionIgnoreAttribute exceptionIgnoreAttribute)
            {
                _methodInfo = methodInfo;
                _businessStepScenario = businessStepScenario;
                _exceptionIgnoreAttribute = exceptionIgnoreAttribute;
            }

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------

            public MethodInfo MethodInfo => _methodInfo;

            //--------------------------------------------------------------------------------------------------------------------------------------

            public ABusinessStepScenarioAttribute BusinessStepScenario => _businessStepScenario;

            //--------------------------------------------------------------------------------------------------------------------------------------

            public AExceptionIgnoreAttribute ExceptionIgnoreAttribute => _exceptionIgnoreAttribute;

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
            var setupStepsResult = RunAllteps(scenario, _scenarioSetups);
            ScenarioResult businessStepsResult = new EmptyScenarioResult();

            if (setupStepsResult.IsSuccessful)
            {
                businessStepsResult = RunAllteps(scenario, _scenarioSteps);
            }

            var cleanupStepsResult = RunAllteps(scenario, _scenarioCleanups);

            return setupStepsResult + businessStepsResult + cleanupStepsResult;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        private ScenarioResult RunAllteps<TScenario>(
            TScenario scenario,
            IDictionary<int, StepAndAttributeBundle> steps)
        {
            ScenarioResult scenarioResult = new EmptyScenarioResult();
            var numberInvokedSteps = 0;
            object[] previousParameters = null;


            var orderedMethods = steps.OrderBy(x => x.Key).ToList();

            for (int index = 0; index < orderedMethods.Count;)
            {
                var method = orderedMethods[index].Value.MethodInfo;
                var timeout = ExtractTimeout(method);

                numberInvokedSteps++;
                ScenarioResult scenarioResultCurrent;
                var scenarioStepResult = RunScenarioStep(scenario, method, timeout, previousParameters, orderedMethods[index].Value.BusinessStepScenario,
                    out scenarioResultCurrent);

                scenarioResult += scenarioResultCurrent;

                if (scenarioResultCurrent.IsSuccessful == false)
                {
                    if (orderedMethods[index].Value.ExceptionIgnoreAttribute == null)
                    {
                        return scenarioResult;
                    }

                    scenarioResult |= new EmptyScenarioResult(isSuccessful: true);
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

            return scenarioResult;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        private ScenarioStepReturnBase RunScenarioStep<TScenario>(
            TScenario scenario,
            MethodInfo method,
            TimeSpan timeout,
            object[] previousParameters,
            ABusinessStepScenarioAttribute businessStepScenario,
            out ScenarioResult scenarioResult)
        {
            scenarioResult = new EmptyScenarioResult(numberScenarioStepInvoked: 1);
            ScenarioStepReturnBase scenarioStepResult = null;
            try
            {
                _stepPublisherLogger.Log(string.Format("[{0}] Step was started.", businessStepScenario.Description));
                scenarioStepResult = _methodInvoker.MethodInvoke<TScenario>(
                    scenario,
                    method,
                    timeout,
                    previousParameters);
            }
            catch (AScenarioStepTimeoutException timeoutException)
            {
                _stepPublisherLogger.Error(string.Format("[{0}] Step was finished usuccessfully due to timeout.", businessStepScenario.Description));
                {
                    scenarioResult = new ScenarioResult(isSuccessful: false, numberScenarioStepInvoked: 1, exceptions: timeoutException);
                    return new ScenarioStepReturnVoid();
                }
            }

            catch (Exception exception)
            {
                _stepPublisherLogger.Error(string.Format("[{0}] Step was finished usuccessfully due to the following exception [{1}].", businessStepScenario.Description, exception));

                {
                    scenarioResult = new ScenarioResult(isSuccessful: false, numberScenarioStepInvoked: 1, exceptions: exception);
                    return new ScenarioStepReturnVoid();
                }
            }

            _stepPublisherLogger.Log(string.Format("[{0}] Step was finished successfully.", businessStepScenario.Description));

            return scenarioStepResult;
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
                var ignoreAttribute = method.GetCustomAttribute(typeof(AExceptionIgnoreAttribute)) as AExceptionIgnoreAttribute;

                var setupAttribute = method.GetCustomAttribute(typeof(AStepSetupScenarioAttribute)) as AStepSetupScenarioAttribute;
                if (setupAttribute != null)
                {
                    try
                    {
                        _scenarioSetups.Add(setupAttribute.Index, new StepAndAttributeBundle(methodInfo: method, businessStepScenario: setupAttribute, exceptionIgnoreAttribute: ignoreAttribute));
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
                        _scenarioCleanups.Add(cleanupAttribute.Index, new StepAndAttributeBundle(methodInfo: method, businessStepScenario: cleanupAttribute, exceptionIgnoreAttribute: ignoreAttribute));
                        continue;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }


                var attribute = method.GetCustomAttribute(typeof(ABusinessStepScenarioAttribute)) as ABusinessStepScenarioAttribute;
                if (attribute != null)
                {
                    try
                    {
                        _scenarioSteps.Add(attribute.Index, new StepAndAttributeBundle(methodInfo: method, businessStepScenario: attribute, exceptionIgnoreAttribute: ignoreAttribute));
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
