using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CyberSecurityChatBot
{
    public class TaskStorageHelper
    {
        private const string FilePath = "tasks.json";

        public List<CyberTask> LoadTasks()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    return JsonConvert.DeserializeObject<List<CyberTask>>(json) ?? new List<CyberTask>();
                }
                return new List<CyberTask>();
            }
            catch
            {
                return new List<CyberTask>();
            }
        }

        public void SaveTasks(List<CyberTask> tasks)
        {
            try
            {
                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch { }
        }

        public CyberTask AddTask(string title, string description, string reminder)
        {
            var tasks = LoadTasks();
            int newId = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;

            var task = new CyberTask
            {
                Id = newId,
                Title = title,
                Description = description,
                Reminder = reminder ?? "",
                IsComplete = false
            };

            tasks.Add(task);
            SaveTasks(tasks);
            return task;
        }

        public bool MarkAsComplete(int id)
        {
            var tasks = LoadTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsComplete = true;
                SaveTasks(tasks);
                return true;
            }
            return false;
        }

        public bool DeleteTask(int id)
        {
            var tasks = LoadTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                tasks.Remove(task);
                SaveTasks(tasks);
                return true;
            }
            return false;
        }

        public bool FileExists()
        {
            return File.Exists(FilePath);
        }
    }
}
