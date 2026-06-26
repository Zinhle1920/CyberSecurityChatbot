using CyberSecurityChatbot;
using CyberSecurityChatBot;
using System;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;

namespace CyberSecurityChatbot
{
    public class ChatBot
    {
        private KeywordResponder _keywordResponder;
        private SentimentDetector _sentimentDetector;
        private MemoryStore _memoryStore;
        private TaskManager _taskManager;
        private QuizManager _quizManager;
        private ActivityLogger _activityLogger;
        private SpeechSynthesizer _speechSynthesizer;
        private string _userName;

        public ChatBot(ActivityLogger logger)
        {
            _keywordResponder = new KeywordResponder();
            _sentimentDetector = new SentimentDetector();
            _memoryStore = new MemoryStore();
            _activityLogger = logger;
            _taskManager = new TaskManager(logger);
            _quizManager = new QuizManager();
            _speechSynthesizer = new SpeechSynthesizer();

            // Configure speech synthesizer
            _speechSynthesizer.SetOutputToDefaultAudioDevice();
            _speechSynthesizer.Rate = 0; // Normal speed
            _speechSynthesizer.Volume = 100; // Full volume
        }

        public void Speak(string text)
        {
            try
            {
                // Speak asynchronously
                _speechSynthesizer.SpeakAsync(text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Speech error: {ex.Message}");
            }
        }

        public void SpeakGreeting()
        {
            try
            {
                string greeting = "Welcome to the Cybersecurity Chatbot! I'm here to help you stay safe online.";
                _speechSynthesizer.SpeakAsync(greeting);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Speech error: {ex.Message}");
            }
        }

        public string ProcessInput(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please say something. ";

            string input = userInput.Trim();
            string lowerInput = input.ToLower();

            // Check for explicit 'show more' for activity log
            if (lowerInput.Contains("show more") || lowerInput.Contains("see more"))
            {
                var fullLog = _activityLogger.GetFullLog();
                _memoryStore.AddToHistory(userInput, fullLog);
                return fullLog;
            }

            // Step 1: Detect Intent
            string intent = _keywordResponder.DetectIntent(input);

            // Step 2: Handle intent
            switch (intent)
            {
                case "addtask":
                    return HandleAddTask(input);

                case "setreminder":
                    return HandleSetReminder(input);

                case "startquiz":
                    _activityLogger.Log("Quiz started");
                    return StartQuiz();

                case "showlog":
                    return HandleShowLog();

                case "unknown":
                    // Check for quiz answer
                    if (_quizManager.GetCurrentQuestion() != null)
                    {
                        return ProcessQuizAnswer(input);
                    }
                    break;
            }

            // Step 3: Check for keyword matches
            string keywordResponse = _keywordResponder.GetResponseForKeyword(input);
            if (keywordResponse != null)
            {
                _activityLogger.Log($"Keyword matched: {ExtractKeyword(input)} - response delivered");
                _memoryStore.RememberTopic(ExtractKeyword(input));
                _memoryStore.AddToHistory(userInput, keywordResponse);
                return keywordResponse;
            }

            // Step 4: Sentiment Detection
            string sentiment = _sentimentDetector.DetectSentiment(input);
            if (sentiment == "negative" || sentiment == "positive")
            {
                string sentimentResponse = _sentimentDetector.GetSentimentResponse(sentiment);
                if (sentimentResponse != null)
                {
                    _activityLogger.Log($"Sentiment detected: {sentiment} - auto-gave tip");
                    _memoryStore.AddToHistory(userInput, sentimentResponse);
                    return sentimentResponse;
                }
            }

            // Step 5: Memory/Follow-up
            string followUpResponse = _memoryStore.GetFollowUpResponse(input);
            if (followUpResponse != null)
            {
                _memoryStore.AddToHistory(userInput, followUpResponse);
                return followUpResponse;
            }

            // Step 6: Check for greeting
            if (IsGreeting(input))
            {
                string greeting = GetGreeting();
                _memoryStore.AddToHistory(userInput, greeting);
                return greeting;
            }

            // Step 7: Check for goodbye
            if (IsGoodbye(input))
            {
                string goodbye = "Goodbye! Stay safe online and remember to practice good cybersecurity habits!";
                _memoryStore.AddToHistory(userInput, goodbye);
                return goodbye;
            }

            // Step 8: Fallback
            string fallbackResponse = " I didn't quite understand that. I can help with:\n" +
                                    "• Cybersecurity topics (phishing, passwords, privacy, etc.)\n" +
                                    "• Task management (add, complete, delete tasks)\n" +
                                    "• Quiz (type 'start quiz')\n" +
                                    "• Activity log (type 'show activity log')\n" +
                                    "What would you like to know?";

            _memoryStore.AddToHistory(userInput, fallbackResponse);
            return fallbackResponse;
        }

        private string HandleAddTask(string input)
        {
            // Try to extract task title
            string taskTitle = ExtractTaskTitle(input);

            if (string.IsNullOrEmpty(taskTitle))
            {
                return "I need a task description. Try something like: 'Add task - Review privacy settings'";
            }

            // Check if task already exists
            if (_taskManager.TaskExists(taskTitle))
            {
                return $" Task '{taskTitle}' already exists. Try a different task.";
            }

            string result = _taskManager.AddTask(taskTitle, taskTitle, "");
            _memoryStore.AddToHistory($"Add task: {taskTitle}", result);
            return result + " Would you like to set a reminder?";
        }

        private string HandleSetReminder(string input)
        {
            // Extract reminder details
            string reminderText = ExtractReminderText(input);
            if (string.IsNullOrEmpty(reminderText))
            {
                return "I need a reminder. Try: 'Remind me to update my password tomorrow'";
            }

            _activityLogger.Log($"Reminder set: '{reminderText}'");
            string response = $" Reminder set for '{reminderText}'.";
            _memoryStore.AddToHistory(input, response);
            return response;
        }

        private string HandleShowLog()
        {
            string log = _activityLogger.GetRecentLog(10);
            _activityLogger.Log("Activity log viewed");
            _memoryStore.AddToHistory("show activity log", log);
            return log;
        }

        private string StartQuiz()
        {
            _quizManager.ResetQuiz();
            var question = _quizManager.GetCurrentQuestion();
            return DisplayQuestion(question);
        }

        private string ProcessQuizAnswer(string input)
        {
            var currentQuestion = _quizManager.GetCurrentQuestion();
            if (currentQuestion == null)
                return "The quiz hasn't started. Type 'start quiz' to begin!";

            bool isCorrect = _quizManager.SubmitAnswer(input);
            string feedback = _quizManager.GetFeedback(isCorrect);

            if (_quizManager.IsFinished())
            {
                _activityLogger.Log($"Quiz completed - score: {_quizManager.GetFinalScore()}");
                string score = _quizManager.GetFinalScore();
                string message = _quizManager.GetFinalMessage();
                return $"{feedback}\n\n Final Score: {score}\n{message}";
            }

            var nextQuestion = _quizManager.GetCurrentQuestion();
            return $"{feedback}\n\n{DisplayQuestion(nextQuestion)}";
        }

        private string DisplayQuestion(QuizQuestion question)
        {
            if (question == null)
                return "No more questions!";

            string display = $" Question {_quizManager.GetCurrentIndex() + 1}/{_quizManager.GetTotalQuestions()}\n";
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

        private string ExtractKeyword(string input)
        {
            foreach (var keyword in new[] { "phishing", "password", "privacy", "scam", "malware", "2fa", "vpn" })
            {
                if (input.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    return keyword;
            }
            return "unknown";
        }

        private string ExtractTaskTitle(string input)
        {
            // Try common patterns: "Add task - X", "Add task X", "enable X", etc.
            var patterns = new[]
            {
                @"add\s+task\s*[-:]\s*(.+)",
                @"add\s+a\s+task\s*[-:]\s*(.+)",
                @"create\s+task\s*[-:]\s*(.+)",
                @"add\s+task\s+(.+)",
                @"enable\s+(.+)",
                @"set\s+up\s+(.+)",
                @"remind\s+me\s+to\s+(.+)",
                @"I\s+need\s+to\s+(.+)",
                @"I\s+should\s+(.+)"
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string title = match.Groups[1].Value.Trim();
                    // Remove common trailing words
                    title = Regex.Replace(title, @"\s+tomorrow$", "", RegexOptions.IgnoreCase);
                    title = Regex.Replace(title, @"\s+in\s+\d+\s+days$", "", RegexOptions.IgnoreCase);
                    return title;
                }
            }

            // Fallback: take first 50 characters
            return input.Length > 50 ? input.Substring(0, 50) : input;
        }

        private string ExtractReminderText(string input)
        {
            var patterns = new[]
            {
                @"remind\s+me\s+to\s+(.+?)(?:\s+tomorrow|\s+in\s+\d+\s+days|$)",
                @"reminder\s+for\s+(.+?)(?:\s+tomorrow|\s+in\s+\d+\s+days|$)",
                @"set\s+a\s+reminder\s+for\s+(.+?)(?:\s+tomorrow|\s+in\s+\d+\s+days|$)",
                @"don'?t\s+forget\s+to\s+(.+?)(?:\s+tomorrow|\s+in\s+\d+\s+days|$)"
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Groups[1].Value.Trim();
                }
            }

            return null;
        }

        private bool IsGreeting(string input)
        {
            string lower = input.ToLower();
            var greetings = new[] { "hello", "hi", "hey", "good morning", "good afternoon", "good evening", "howdy" };
            foreach (var greeting in greetings)
            {
                if (lower.Contains(greeting) || lower.StartsWith(greeting))
                    return true;
            }
            return false;
        }

        private bool IsGoodbye(string input)
        {
            string lower = input.ToLower();
            var goodbyes = new[] { "bye", "goodbye", "see you", "farewell", "later", "exit", "quit" };
            foreach (var goodbye in goodbyes)
            {
                if (lower.Contains(goodbye) || lower.StartsWith(goodbye))
                    return true;
            }
            return false;
        }

        private string GetGreeting()
        {
            var greetings = new[]
            {
                "Hello!  I'm your Cybersecurity Chatbot. How can I help you stay safe online today?",
                "Hi there!  Ready to learn about cybersecurity? I can help with phishing, passwords, and more!",
                "Welcome!  I'm here to help with all your cybersecurity questions. What would you like to know?"
            };
            return greetings[new Random().Next(greetings.Length)];
        }

        public string GetUserName()
        {
            return _userName;
        }

        public void SetUserName(string name)
        {
            _userName = name;
            _activityLogger.Log($"User named '{name}' set");
        }

        public string GreetUserByName()
        {
            if (string.IsNullOrEmpty(_userName))
                return "What's your name? ";

            var greetings = new[]
            {
                $"Nice to see you again, {_userName}!  What would you like to learn about today?",
                $"Welcome back, {_userName}!  I've got some new cybersecurity tips to share!",
                $"Hello, {_userName}!  Ready to boost your cybersecurity knowledge?"
            };
            return greetings[new Random().Next(greetings.Length)];
        }

        public void Dispose()
        {
            if (_speechSynthesizer != null)
            {
                _speechSynthesizer.Dispose();
                _speechSynthesizer = null;
            }
        }

        // Add this to handle when speech should stop
        public void StopSpeaking()
        {
            if (_speechSynthesizer != null)
            {
                _speechSynthesizer.SpeakAsyncCancelAll();
            }
        }
    }
}