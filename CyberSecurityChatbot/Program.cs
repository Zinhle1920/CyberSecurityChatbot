

using System;


namespace CyberSecurityChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Speech speech = new Speech();

            // 🎨 HEADER
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===========================================");
            Console.WriteLine(" HELLO,WELCOME TO CYBERSECURITY CHATBOT");
            Console.WriteLine("===========================================");
            Console.ResetColor();

            // 🎨 ASCII ART
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"
  ____      _                ____        _
 / ___|   _| |__   ___ _ __ | __ )  ___ | |_
| |  | | | | '_ \ / _ \ '_ \|  _ \ / _ \| __|
| |__| |_| | |_) |  __/ | | | |_) | (_) | |_
 \____\__, |_.__/ \___|_| |_|____/ \___/ \__|
      |___/
");
            Console.ResetColor();

            // 🔊 INTRO SPEECH
            speech.Speak("Welcome to the Cybersecurity Awareness Bot.");

            // 👤 USER INPUT
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "User";
            }

            User user = new User();
            user.Name = name;

            // 🔊 GREETING
            speech.Speak("Hello " + user.Name + ". Welcome to the Cybersecurity Awareness Bot. I am here to help you stay safe online.");

            // 🤖 START CHAT
            Chatbot.StartChat(user, speech);
        }
    }
}
