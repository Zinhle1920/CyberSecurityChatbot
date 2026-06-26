using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot
{
    public class SentimentDetector
    {
        private List<string> _positiveWords = new List<string>
        {
            "happy", "great", "good", "excellent", "wonderful", "amazing", "fantastic",
            "secure", "safe", "confident", "relieved", "thankful", "grateful"
        };

        private List<string> _negativeWords = new List<string>
        {
            "worried", "anxious", "scared", "fear", "concern", "danger", "threat",
            "unsafe", "vulnerable", "compromised", "hacked", "breached", "scam"
        };

        public string DetectSentiment(string input)
        {
            string lowerInput = input.ToLower();

            // Check for emotional indicators
            if (lowerInput.Contains("i am") || lowerInput.Contains("i'm"))
            {
                foreach (var word in _negativeWords)
                {
                    if (lowerInput.Contains(word))
                        return "negative";
                }

                foreach (var word in _positiveWords)
                {
                    if (lowerInput.Contains(word))
                        return "positive";
                }
            }

            // Check for general sentiment words
            foreach (var word in _negativeWords)
            {
                if (lowerInput.Contains(word))
                    return "negative";
            }

            foreach (var word in _positiveWords)
            {
                if (lowerInput.Contains(word))
                    return "positive";
            }

            return "neutral";
        }

        public string GetSentimentResponse(string sentiment)
        {
            switch (sentiment)
            {
                case "negative":
                    return " I understand you're concerned. Here's a tip: " + GetCybersecurityTip();
                case "positive":
                    return " That's great to hear! Stay safe online! Here's a tip: " + GetCybersecurityTip();
                default:
                    return null;
            }
        }

        private string GetCybersecurityTip()
        {
            var tips = new List<string>
            {
                "Always enable two-factor authentication on important accounts.",
                "Use unique, strong passwords for every account.",
                "Be cautious of suspicious emails and never click on unknown links.",
                "Keep your software and antivirus programs updated.",
                "Back up important data regularly to prevent loss.",
                "Use a VPN when connecting to public Wi-Fi.",
                "Review your privacy settings on social media regularly."
            };
            return tips[new Random().Next(tips.Count)];
        }
    }
}