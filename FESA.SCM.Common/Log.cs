using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace FESA.SCM.Common
{
    public class Log
    {
        public static void Write(string message)
        {
            Logger.Write(message);
        }

        public static void Write(object message)
        {
            var exception = (Exception)message;
            var logEntry = new LogEntry
            {
                Title = exception.Message,
                Message = exception.StackTrace,
                Priority = 5,
                Severity = TraceEventType.Error,
            };
            Logger.Write(logEntry);
        }
    }
}