namespace CyberSecurityChatBot
{
    public class ChatBot
    {
        private KeywordResponder responder;
        private SentimentDetector sentimentDetector;
        private MemoryStore memory;

        private bool waitingForName = true;

        public ChatBot()
        {
            responder = new KeywordResponder();
            sentimentDetector = new SentimentDetector();
            memory = new MemoryStore();
        }

        public string ProcessInput(string input)
        {
            // ASK FOR NAME FIRST
            if (waitingForName)
            {
                memory.UserName = input;

                waitingForName = false;

                return "Hello " + memory.UserName +
                       "! How can I help you stay safe online today?";
            }

            string sentiment =
                sentimentDetector.DetectSentiment(input);

            string sentimentMessage =
                sentimentDetector.GetSentimentResponse(sentiment);

            // FOLLOW-UP FEATURE
            if (input.ToLower().Contains("tell me more"))
            {
                if (memory.LastTopic != null)
                {
                    return GetMoreInfo(memory.LastTopic);
                }

                return "Please ask about a cybersecurity topic first.";
            }

            // KEYWORD RESPONSE
            string response = responder.GetResponse(input);

            if (response != null)
            {
                if (input.ToLower().Contains("password"))
                {
                    memory.LastTopic = "password";
                }

                else if (input.ToLower().Contains("phishing"))
                {
                    memory.LastTopic = "phishing";
                }

                else if (input.ToLower().Contains("privacy"))
                {
                    memory.LastTopic = "privacy";
                }

                else if (input.ToLower().Contains("malware"))
                {
                    memory.LastTopic = "malware";
                }

                else if (input.ToLower().Contains("vpn"))
                {
                    memory.LastTopic = "vpn";
                }

                // ADD NAME TO RESPONSE
                response = memory.UserName + ", " + response;

                // SENTIMENT RESPONSE
                if (!string.IsNullOrEmpty(sentimentMessage))
                {
                    response =
                        sentimentMessage + "\n" + response;
                }

                return response;
            }

            return "I do not have an answer for that yet. Please ask a cybersecurity question.";
        }

        private string GetMoreInfo(string topic)
        {
            switch (topic)
            {
                case "password":
                    return "Strong passwords should contain at least 12 characters and avoid personal information.";

                case "phishing":
                    return "Phishing attacks often pretend to be trusted companies.";

                case "privacy":
                    return "Privacy online means protecting your personal information.";

                case "malware":
                    return "Malware includes viruses, spyware, and ransomware.";

                case "vpn":
                    return "VPNs help secure your connection on public WiFi.";

                default:
                    return "I do not have more information on that topic.";
            }
        }
    }
}