using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Input;

namespace CyberSecurityChatBot
{
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer speaker = new SpeechSynthesizer();

        private ChatBot bot = new ChatBot();

        public MainWindow()
        {
            InitializeComponent();

            string greeting =
                "Welcome to the Cybersecurity Awareness Chatbot. What is your name?";

            ChatDisplay.Text = "Bot: " + greeting;

            speaker.SpeakAsync(greeting);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void SendMessage()
        {
            string input = UserInput.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            ChatDisplay.Text += "\n\nYou: " + input;

            string response = bot.ProcessInput(input);

            ChatDisplay.Text += "\nBot: " + response;

            speaker.SpeakAsync(response);

            UserInput.Clear();
        }
    }
}

