using System;

namespace CyberSecurityChatbot
{
    public static class Chatbot
    {
        public static void StartChat(User user, Speech speech)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\n{user.Name}: ");
                Console.ResetColor();

                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Please enter something.");
                    continue;
                }

                input = input.ToLower();

                if (input == "exit")
                {
                    Console.WriteLine("Goodbye!");
                    speech.Speak("Goodbye");
                    break;
                }

                Respond(input, user.Name, speech);
            }
        }

        private static void Respond(string input, string name, Speech speech)
        {
            string response;

            if (input.Contains("how are you"))
                response = "I am running perfectly.";

            else if (input.Contains("what is your purpose"))
                response = "I help you learn cybersecurity safety.";

            else if (input.Contains("how can i create a strong password"))
                response = "Use strong passwords with symbols, numbers, and uppercase letters.";

            else if (input.Contains("what is phishing"))
                response = "Phishing is when attackers trick you into giving personal information.";

            else if (input.Contains("what is malware") || input.Contains("what is a virus"))
                response = "Malware is harmful software that damages your device.";

            else if (input.Contains("what is two-factor authentication") || input.Contains("what is 2fa"))
                response = "Two-factor authentication adds an extra layer of security to your accounts.";

            else if (input.Contains("what is ransomware"))
                response = "Ransomware is a type of malware that locks your files and demands payment to unlock them.";

            else if (input.Contains("what is safe browsing"))
                response = "Safe browsing means being cautious about the websites you visit and the links you click on.";

            else
                response = "I don't understand. Try asking something else.";

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Bot: " + response);
            Console.ResetColor();

            speech.Speak(response);
        }
    }
}
