using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public class KeywordResponder
    {
        private Dictionary<string, List<string>> _keywordResponses;
        private Dictionary<string, List<string>> _intentPhrases;

        public KeywordResponder()
        {
            InitializeKeywordResponses();
            InitializeIntentPhrases();
        }

        private void InitializeKeywordResponses()
        {
            _keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["phishing"] = new List<string>
                {
                    "Phishing attacks try to trick you into revealing sensitive information.  Always verify the sender before clicking links!",
                    "Be cautious of suspicious emails asking for personal information. Report phishing attempts to your IT department! ",
                    "Remember: legitimate companies never ask for passwords via email. Stay vigilant against phishing! "
                },
                ["password"] = new List<string>
                {
                    "Use strong passwords with a mix of uppercase, lowercase, numbers, and special characters. ",
                    "Never share your password with anyone and use unique passwords for each account! ",
                    "Consider using a password manager to generate and store strong passwords securely. "
                },
                ["privacy"] = new List<string>
                {
                    "Review your privacy settings regularly on all platforms. ",
                    "Limit the personal information you share online to protect your privacy. ",
                    "Be aware of what information you're sharing on social media. Privacy matters! "
                },
                ["scam"] = new List<string>
                {
                    "Scammers often create urgency to make you act without thinking. Take a moment to verify! ",
                    "If something seems too good to be true, it probably is a scam. Trust your instincts! ",
                    "Report scams to relevant authorities to help protect others from falling victim. "
                },
                ["malware"] = new List<string>
                {
                    "Keep your antivirus software updated to protect against malware. ",
                    "Avoid downloading files from untrusted sources to prevent malware infections. ",
                    "Regular system scans can detect and remove malware before it causes damage. "
                },
                ["2fa"] = new List<string>
                {
                    "Two-factor authentication adds an extra layer of security to your accounts! ",
                    "Enable 2FA wherever possible to protect your accounts from unauthorized access. ",
                    "2FA combines something you know (password) with something you have (phone). Smart security! "
                },
                ["vpn"] = new List<string>
                {
                    "VPNs encrypt your internet traffic, protecting your privacy on public networks. ",
                    "Always use a VPN when connecting to public Wi-Fi to keep your data secure. ",
                    "A good VPN masks your IP address and encrypts your online activities. "
                }
            };
        }

        private void InitializeIntentPhrases()
        {
            _intentPhrases = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["addtask"] = new List<string>
                {
                    "add task", "add a task", "create task", "create a task",
                    "I need to", "I should", "remind me to", "don't forget to",
                    "enable", "set up", "configure", "implement"
                },
                ["setreminder"] = new List<string>
                {
                    "remind me", "reminder", "set a reminder", "set reminder",
                    "remind me to", "remember to", "don't forget", "notify me",
                    "in days", "tomorrow", "next week"
                },
                ["startquiz"] = new List<string>
                {
                    "start quiz", "take quiz", "quiz me", "test my knowledge",
                    "play quiz", "begin quiz", "start the quiz", "take the quiz",
                    "cybersecurity quiz", "knowledge test"
                },
                ["showlog"] = new List<string>
                {
                    "show activity log", "what have you done", "what did you do",
                    "show log", "recent actions", "activity log", "show activities",
                    "what have you been doing", "show me the log"
                },
                ["showmore"] = new List<string>
                {
                    "show more", "see more", "more entries", "show all",
                    "display all", "full log", "complete log"
                }
            };
        }

        public string GetResponseForKeyword(string input)
        {
            foreach (var keyword in _keywordResponses.Keys)
            {
                if (input.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    var responses = _keywordResponses[keyword];
                    return responses[new Random().Next(responses.Count)];
                }
            }
            return null;
        }

        public string DetectIntent(string input)
        {
            string lowerInput = input.ToLower();

            foreach (var intent in _intentPhrases)
            {
                foreach (var phrase in intent.Value)
                {
                    if (lowerInput.Contains(phrase.ToLower()))
                    {
                        return intent.Key;
                    }
                }
            }
            return "unknown";
        }

        public bool HasKeywordMatch(string input)
        {
            foreach (var keyword in _keywordResponses.Keys)
            {
                if (input.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}