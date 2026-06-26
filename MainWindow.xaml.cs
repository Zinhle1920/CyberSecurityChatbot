using CyberSecurityChatbot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CyberSecurityChatBot
{
    public partial class MainWindow : Window
    {
        // ============================================
        // VARIABLES
        // ============================================
        private SpeechSynthesizer synthesizer;
        private string userName;
        private bool nameAsked;
        private Random random;

        // Cybersecurity knowledge base
        private Dictionary<string, string> cybersecurityAnswers;

        // Emotional sentiment keywords
        private HashSet<string> positiveWords;
        private HashSet<string> negativeWords;
        private HashSet<string> fearWords;

        // Randomized responses for unknown questions
        private string[] fallbackResponses;

        // Conversation memory with timestamps
        private ObservableCollection<ConversationMessage> conversationMemory;

        // ============================================
        // NEW FEATURES: Task, Quiz, Activity Log
        // ============================================
        private ActivityLogger _logger;
        private TaskManager _taskManager;
        private TaskStorageHelper _taskStorage;
        private QuizManager _quizManager;
        private bool _isQuizMode = false;

        // ============================================
        // CONVERSATION MESSAGE CLASS
        // ============================================
        public class ConversationMessage
        {
            public string Sender { get; set; }
            public string Message { get; set; }
            public string Timestamp { get; set; }
        }

        // ============================================
        // CONSTRUCTOR
        // ============================================
        public MainWindow()
        {
            InitializeComponent();
            InitializeComponents();
            InitializeUI();
            InitializeSpeechSynthesizer();
            LoadTasks();
        }

        // ============================================
        // INITIALIZATION METHODS
        // ============================================
        private void InitializeUI()
        {
            // Initialize conversation memory
            conversationMemory = new ObservableCollection<ConversationMessage>();
            ChatListBox.ItemsSource = conversationMemory;

            AppendMessage("Bot", "Hello! I'm your CyberSecurity Awareness Bot. What is your name?");
            StatusText.Text = "Ready to help you stay safe online!";
        }

        private void InitializeComponents()
        {
            random = new Random();
            nameAsked = false;
            userName = null;

            // Initialize new features
            _logger = new ActivityLogger();
            _taskStorage = new TaskStorageHelper();
            _taskManager = new TaskManager(_logger);
            _quizManager = new QuizManager();

            InitializeCybersecurityKnowledge();
            InitializeSentimentKeywords();
            InitializeFallbackResponses();

            _logger.Log("Application started");
        }

        private void InitializeSpeechSynthesizer()
        {
            try
            {
                synthesizer = new SpeechSynthesizer();
                synthesizer.Volume = 80;
                synthesizer.Rate = 0;
                synthesizer.SpeakAsync("Hello! I'm your CyberSecurity Awareness Bot. What is your name?");
            }
            catch (Exception ex)
            {
                // Speech synthesizer might not be available
                Console.WriteLine($"Speech error: {ex.Message}");
            }
        }

        // ============================================
        // CYBERSECURITY KNOWLEDGE BASE
        // ============================================
        private void InitializeCybersecurityKnowledge()
        {
            cybersecurityAnswers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            cybersecurityAnswers["phishing"] = "Phishing is when attackers pretend to be trustworthy (like fake bank emails) to steal your information. Always verify sender emails, never click suspicious links, and hover over links first to see the real URL.";
            cybersecurityAnswers["phishing email"] = "Phishing emails create urgency ('Account closed!'), use generic greetings ('Dear Customer'), have spelling errors, and ask for personal info. Verify by calling the company directly.";
            cybersecurityAnswers["spear phishing"] = "Spear phishing targets specific people or companies. Attackers research you to seem legitimate. Be extra careful with emails requesting sensitive data, even from known contacts.";
            cybersecurityAnswers["password"] = "Use 12+ character passwords with uppercase, lowercase, numbers, and symbols. Never reuse passwords. Use a password manager (Bitwarden, 1Password). Enable 2FA everywhere!";
            cybersecurityAnswers["password security"] = "Password tips: 1) Unique passwords for each account 2) 12+ characters 3) Use password manager 4) Enable 2FA 5) Change immediately if breached 6) Never share via email.";
            cybersecurityAnswers["2fa"] = "Two-Factor Authentication (2FA) adds extra security. Even with your password stolen, attackers need your second factor (Google Authenticator, Authy, YubiKey). Always enable 2FA!";
            cybersecurityAnswers["multifactor authentication"] = "MFA requires multiple checks: something you know (password), have (phone), or are (fingerprint). Use authenticator apps over SMS - SIM swapping can hack SMS codes.";
            cybersecurityAnswers["malware"] = "Malware includes viruses, worms, trojans, ransomware, spyware. Protect yourself: use antivirus, keep software updated, don't download from suspicious sites, be careful with email attachments, backup data regularly.";
            cybersecurityAnswers["ransomware"] = "Ransomware encrypts your files and demands payment. Prevention: backup regularly (3-2-1 rule), update systems, avoid suspicious attachments, use antivirus. NEVER pay ransom - it doesn't guarantee recovery.";
            cybersecurityAnswers["virus"] = "Viruses are malicious programs that infect files. Signs: slow PC, pop-ups, crashes, missing files. Fix: run antivirus, disconnect from internet, restore from clean backup.";
            cybersecurityAnswers["spyware"] = "Spyware secretly monitors you and steals info. Protection: use antivirus, avoid untrusted downloads, keep software updated, use ad blockers, check browser extensions, watch for unusual bandwidth use.";
            cybersecurityAnswers["scam"] = "Common scams: romance, lottery, tech support, crypto, job scams. Red flags: payment in crypto/gift cards, urgency, too-good-to-be-true, personal info requests. Always verify independently.";
            cybersecurityAnswers["tech support scam"] = "Tech support scammers claim to be from Microsoft/Apple, asking for remote access or payment. REAL companies NEVER call unrequested. Hang up, don't pay, contact company officially.";
            cybersecurityAnswers["romance scam"] = "Romance scammers build fake relationships to steal money. Warning signs: quick love declarations, no video calls, money requests for emergencies, inconsistent stories, won't meet. Never send money to strangers.";
            cybersecurityAnswers["crypto scam"] = "Crypto scams include fake exchanges and investment schemes. Red flags: guaranteed returns, pressure to invest fast, unsolicited offers, sending crypto first. Use reputable exchanges only. If too good to be true, it is.";
            cybersecurityAnswers["vpn"] = "VPN encrypts traffic and hides your IP. Benefits: protects on public WiFi, bypasses geo-blocks, prevents ISP tracking. Use paid VPNs (ExpressVPN, NordVPN, ProtonVPN). Avoid free VPNs - they sell your data.";
            cybersecurityAnswers["public wifi"] = "Public WiFi is dangerous - attackers intercept data. Always use VPN on public WiFi, avoid banking, disable file sharing, forget network after use, verify network name with staff to avoid fake networks.";
            cybersecurityAnswers["privacy"] = "Privacy protection: use privacy browsers (Firefox, Brave), install extensions (uBlock Origin), clear cookies regularly, use encrypted messaging (Signal), limit social media sharing, review app permissions.";
            cybersecurityAnswers["cookies"] = "Cookies track browsing. Manage: clear regularly, use privacy settings, block third-party cookies, use incognito for sensitive browsing. Some cookies are necessary (keeping you logged in).";
            cybersecurityAnswers["data breach"] = "If breached: 1) Change passwords immediately 2) Enable 2FA 3) Monitor accounts 4) Check HaveIBeenPwned.com 5) Consider credit monitoring 6) Watch for phishing using your leaked data.";
            cybersecurityAnswers["security"] = "Cybersecurity basics: strong unique passwords, enable 2FA, update software, be careful with emails/links, use antivirus, backup data, use VPN on public WiFi, don't share personal info, verify before clicking.";
            cybersecurityAnswers["safe browsing"] = "Safe browsing: check HTTPS (padlock), watch for suspicious URLs, don't click pop-ups, download from official sources only, keep browser updated, use ad blockers, trust your instincts.";
            cybersecurityAnswers["social engineering"] = "Social engineering manipulates people into revealing info. Tactics: urgency, fake authority, social proof, fear. Defense: verify identities, don't rush, be skeptical of requests, report suspicious attempts.";
        }

        private void InitializeSentimentKeywords()
        {
            positiveWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "great", "good", "awesome", "excellent", "thanks", "thank", "helpful",
                "love", "happy", "glad", "appreciate", "nice", "wonderful", "fantastic"
            };

            negativeWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "bad", "terrible", "awful", "horrible", "annoying", "frustrating",
                "useless", "waste", "hate", "angry", "upset", "disappointed"
            };

            fearWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "scared", "afraid", "worried", "anxious", "nervous", "fear", "panic",
                "dangerous", "threat", "unsafe", "concerned", "hacked", "stolen"
            };
        }

        private void InitializeFallbackResponses()
        {
            fallbackResponses = new string[]
            {
                "Great question! Check CISA.gov or NIST.gov for authoritative cybersecurity information.",
                "For that topic, visit HaveIBeenPwned.com or the FTC's cybersecurity page for details.",
                "Always verify cybersecurity info from official sources and be skeptical of unsolicited advice.",
                "Check your government's cybersecurity agency for accurate information on that topic.",
                "Consult cybersecurity professionals or CISA.gov for accurate information on that topic."
            };
        }

        // ============================================
        // CORE METHODS
        // ============================================

        private void AppendMessage(string sender, string message)
        {
            var chatMessage = new ConversationMessage
            {
                Sender = sender,
                Message = message,
                Timestamp = DateTime.Now.ToString("HH:mm")
            };

            conversationMemory.Add(chatMessage);

            // Auto-scroll to latest message
            if (ChatListBox.Items.Count > 0)
            {
                ChatListBox.ScrollIntoView(ChatListBox.Items[ChatListBox.Items.Count - 1]);
            }
        }

        private void SpeakText(string text)
        {
            if (synthesizer != null)
            {
                try
                {
                    synthesizer.SpeakAsyncCancelAll();
                    synthesizer.SpeakAsync(text);
                }
                catch { /* Ignore speech errors */ }
            }
        }

        private string DetectSentiment(string input)
        {
            input = input.ToLower();
            int score = 0;

            foreach (string word in positiveWords)
            {
                if (input.Contains(word)) score++;
            }

            foreach (string word in negativeWords)
            {
                if (input.Contains(word)) score--;
            }

            foreach (string word in fearWords)
            {
                if (input.Contains(word)) score -= 2;
            }

            if (score >= 2) return "positive";
            if (score <= -2) return "negative";
            if (score < 0) return "concerned";
            return "neutral";
        }

        private string GetSentimentalResponse(string sentiment)
        {
            switch (sentiment)
            {
                case "positive":
                    return "Glad I could help! Staying informed is your best defense. Anything else?";
                case "negative":
                    return "I understand this can be frustrating. Cybersecurity is complex. What's bothering you?";
                case "concerned":
                    return "Being concerned is good - awareness is your first defense. Let me help you protect yourself.";
                default:
                    return null;
            }
        }

        // ============================================
        // TASK METHODS
        // ============================================

        private void LoadTasks()
        {
            try
            {
                var tasks = _taskManager.GetAllTasks();
                TaskListBox.ItemsSource = tasks;
                TaskCountText.Text = $"Tasks: {tasks.Count} | Ready";
            }
            catch (Exception ex)
            {
                AppendMessage("Bot", $"⚠️ Error loading tasks: {ex.Message}");
            }
        }

        private string ExtractTaskTitle(string input)
        {
            string lowerInput = input.ToLower();

            // Try common patterns
            if (lowerInput.Contains("add task") || lowerInput.Contains("create task"))
            {
                // Extract after "add task"
                if (lowerInput.Contains("add task"))
                {
                    int index = lowerInput.IndexOf("add task") + 8;
                    if (index < input.Length)
                    {
                        string title = input.Substring(index).Trim();
                        // Remove "to" if it exists
                        if (title.ToLower().StartsWith("to "))
                            title = title.Substring(3);
                        return title;
                    }
                }
            }

            // If it contains "enable" or "set up" - use the whole phrase
            if (lowerInput.Contains("enable") || lowerInput.Contains("set up"))
            {
                return input.Trim();
            }

            return null;
        }

        private string HandleTaskIntent(string input)
        {
            string taskTitle = ExtractTaskTitle(input);

            if (string.IsNullOrEmpty(taskTitle) || taskTitle.Length < 3)
            {
                return "I need a task description. Try: 'Add task - Review privacy settings'";
            }

            // Check if task already exists
            if (_taskManager.TaskExists(taskTitle))
            {
                return $"⚠️ Task '{taskTitle}' already exists. Try a different task.";
            }

            string result = _taskManager.AddTask(taskTitle, taskTitle, "");
            LoadTasks();
            return result + " Would you like to set a reminder?";
        }

        // ============================================
        // QUIZ METHODS
        // ============================================

        private void StartQuiz()
        {
            _quizManager.ResetQuiz();
            _isQuizMode = true;
            _logger.Log("Quiz started");

            var question = _quizManager.GetCurrentQuestion();
            string display = DisplayQuestion(question);

            AppendMessage("Bot", display);
            SpeakText("Starting quiz. Answer each question by typing your answer.");
        }

        private string ProcessQuizAnswer(string userInput)
        {
            var currentQuestion = _quizManager.GetCurrentQuestion();
            if (currentQuestion == null)
            {
                _isQuizMode = false;
                return "The quiz hasn't started. Type 'start quiz' to begin!";
            }

            // Submit the answer
            bool isCorrect = _quizManager.SubmitAnswer(userInput);
            string feedback = _quizManager.GetFeedback(isCorrect);

            // Check if quiz is finished
            if (_quizManager.IsFinished())
            {
                _logger.Log($"Quiz completed - score: {_quizManager.GetFinalScore()}");
                _isQuizMode = false;

                string score = _quizManager.GetFinalScore();
                string message = _quizManager.GetFinalMessage();
                return $"{feedback}\n\n📊 Final Score: {score}\n{message}";
            }

            // Get next question
            var nextQuestion = _quizManager.GetCurrentQuestion();
            return $"{feedback}\n\n{DisplayQuestion(nextQuestion)}";
        }

        private string DisplayQuestion(QuizQuestion question)
        {
            if (question == null)
                return "No more questions!";

            string display = $"📝 Question {_quizManager.GetCurrentIndex() + 1}/{_quizManager.GetTotalQuestions()}\n";
            display += $"{question.Question}\n\n";

            if (question.IsTrueFalse)
            {
                display += "Type 'True' or 'False'";
            }
            else
            {
                foreach (var option in question.Options)
                {
                    display += $"{option}\n";
                }
                display += "\nType the letter (A, B, C, or D)";
            }

            display += $"\n\nScore: {_quizManager.GetScore()}/{_quizManager.GetCurrentIndex()}";
            return display;
        }

        // ============================================
        // MAIN PROCESSING METHOD
        // ============================================

        private string GetCybersecurityResponse(string input)
        {
            string lowerInput = input.ToLower();

            // ============================================
            // 1. QUIZ MODE HANDLING
            // ============================================
            if (_isQuizMode)
            {
                string quizResponse = ProcessQuizAnswer(input);
                SpeakText(quizResponse);
                return quizResponse;
            }

            // ============================================
            // 2. QUIZ START COMMANDS
            // ============================================
            if (lowerInput.Contains("start quiz") ||
                lowerInput.Contains("take quiz") ||
                lowerInput.Contains("quiz me") ||
                lowerInput.Contains("test my knowledge"))
            {
                StartQuiz();
                string startMessage = "🎯 Starting the Cybersecurity Quiz!";
                SpeakText(startMessage);
                return startMessage + "\n\n" + DisplayQuestion(_quizManager.GetCurrentQuestion());
            }

            // ============================================
            // 3. ACTIVITY LOG COMMANDS
            // ============================================
            if (lowerInput.Contains("show activity log") ||
                lowerInput.Contains("what have you done") ||
                lowerInput.Contains("what did you do") ||
                lowerInput.Contains("show log") ||
                lowerInput.Contains("recent actions"))
            {
                string log = _logger.GetRecentLog(10);
                SpeakText("Here's your activity log.");
                return log;
            }

            // ============================================
            // 4. SHOW MORE LOG
            // ============================================
            if (lowerInput.Contains("show more") || lowerInput.Contains("see more"))
            {
                string fullLog = _logger.GetFullLog();
                SpeakText("Here's the complete log.");
                return fullLog;
            }

            // ============================================
            // 5. TASK INTENT
            // ============================================
            if (lowerInput.Contains("add task") ||
                lowerInput.Contains("create task") ||
                lowerInput.Contains("new task") ||
                lowerInput.Contains("enable") ||
                lowerInput.Contains("set up"))
            {
                string taskResponse = HandleTaskIntent(input);
                SpeakText(taskResponse);
                return taskResponse;
            }

            // ============================================
            // 6. NAME COLLECTION
            // ============================================
            if (!nameAsked)
            {
                if (lowerInput.Contains("my name is"))
                {
                    int startIndex = lowerInput.IndexOf("my name is") + 10;
                    if (startIndex < input.Length)
                        userName = input.Substring(startIndex).Trim().Trim('.', '!', '?');
                }
                else if (lowerInput.Contains("i'm ") || lowerInput.Contains("i am "))
                {
                    if (lowerInput.Contains("i'm "))
                    {
                        int startIndex = lowerInput.IndexOf("i'm ") + 4;
                        if (startIndex < input.Length)
                            userName = input.Substring(startIndex).Trim().Trim('.', '!', '?');
                    }
                    else
                    {
                        int startIndex = lowerInput.IndexOf("i am ") + 5;
                        if (startIndex < input.Length)
                            userName = input.Substring(startIndex).Trim().Trim('.', '!', '?');
                    }
                }
                else
                {
                    userName = input.Trim().Trim('.', '!', '?');
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    nameAsked = true;
                    _logger.Log($"User name set to: {userName}");
                    string greeting = $"Nice to meet you, {userName}! I help with phishing, passwords, malware, scams, VPNs, and privacy. What do you want to know?";
                    SpeakText(greeting);
                    return greeting;
                }
            }

            // ============================================
            // 7. KEYWORD MATCHING
            // ============================================
            foreach (var kvp in cybersecurityAnswers)
            {
                if (lowerInput.Contains(kvp.Key))
                {
                    _logger.Log($"Keyword matched: {kvp.Key}");
                    SpeakText(kvp.Value);
                    return kvp.Value;
                }
            }

            // ============================================
            // 8. GREETINGS
            // ============================================
            if (lowerInput.Contains("hello") || lowerInput.Contains("hi ") || lowerInput.Contains("hey "))
            {
                string greet = userName != null ? $"Hello {userName}! What cybersecurity topic do you want to learn?" : "Hello! What is your name?";
                SpeakText(greet);
                return greet;
            }

            // ============================================
            // 9. THANK YOU
            // ============================================
            if (lowerInput.Contains("thank") || lowerInput.Contains("thanks"))
            {
                string response = userName != null ? $"You're welcome, {userName}! Stay safe online!" : "You're welcome! Stay safe online!";
                SpeakText(response);
                return response;
            }

            // ============================================
            // 10. GOODBYE
            // ============================================
            if (lowerInput.Contains("bye") || lowerInput.Contains("goodbye") || lowerInput.Contains("exit"))
            {
                string response = userName != null ? $"Goodbye {userName}! Practice good cybersecurity. Stay safe!" : "Goodbye! Practice good cybersecurity. Stay safe!";
                SpeakText(response);
                return response;
            }

            // ============================================
            // 11. SENTIMENT DETECTION
            // ============================================
            string sentiment = DetectSentiment(input);
            string sentimentalResponse = GetSentimentalResponse(sentiment);
            if (sentimentalResponse != null)
            {
                _logger.Log($"Sentiment detected: {sentiment}");
                SpeakText(sentimentalResponse);
                return sentimentalResponse;
            }

            // ============================================
            // 12. FALLBACK
            // ============================================
            string randomResponse = fallbackResponses[random.Next(fallbackResponses.Length)];
            SpeakText(randomResponse);
            return randomResponse;
        }

        // ============================================
        // UI EVENT HANDLERS
        // ============================================

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
            }
        }

        private void ProcessUserInput(string input = null)
        {
            string userInput = input ?? UserInput.Text.Trim();

            if (string.IsNullOrEmpty(userInput))
                return;

            AppendMessage("You", userInput);
            UserInput.Clear();

            string botResponse = GetCybersecurityResponse(userInput);
            AppendMessage("Bot", botResponse);
            SpeakText(botResponse);
        }

        // ============================================
        // TASK BUTTON EVENT HANDLERS
        // ============================================

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleBox.Text.Trim();
            string description = TaskDescriptionBox.Text.Trim();
            string reminder = TaskReminderBox.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                AppendMessage("Bot", "⚠️ Please enter a task title.");
                return;
            }

            string result = _taskManager.AddTask(title, description, reminder);
            AppendMessage("Bot", result);
            LoadTasks();

            TaskTitleBox.Clear();
            TaskDescriptionBox.Clear();
            TaskReminderBox.Clear();
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is CyberTask selectedTask)
            {
                string result = _taskManager.MarkAsComplete(selectedTask.Id);
                AppendMessage("Bot", result);
                LoadTasks();
            }
            else
            {
                AppendMessage("Bot", "⚠️ Please select a task to mark as complete.");
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is CyberTask selectedTask)
            {
                var result = MessageBox.Show($"Are you sure you want to delete task '{selectedTask.Title}'?",
                                            "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    string response = _taskManager.DeleteTask(selectedTask.Id);
                    AppendMessage("Bot", response);
                    LoadTasks();
                }
            }
            else
            {
                AppendMessage("Bot", "⚠️ Please select a task to delete.");
            }
        }

        // ============================================
        // QUICK ACTION BUTTON EVENT HANDLERS
        // ============================================

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput("start quiz");
        }

        private void ShowLogButton_Click(object sender, RoutedEventArgs e)
        {
            string log = _logger.GetRecentLog(10);
            AppendMessage("Bot", log);
            SpeakText("Here's your activity log.");
        }

        private void RefreshTasksButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
            AppendMessage("Bot", "🔄 Task list refreshed from JSON file.");
        }

        private void ClearChatButton_Click(object sender, RoutedEventArgs e)
        {
            conversationMemory.Clear();
            AppendMessage("Bot", "🧹 Chat cleared. How can I help you?");
            SpeakText("Chat cleared.");
        }

        // ============================================
        // WINDOW CLOSE HANDLER
        // ============================================

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            try
            {
                synthesizer?.Dispose();
            }
            catch { /* Ignore dispose errors */ }
        }
    }

    // ============================================
    // XAML CONVERTERS (MUST BE OUTSIDE MAINWINDOW CLASS)
    // ============================================

    public class BoolToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "✅" : "⏳";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == "✅";
        }
    }

    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value?.ToString()) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}