﻿using Helpers;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace WCFService
{
    internal static class EventLogger
    {
        private static readonly string source = ConfigHelper.GetString("EventLogSource");
        private static readonly string logName = ConfigHelper.GetString("EventLogName");
        private static readonly int attemptLimit = int.Parse(ConfigHelper.GetString("AttemptLimit"));
        private static readonly int attemptTimeout = int.Parse(ConfigHelper.GetString("AttemptTimeout"));

        private static readonly ConcurrentDictionary<int, int> attempts;
        private static readonly Timer timer;

        static EventLogger()
        {
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, logName);
            }

            attempts = new ConcurrentDictionary<int, int>();
            timer = new Timer(DecreaseAttempts, null, attemptTimeout, attemptTimeout);
        }

        private static void DecreaseAttempts(object state)
        {
            foreach (int entry in attempts.Keys)
            {
                if (attempts[entry] > 0)
                {
                    attempts[entry]--;
                }
            }
        }

        public static void IncreaseAttemps(int entryId)
        {
            attempts.TryAdd(entryId, 0);
            attempts[entryId]++;

            if (attempts[entryId] >= attemptLimit)
            {
                using (IDSServiceClient client = new IDSServiceClient())
                {
                    client.Alarm(string.Format(ResourceHelper.GetString("Alarm"), entryId));
                }

                attempts[entryId] = 0;
            }
        }

        public static void AuthenticationSuccess(string username)
        {
            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                log.WriteEntry(string.Format(ResourceHelper.GetString("AuthenticationSuccess"), username), EventLogEntryType.SuccessAudit);
            }
        }

        public static void AuthorizationSuccess(string username, string action)
        {
            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                log.WriteEntry(string.Format(ResourceHelper.GetString("AuthorizationSuccess"), username, action), EventLogEntryType.SuccessAudit);
            }
        }

        public static void AuthorizationFailure(string username, string action, string permission)
        {
            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                log.WriteEntry(string.Format(ResourceHelper.GetString("AuthorizationFailurePermission"), username, action, permission), EventLogEntryType.FailureAudit);
            }
        }

        public static void AuthorizationFailure(string username, string action)
        {
            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                log.WriteEntry(string.Format(ResourceHelper.GetString("AuthorizationFailureKey"), username, action), EventLogEntryType.FailureAudit);
            }
        }
    }
}
