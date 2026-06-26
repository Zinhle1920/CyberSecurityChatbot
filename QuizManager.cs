using CyberSecurityChatbot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot
{
    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;
        private bool _quizCompleted = false;

        public QuizManager()
        {
            _questions = new List<QuizQuestion>();
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            // 1. Phishing
            _questions.Add(new QuizQuestion
            {
                Question = "What should you do if you receive an email asking for your password?",
                Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                CorrectAnswer = "C",
                Explanation = "Reporting phishing emails helps prevent scams and protects others.",
                IsTrueFalse = false
            });

            // 2. Password Safety
            _questions.Add(new QuizQuestion
            {
                Question = "Is it safe to use the same password for multiple accounts?",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "Using the same password across multiple accounts is dangerous. If one is compromised, all are at risk.",
                IsTrueFalse = true
            });

            // 3. HTTPS
            _questions.Add(new QuizQuestion
            {
                Question = "What does HTTPS stand for?",
                Options = new List<string> { "A) HyperText Transfer Protocol Secure", "B) High-Tech Transfer Protocol System", "C) Hyper Transfer Text Protocol System", "D) High-Speed Text Transfer Protocol" },
                CorrectAnswer = "A",
                Explanation = "HTTPS encrypts data between your browser and the website, keeping your information secure.",
                IsTrueFalse = false
            });

            // 4. Public Wi-Fi
            _questions.Add(new QuizQuestion
            {
                Question = "Is it safe to connect to public Wi-Fi without a VPN?",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "Public Wi-Fi networks are often unsecured. Always use a VPN to protect your data.",
                IsTrueFalse = true
            });

            // 5. Social Engineering
            _questions.Add(new QuizQuestion
            {
                Question = "What is social engineering in cybersecurity?",
                Options = new List<string> { "A) Building secure social media platforms", "B) Manipulating people into revealing confidential information", "C) Engineering social networks", "D) Creating secure communication channels" },
                CorrectAnswer = "B",
                Explanation = "Social engineering exploits human psychology to gain unauthorized access to information.",
                IsTrueFalse = false
            });

            // 6. Two-Factor Authentication
            _questions.Add(new QuizQuestion
            {
                Question = "What is two-factor authentication (2FA)?",
                Options = new List<string> { "A) Using two different passwords", "B) A security method requiring two forms of verification", "C) Having two separate accounts", "D) Using a secondary email" },
                CorrectAnswer = "B",
                Explanation = "2FA adds an extra layer of security by requiring a second verification method.",
                IsTrueFalse = false
            });

            // 7. Malware
            _questions.Add(new QuizQuestion
            {
                Question = "What is malware?",
                Options = new List<string> { "A) Software that is malicious", "B) A type of hardware", "C) A programming language", "D) A security protocol" },
                CorrectAnswer = "A",
                Explanation = "Malware is malicious software designed to damage or gain unauthorized access to systems.",
                IsTrueFalse = false
            });

            // 8. Ransomware
            _questions.Add(new QuizQuestion
            {
                Question = "Ransomware is a type of malware that:",
                Options = new List<string> { "A) Steals passwords", "B) Encrypts files and demands payment", "C) Deletes system files", "D) Monitors user activity" },
                CorrectAnswer = "B",
                Explanation = "Ransomware encrypts your files and demands payment for the decryption key.",
                IsTrueFalse = false
            });

            // 9. Privacy Settings
            _questions.Add(new QuizQuestion
            {
                Question = "Why are privacy settings important on social media?",
                Options = new List<string> { "A) They control who can see your information", "B) They make your profile look better", "C) They are required by law", "D) They increase follower count" },
                CorrectAnswer = "A",
                Explanation = "Privacy settings let you control who can view your personal information, protecting you from cyber threats.",
                IsTrueFalse = false
            });

            // 10. Data Backup
            _questions.Add(new QuizQuestion
            {
                Question = "Should you regularly back up your data?",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "True",
                Explanation = "Regular backups protect your data from loss due to hardware failure, malware, or accidental deletion.",
                IsTrueFalse = true
            });

            // 11. Password Strength
            _questions.Add(new QuizQuestion
            {
                Question = "Which of these is the strongest password?",
                Options = new List<string> { "A) password123", "B) 12345678", "C) P@ssw0rd!2026", "D) qwerty" },
                CorrectAnswer = "C",
                Explanation = "A strong password uses a mix of uppercase, lowercase, numbers, and special characters.",
                IsTrueFalse = false
            });

            // 12. Phishing Prevention
            _questions.Add(new QuizQuestion
            {
                Question = "What is the best way to protect against phishing attacks?",
                Options = new List<string> { "A) Always click links in emails", "B) Verify the sender before clicking links", "C) Use the same password everywhere", "D) Ignore all emails" },
                CorrectAnswer = "B",
                Explanation = "Always verify the sender's identity and avoid clicking suspicious links to prevent phishing attacks.",
                IsTrueFalse = false
            });
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (_currentIndex < _questions.Count && !_quizCompleted)
                return _questions[_currentIndex];
            return null;
        }

        public bool SubmitAnswer(string answer)
        {
            if (_quizCompleted || _currentIndex >= _questions.Count)
                return false;

            var currentQuestion = _questions[_currentIndex];
            bool isCorrect = string.Equals(answer.Trim(), currentQuestion.CorrectAnswer, StringComparison.OrdinalIgnoreCase);

            if (isCorrect)
                _score++;

            _currentIndex++;

            if (_currentIndex >= _questions.Count)
                _quizCompleted = true;

            return isCorrect;
        }

        public string GetFeedback(bool isCorrect)
        {
            if (_currentIndex == 0) return "";
            var lastQuestion = _questions[_currentIndex - 1];
            return isCorrect ? $" Correct! {lastQuestion.Explanation}" : $" Incorrect. {lastQuestion.Explanation}";
        }

        public bool IsFinished()
        {
            return _quizCompleted || _currentIndex >= _questions.Count;
        }

        public string GetFinalScore()
        {
            return $"{_score} out of {_questions.Count}";
        }

        public string GetFinalMessage()
        {
            double percentage = (double)_score / _questions.Count;
            if (percentage >= 0.8)
                return " Excellent! You're a cybersecurity expert! ";
            else if (percentage >= 0.6)
                return " Good job! Keep learning to improve your score. ";
            else
                return "Keep studying cybersecurity topics and try again! You can do it! ";
        }

        public void ResetQuiz()
        {
            _currentIndex = 0;
            _score = 0;
            _quizCompleted = false;
        }

        public int GetTotalQuestions()
        {
            return _questions.Count;
        }

        public int GetCurrentIndex()
        {
            return _currentIndex;
        }

        public int GetScore()
        {
            return _score;
        }
    }
}