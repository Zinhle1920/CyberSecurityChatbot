namespace CyberSecurityChatBot
{
    public class SentimentDetector
    {
        public string DetectSentiment(string input)
        {
            input = input.ToLower();

            if (input.Contains("worried") ||
                input.Contains("scared") ||
                input.Contains("afraid"))
            {
                return "worried";
            }

            if (input.Contains("angry") ||
                input.Contains("frustrated") ||
                input.Contains("annoyed"))
            {
                return "frustrated";
            }

            if (input.Contains("curious") ||
                input.Contains("interested"))
            {
                return "curious";
            }

            return "neutral";
        }

        public string GetSentimentResponse(string sentiment)
        {
            switch (sentiment)
            {
                case "worried":
                    return "I understand your concern. Cybersecurity can feel overwhelming sometimes.";

                case "frustrated":
                    return "I understand this can be frustrating. Let us solve it together.";

                case "curious":
                    return "Curiosity is great. Learning cybersecurity helps keep you safe online.";

                default:
                    return "";
            }
        }
    }
}