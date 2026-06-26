 AI Cybersecurity Chatbot (WPF)

An intelligent desktop chatbot developed in **C# using WPF (.NET)** that educates users about cybersecurity while providing task management, reminders, quiz functionality, activity logging, and machine learning-based response prediction.

 Project Overview

This project was developed as part of a cybersecurity chatbot assignment. The chatbot provides users with cybersecurity awareness, allows users to manage personal tasks, remembers user interests, and includes an interactive quiz to improve cybersecurity knowledge.

The application combines:

- Rule-based responses
- Machine Learning (ML.NET)
- SQL Server LocalDB
- WPF Desktop Interface
 Features
 User Management
- Username registration
- Returning user recognition
- Personalized welcome messages
- Stores usernames locally

AI Chatbot
- Answers cybersecurity questions
- Keyword-based response matching
- ML.NET prediction model
- Learns from unknown questions
- Remembers user interests
- Automatically reminds users about saved interests

Example questions:

- What is phishing?
- What is cybersecurity?
- What is a firewall?
- How do I create a strong password?
- What is a VPN?

Task Management

Users can:

- Add tasks
- Add task descriptions
- Set reminders
- View pending tasks
- Mark tasks as completed
- Delete completed tasks

Task data is stored in:

- SQL Server LocalDB

---

### 📅 Reminder System

Supports:

- No reminder
- Reminder today
- Reminder after X days
 Cybersecurity Quiz

Interactive multiple-choice quiz including questions about:

- Phishing
- Password Security
- Safe Browsing
- VPNs
- Public Wi-Fi
- Scam Detection

Features:

- Randomized answers
- Score tracking
- Immediate feedback
- Quiz reset after completion

Activity Log

Tracks important events including:

- Added tasks
- Completed tasks
- Deleted tasks
- Quiz completion

 Machine Learning

The chatbot uses **ML.NET** to improve responses by:

- Training on existing chatbot data
- Predicting appropriate responses
- Saving trained model
- Learning from unknown questions

Model file:

nlp_model.zip


---

## 🛠 Technologies Used

- C#
- WPF (.NET)
- XAML
- SQL Server LocalDB
- ML.NET
- Visual Studio 2022

 Project Structure


Chatbot3

 MainWindow.xaml
 MainWindow.xaml.cs
 respond.cs
 training.cs
 tasks.cs
 user_name.cs
 Question_in_quiz.cs
Quiz_Question_Load.cs
 voice_greeting.cs

 Logo3.jpg
 interested_topic.txt
 user_names.txt
 unknown_questions.txt
 learned_questions.txt
nlp_model.zip




 Database

Database:


pro_tasks


Table:


demo_tasks
```

Columns:

 Column  Type 

 task_id  INT
 task_name  NVARCHAR 
 task_description  NVARCHAR 
task_dueDate| NVARCHAR 
 task_status  NVARCHAR 

The application automatically creates the table if it does not already exist.



 Getting Started

 Requirements

- Visual Studio 2022
- .NET Desktop Development
- SQL Server LocalDB
- .NET Framework / .NET supported by the project
- ML.NET NuGet Packages

---

Installation

1. Clone the repository

bash
git clone https://github.com/yourusername/AI-Cybersecurity-Chatbot.git


2. Open the solution in Visual Studio.

3. Restore NuGet packages.

4. Build the project.

5. Run the application.

Application Screens

- Home Screen
- Username Screen
- Chat Page
- Task Manager
- Activity Log
- Cybersecurity Quiz





 Machine Learning Workflow

1. Load chatbot responses
2. Clean text
3. Train ML.NET model
4. Save model
5. Predict responses
6. Learn from unknown questions



Cybersecurity Topics Covered

- Phishing
- Password Safety
- Firewalls
- VPNs
- Public Wi-Fi
- Fraud Prevention
- Safe Browsing
- Hacked Accounts
- Malicious Chatbots



Future Improvements

- Voice recognition
- Speech-to-text
- Online database support
- AI API integration
- Better Natural Language Processing
- User authentication
- Dark mode
- Export activity logs
- Cloud synchronization



 Author

**Nkanyezi Mokoena**

---

 License

This project was created for educational purposes.

MIT License

Copyright (c) 2026 Nkanyezi Mokoena

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files to deal in the Software
without restriction, including without limitation the rights to use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies of the
Software.

 Acknowledgements

- Microsoft WPF
- ML.NET
- SQL Server LocalDB
- Visual Studio
- Cybersecurity learning resources


