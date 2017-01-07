using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ark.StepRunner.TraceLogger
{

    public interface IStepRunnerLoggerBase
    {
        void Debug(string message);
        void Log(string message);
        void Error(string message);
        void Warning(string message);
    }


    public interface IStepPublisherLogger : IStepRunnerLoggerBase
    {

    }

}
