using System;
using System.Collections.Generic;

namespace CyberSecurityChatBot
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;
        private ActivityLogger _logger;

        public TaskManager(ActivityLogger logger)
        {
            _storage = new TaskStorageHelper();
            _logger = logger;
        }

        public string AddTask(string title, string description, string reminder = "")
        {
            try
            {
                var task = _storage.AddTask(title, description, reminder);
                _logger.Log($"Task added: '{title}'");
                return $"✅ Task '{title}' added successfully!";
            }
            catch
            {
                return "❌ Error adding task.";
            }
        }

        public List<CyberTask> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        public string MarkAsComplete(int id)
        {
            try
            {
                var tasks = _storage.LoadTasks();
                var task = tasks.Find(t => t.Id == id);
                if (task != null)
                {
                    _storage.MarkAsComplete(id);
                    _logger.Log($"Task marked complete: '{task.Title}'");
                    return $"✅ Task '{task.Title}' marked as complete!";
                }
                return "❌ Task not found.";
            }
            catch
            {
                return "❌ Error marking task.";
            }
        }

        public string DeleteTask(int id)
        {
            try
            {
                var tasks = _storage.LoadTasks();
                var task = tasks.Find(t => t.Id == id);
                if (task != null)
                {
                    _storage.DeleteTask(id);
                    _logger.Log($"Task deleted: '{task.Title}'");
                    return $"✅ Task '{task.Title}' deleted!";
                }
                return "❌ Task not found.";
            }
            catch
            {
                return "❌ Error deleting task.";
            }
        }

        public bool TaskExists(string title)
        {
            var tasks = _storage.LoadTasks();
            return tasks.Exists(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }
    }
}