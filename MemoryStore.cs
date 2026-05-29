namespace CyberSecurityChatBot
{
    public class MemoryStore
    {
        public string UserName { get; set; } = "";
        public string FavouriteTopic { get; set; } = "";
        public string FavoriteTopic
        {
            get => FavouriteTopic;
            set => FavouriteTopic = value;
        }

        public string LastTopic { get; set; } = "";
    }

    // ChatBot implementation lives in Chatbot.cs. Keep MemoryStore focused on storing user data.
}
