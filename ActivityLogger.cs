using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatBot
{
    public class ActivityLogger
    {
        private List<string> _log = new List<string>();

        public void Log(string action)
        {
            string entry = $"[{DateTime.Now.ToString("HH:mm")}] {action}";
            _log.Add(entry);
        }

        public string GetRecentLog(int count = 10)
        {
            if (_log.Count == 0)
                return "No activities logged yet.";

            int entriesToShow = Math.Min(count, _log.Count);
            var recentEntries = _log.Skip(_log.Count - entriesToShow).ToList();

            string result = " Here's a summary of recent actions:\n";
            for (int i = 0; i < recentEntries.Count; i++)
            {
                result += $"{i + 1}. {recentEntries[i]}\n";
            }

            if (_log.Count > count)
            {
                result += $"\n💡 There are {_log.Count - count} more entries. Type 'show more' to see all.";
            }

            return result.Trim();
        }

        public string GetFullLog()
        {
            if (_log.Count == 0)
                return "No activities logged yet.";

            string result = " Complete Activity Log:\n";
            for (int i = 0; i < _log.Count; i++)
            {
                result += $"{i + 1}. {_log[i]}\n";
            }
            return result.Trim();
        }

        public int GetCount()
        {
            return _log.Count;
        }
    }
}