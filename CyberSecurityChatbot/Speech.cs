using System;

namespace CyberSecurityChatbot
{
    public class Speech
    {
        public void Speak(string text)
        {
            try
            {
                dynamic voice = Activator.CreateInstance(Type.GetTypeFromProgID("SAPI.SpVoice"));
                voice.Speak(text);
            }
            catch
            {
                Console.WriteLine("[Speech not supported]");
            }
        }
    }
}