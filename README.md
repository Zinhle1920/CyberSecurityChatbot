# Cybersecurity Chatbot (C# Console Application)

## 📌 Project Overview
This project is a C# console-based chatbot designed to educate users about basic cybersecurity concepts. It interacts with users through text input and provides educational responses about online safety.

The chatbot also includes text-to-speech functionality and ASCII art to enhance the user experience.

---

## 🎯 Features
- Interactive console chatbot
- User input and personalised responses
- Cybersecurity topic support:
  - Password security
  - Phishing
  - Malware
  - Ransomware
  - Two-factor authentication (2FA)
  - Safe browsing
- Text-to-speech using Windows SAPI (SpVoice)
- ASCII art welcome screen
- Input validation (handles empty inputs)
- Continuous chat loop until user exits

---

## 🔊 Text-to-Speech
The project uses the built-in Windows Speech API (SAPI SpVoice) for text-to-speech functionality.  
No external libraries or audio files are required.

---

## 🧠 How It Works
The chatbot uses simple keyword matching to detect user input and respond with relevant cybersecurity information.

Example:
- User: "What is phishing?"
- Bot: Explains phishing attacks and how to avoid them

---

## ▶️ How to Run the Project
1. Open the project in Visual Studio
2. Build the solution
3. Run using:
   - Press **F5** or click the green ▶ button

---

## 💾 GitHub & CI
- Project is hosted on GitHub
- Includes multiple commits showing development progress
- GitHub Actions CI is used to automatically build the project

---

## 🛠 Technologies Used
- C# (.NET 6 Console Application)
- Visual Studio
- Git & GitHub
- Windows SAPI (Speech API)

---
## youtube link to Video prensentation 
https://youtu.be/lx99ztLPTCs
------

## 👤 Author
Student Project – Cybersecurity Chatbot Assignment

## Part 2 POE Improvements
- Part 2 introduced major improvements and intellegent chatbot functionality, including:
## Keyword Recognition
- The chatbot now recognises cybersecurity-related keywords such as:
- phishing
- password
- malware
- privacy
- VPN
- scams
- The chatbot provides relevant educational cybersecurity responses based on user input

## Random Responses 
The chatbot now generates randomised responses for cybersecurity topics, improving user interaction and making the conversartion feel more natural

## Memory and Recall
The chatbot remembers:
-the user's name 
-previously discussed topics 
-conversation context
This allows the chatbot to personalise future responses.

## Sentiment Detection 
The chatbot can detect user emotions such as:
-worry
-frustration
-curiosity
The chatbot responds empathetically before providing cybersecurity advice.

## Technologies used 
-C#
-WPF application
-.NET
-Visual Studio
-GitHub
Text-to-speech

## Project structure
-MainWindow.xaml
-MainWindow.xaml.cs
-Chatbot.cs
-KeywordResponder.cs
-SentimentDetector.cs
-MemoryStore.cs
-App.xaml
-App.xaml.cs

## Video Presentation for part 2 - Youtube link
- https://youtu.be/WuhgxkXD09Y


# Part 3 for the CyberSecurityChatbot

## MainWindow.xaml

* Created the chatbot user interface.
* Added chat panel and task assistant.
* Added quick action buttons.
* Improved accessibility.
* Fixed XAML compatibility issues.

## MainWindow.xaml.cs

* Added chatbot functionality.
* Implemented task management.
* Added quiz functionality.
* Added activity log.
* Added speech responses.
* Added conversation memory.

## ChatBot.cs

* Processes user input.
* Detects user intentions.
* Responds to cybersecurity questions.
* Connects tasks, quiz and activity log.

## KeywordResponder.cs

* Detects cybersecurity keywords.
* Responds to phishing, passwords, malware and other topics.
* Detects task, quiz and activity log commands.

## SentimentDetector.cs

* Detects positive, negative and worried emotions.
* Provides helpful cybersecurity tips.

## MemoryStore.cs

* Stores conversation history.
* Remembers previous topics.
* Supports follow-up questions.

## CyberTask.cs

* Stores task information.
* Includes task title, description, reminder and completion status.

## TaskStorageHelper.cs

* Saves tasks to a JSON file.
* Loads tasks from JSON.
* Updates and deletes tasks.

## TaskManager.cs

* Manages task operations.
* Adds, completes and deletes tasks.
* Connects task storage with activity logging.

## QuizQuestion.cs

* Stores quiz questions and answers.
* Supports explanations for correct answers.

## QuizManager.cs

* Contains cybersecurity quiz questions.
* Tracks scores.
* Gives feedback after each question.
* Displays final results.

## ActivityLogger.cs

* Records chatbot activities.
* Stores timestamps.
* Displays recent and complete activity logs.

## App.xaml

* Starts the application.
* Loads application resources.

## App.xaml.cs

* Initializes the application.

## Additional Files

* **README.md** – Project information and setup guide.
* **.gitignore** – Ignores unnecessary Visual Studio files.
* **tasks.json** – Stores task data.
* **greeting.wav** – Startup greeting audio (optional).

## Project Configuration

* Added Newtonsoft.Json package.
* Added System.Speech reference.
* Configured project for .NET 8.

## YOUTUBE PRESENTATION LINK FOR PART 3:
https://www.youtube.com/watch?v=A8TyWm6P-ks


