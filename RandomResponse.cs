using System;

namespace CyberSecurityChatbot
{
    public class RandomResponse
    {
        private Random random = new Random();

        public string GetPasswordTip()
        {
            string[] responses =
            {
                "Use a password that is at least 12 characters long.",
                "Never reuse passwords across multiple accounts.",
                "Use a password manager to store your passwords safely.",
                "Combine uppercase, lowercase, numbers and symbols.",
                "Enable Multi-Factor Authentication whenever possible."
            };

            return responses[random.Next(responses.Length)];
        }

        public string GetPhishingTip()
        {
            string[] responses =
            {
                "Never click suspicious email links.",
                "Always verify the sender's email address.",
                "Be cautious of urgent messages requesting personal information.",
                "Hover over links before clicking them.",
                "Report phishing emails to your IT department."
            };

            return responses[random.Next(responses.Length)];
        }

        public string GetGeneralTip()
        {
            string[] responses =
            {
                "Keep your software updated.",
                "Install trusted antivirus software.",
                "Backup important files regularly.",
                "Avoid using public Wi-Fi without a VPN.",
                "Lock your computer when you leave it unattended."
            };

            return responses[random.Next(responses.Length)];
        }
    }
}
