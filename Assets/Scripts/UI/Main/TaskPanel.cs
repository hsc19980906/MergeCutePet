using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskPanel : UIBase
{
    private List<Task> tasks;
    public GameObject Main;
    public GameObject Daily;
    public GameObject Collect;
    public GameObject ExChange;
    private TaskUI[] MainTasks;
    private TaskUI[] DailyTasks;
    private TaskUI[] CollectTasks;
    private TaskUI[] ExChangeTasks;
    private string path;
    private List<int> finishedTasks;

    private void Awake()
    {
        path = Application.persistentDataPath;
        Bind(UIEvent.TASK_PANEL_ACTIVE,UIEvent.REMOVE_TASK,UIEvent.MAIN_TASK_FINISH_TIME);
        tasks = new List<Task>();
        finishedTasks = new List<int>();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.TASK_PANEL_ACTIVE:
                setPanelActive((bool)message);
                UpdateUI();
                break;
            case UIEvent.REMOVE_TASK:
                tasks.Remove(message as Task);
                finishedTasks.Add((message as Task).id);
                UpdateUI();
                break;
            case UIEvent.MAIN_TASK_FINISH_TIME:
                Task task = message as Task;
                if (finishedTasks.Count >= 7)
                    task.Finished = true;
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        MainTasks = Main.GetComponentsInChildren<TaskUI>();
        DailyTasks = Daily.GetComponentsInChildren<TaskUI>();
        CollectTasks = Collect.GetComponentsInChildren<TaskUI>();
        ExChangeTasks = ExChange.GetComponentsInChildren<TaskUI>();

        ParseFinishedTaskJson();
        ParseTaskJson();
        setPanelActive(false);
    }

    private void ParseTaskJson()
    {
        TextAsset taskText = Resources.Load<TextAsset>("Task");
        string tasksJson = taskText.text;
        tasks = JsonConvert.DeserializeObject<List<Task>>(tasksJson);
        foreach (int id in finishedTasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].id == id)
                    tasks.Remove(tasks[i]);
            }
        }
        UpdateUI();
    }

    private void ParseFinishedTaskJson()
    {
        if (!File.Exists(path + "/Task.json"))
        {
            File.Create(path + "/Task.json").Dispose();
        }
        if (File.Exists(path + "/Task.json"))
        {
            string json = File.ReadAllText(path + "/Task.json");
            if (json != "")
            {
                finishedTasks = JsonConvert.DeserializeObject<List<int>>(json);
            }
        }
    }

    private void SaveFinishedTask()
    {
        if (!File.Exists(path + "/Task.json"))
        {
            File.Create(path + "/Task.json").Dispose();
        }
        string json = JsonConvert.SerializeObject(finishedTasks);

        if (File.Exists(path + "/Task.json"))
        {
            File.WriteAllText(path + "/Task.json", json);
        }
    }

    private void UpdateUI()
    {
        ClearUpTasks();
        if(tasks.Count>0)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                switch (tasks[i].taskType)
                {
                    case Task.TaskType.Main:
                        for (int j = 0; j < MainTasks.Length; j++)
                        {
                            if(!MainTasks[j].IsExistTask())
                            {
                                MainTasks[j].SetTask(tasks[i]);
                                break;
                            }
                        }
                        break;
                    case Task.TaskType.Daily:
                        for (int j = 0; j < DailyTasks.Length; j++)
                        {
                            if (!DailyTasks[j].IsExistTask())
                            {
                                DailyTasks[j].SetTask(tasks[i]);
                                break;
                            }
                        }
                        break;
                    case Task.TaskType.Collect:
                        for (int j = 0; j < CollectTasks.Length; j++)
                        {
                            if (!CollectTasks[j].IsExistTask())
                            {
                                CollectTasks[j].SetTask(tasks[i]);
                                break;
                            }
                        }
                        break;
                    case Task.TaskType.ExChange:
                        for (int j = 0; j < ExChangeTasks.Length; j++)
                        {
                            if (!ExChangeTasks[j].IsExistTask())
                            {
                                ExChangeTasks[j].SetTask(tasks[i]);
                                break;
                            }
                        }
                        break;
                    default:
                        print("没有设置任务类型");
                        break;
                }
            }
        }
    }

    private void ClearUpTasks()
    {
        foreach (TaskUI item in MainTasks)
        {
            item.ClearUp();
        }
        foreach (TaskUI item in DailyTasks)
        {
            item.ClearUp();
        }
        foreach (TaskUI item in ExChangeTasks)
        {
            item.ClearUp();
        }
        foreach (TaskUI item in CollectTasks)
        {
            item.ClearUp();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        SaveFinishedTask();
    }
}
