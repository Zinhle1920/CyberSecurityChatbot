using System;
using System.Collections.Generic;

namespace CyberSecurityChatBot
{
    public class KeywordResponder
    {
        private Random random = new Random();

        private Dictionary<string, List<string>> responses =
            new Dictionary<string, List<string>>()
        {
            {
                "password",
                new List<string>()
                {
                    "Use strong passwords with symbols, numbers, and uppercase letters.",
                    "Never reuse passwords across multiple accounts.",
                    "A password manager can help you store passwords securely."
                }
            },

            {
                "phishing",
                new List<string>()
                {
                    "Phishing scams trick users into giving away sensitive information.",
                    "Never click suspicious email links.",
                    "Always verify the sender before opening attachments."
                }
            },

            {
                "privacy",
                new List<string>()
                {
                    "Protect your privacy by limiting personal information online.",
                    "Review your privacy settings regularly.",
                    "Avoid sharing sensitive data on public platforms."
                }
            },

            {
                "malware",
                new List<string>()
                {
                    "Malware is harmful software designed to damage systems.",
                    "Install antivirus software to help prevent malware infections.",
                    "Avoid downloading files from unknown websites."
                }
            },

            {
                "vpn",
                new List<string>()
                {
                    "A VPN encrypts your internet connection for safer browsing.",
                    "VPNs help protect your privacy on public WiFi.",
                    "Using a VPN can reduce tracking online."
                }
            }
        };

        public string? GetResponse(string? input)
        {
            string inputLower = input?.ToLower() ?? string.Empty;

            foreach (var keyword in responses.Keys)
            {
                if (inputLower.Contains(keyword))
                {
                    List<string> possibleResponses = responses[keyword];

                    int index = random.Next(possibleResponses.Count);

                    return possibleResponses[index];
                }
            }

            return null;
        }
    }
}