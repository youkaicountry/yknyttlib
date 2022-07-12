using System;
using System.Text;
using System.Globalization;

namespace YKnyttLib.Logging
{
    public struct KnyttLogMessage
    {
        public KnyttLogger.Level Level;
        public string Value;
        public KnyttPoint? WorldPos;
        public KnyttPoint? ScreenPos;
        public DateTime Timestamp;

        public string Render()
        {
            var sb = new StringBuilder();
            sb.Append(Timestamp);
            sb.Append($" {Value}");
            return sb.ToString();
        }
    }
}
