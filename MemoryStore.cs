using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public class MemoryStore
    {
        private string _lastTopic = "";
        private int _followUpCount = 0;
        private Dictionary<string, int> _topicCount = new Dictionary<string, int>();
        private List<string> _conversationHistory = new List<string>();

        public void RememberTopic(string topic)
        {
            if (!string.IsNullOrEmpty(topic))
            {
                _lastTopic = topic;
                _followUpCount = 0;

                if (!_topicCount.ContainsKey(topic))
                    _topicCount[topic] = 0;
                _topicCount[topic]++;
            }
        }

        public string GetLastTopic()
        {
            return _lastTopic;
        }

        public string GetFollowUpResponse(string userInput)
        {
            if (string.IsNullOrEmpty(_lastTopic))
                return null;

            _followUpCount++;

            var followUps = new Dictionary<string, List<string>>
            {
                ["phishing"] = new List<string>
                {
                    "Phishing attacks often impersonate legitimate organizations. Always check the sender's email address!",
                    "Remember: legitimate companies never ask for your password via email.",
                    "If you're unsure about an email, contact the organization directly using a known phone number.",
                    "Phishing emails often create urgency. Take a moment to verify before acting!"
                },
                ["password"] = new List<string>
                {
                    "Consider using a password manager to generate and store strong passwords.",
                    "Enable two-factor authentication for an extra layer of security.",
                    "Change passwords immediately if you suspect they've been compromised.",
                    "Use different passwords for different accounts to limit damage if one is compromised."
                },
                ["privacy"] = new List<string>
                {
                    "Review which apps have access to your personal information.",
                    "Be mindful of what you share on social media - oversharing can be risky.",
                    "Use privacy-focused browsers and search engines.",
                    "Regularly check your account security settings."
                }
            };

            if (followUps.ContainsKey(_lastTopic.ToLower()))
            {
                var responses = followUps[_lastTopic.ToLower()];
                int index = Math.Min(_followUpCount - 1, responses.Count - 1);
                return responses[index];
            }

            return null;
        }

        public void AddToHistory(string userInput, string botResponse)
        {
            _conversationHistory.Add($"User: {userInput}");
            _conversationHistory.Add($"Bot: {botResponse}");

            // Keep only last 50 exchanges
            if (_conversationHistory.Count > 100)
                _conversationHistory.RemoveRange(0, _conversationHistory.Count - 100);
        }

        public string GetHistory()
        {
            if (_conversationHistory.Count == 0)
                return "No conversation history yet.";

            return string.Join("\n", _conversationHistory);
        }
    }
}