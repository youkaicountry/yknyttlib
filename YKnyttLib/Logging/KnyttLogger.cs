using System;
using System.Collections.Generic;

namespace YKnyttLib.Logging
{
    public static class KnyttLogger
    {
        private static List<IKnyttLoggerTarget> targets;
        private static Level logLevel;

        public enum Level
        {
            TRACE = 0,
            DEBUG = 1,
            INFO = 2,
            WARN = 3,
            ERROR = 4,
            FATAL = 5,
            OFF = 6,
        }

        static KnyttLogger()
        {
            targets = new List<IKnyttLoggerTarget>();
            logLevel = Level.INFO;
        }

        public static void AddTarget(IKnyttLoggerTarget target)
        {
            targets.Add(target);
        }

        public static void SetLogLevel(Level level)
        {
            logLevel = level;
        }

        public static void Log(Level level, string message, KnyttPoint? worldPos=null, KnyttPoint? screenPos=null)
        {
            // Ensure logger is showing messages of this level
            if (level < logLevel) { return; }

            // Construct the message
            var m = new KnyttLogMessage() {Timestamp=DateTime.Now, Level=level, Value=message, WorldPos=worldPos, ScreenPos=screenPos};

            // Pass it along
            foreach (var target in targets)
            {
                target.NewMessage(m);
            }
        }
    }
}
