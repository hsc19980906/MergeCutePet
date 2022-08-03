using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : UIBase
{
    private Task task;
    private Image imgType;
    private Text txtName;
    private Button btnExpand;//点击展开具体任务描述
    private Button btnGet;//领取奖励
    private Sprite sprite;

    private Image ImgType
    {
        get
        {
            if (imgType == null)
                imgType = GetComponentInChildren<Image>();
            sprite = imgType.sprite;
            return imgType;
        }
    }

    private Text TxtName
    {
        get
        {
            if (txtName == null)
            {
                txtName = GetComponentInChildren<Text>();
            }
            return txtName;
        }
    }

    private Button BtnExpand
    {
        get
        {
            if (btnExpand == null)
            {
                btnExpand = transform.Find("txtName").GetComponent<Button>();
            }
            return btnExpand;
        }
    }

    private Button BtnGet
    {
        get
        {
            if (btnGet == null)
            {
                btnGet = transform.Find("btnGet").GetComponent<Button>();
            }
            return btnGet;
        }
    }

    private void Start()
    {
        BtnExpand.onClick.AddListener(ExpandInfo);
        BtnGet.onClick.AddListener(GetReward);
    }

    private void GetReward()
    {
        foreach (Task.RequireType requireType in task.Requires.Keys)
        {
            switch (requireType)
            {
                case Task.RequireType.NewBase:
                    PetCharacter.Instance.state.KillNum1 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Forest:
                    PetCharacter.Instance.state.KillNum2 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Cliff:
                    PetCharacter.Instance.state.KillNum3 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Lode:
                    PetCharacter.Instance.state.KillNum4 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Ridge:
                    PetCharacter.Instance.state.KillNum5 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Beach:
                    PetCharacter.Instance.state.KillNum6 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Volcano:
                    PetCharacter.Instance.state.KillNum7 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Desert:
                    PetCharacter.Instance.state.KillNum8 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Mirage:
                    PetCharacter.Instance.state.KillNum9 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Ice:
                    PetCharacter.Instance.state.KillNum10 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Sea:
                    PetCharacter.Instance.state.KillNum11 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Christmas:
                    PetCharacter.Instance.state.KillNum12 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Eddy:
                    PetCharacter.Instance.state.KillNum13 -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Item:
                    foreach (int id in task.Requires[requireType].Keys)
                    {
                        Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { ItemId = id, Amount = task.Requires[requireType][id] });
                    }
                    break;
                case Task.RequireType.Merge:
                    PetCharacter.Instance.state.MergeNum -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Coin:
                    PlayerCharacter.Instance.player.Coin -= task.Requires[requireType][-1];
                    break;
                case Task.RequireType.Diamond:
                    PlayerCharacter.Instance.player.Diamond -= task.Requires[requireType][-1];
                    break;
                default:
                    break;
            }
        }
        foreach (Task.RewardType rewardType in task.Rewards.Keys)
        {
            switch (rewardType)
            {
                case Task.RewardType.Item:
                    foreach (int id in task.Rewards[rewardType].Keys)
                    {
                        Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { ItemId =id,Amount= task.Rewards[rewardType][id]});
                    }
                    break;
                case Task.RewardType.Coin:
                    PlayerCharacter.Instance.player.Coin += task.Rewards[rewardType][-1];
                    Dispatch(AreaCode.UI, UIEvent.REFRESH_PLAYER_MONEY, PlayerCharacter.Instance.player);
                    break;
                case Task.RewardType.Diamond:
                    PlayerCharacter.Instance.player.Diamond += task.Rewards[rewardType][-1];
                    Dispatch(AreaCode.UI, UIEvent.REFRESH_PLAYER_MONEY, PlayerCharacter.Instance.player);
                    break;
                case Task.RewardType.Gold:
                    PlayerCharacter.Instance.player.Gold += task.Rewards[rewardType][-1];
                    Dispatch(AreaCode.UI, UIEvent.REFRESH_PLAYER_MONEY, PlayerCharacter.Instance.player);
                    break;
                case Task.RewardType.Exp:
                    PetCharacter.Instance.state.TotalExp += task.Rewards[rewardType][-1];
                    break;
                default:
                    break;
            }
        }
        //TODO 主线任务怎么依次完成
        if (task.taskType == Task.TaskType.Main)
        {
            //PetCharacter.Instance.state.MainFinishTime++;
            Dispatch(AreaCode.UI, UIEvent.REMOVE_TASK, task);
        }
        if (task != null)
        {
            Dispatch(AreaCode.UI, UIEvent.TASK_FINISH, task);
            UpdateUI();
        }
    }

    private void ExpandInfo()
    {
        //, new Vector3(-500, 400)
        if (task != null)
        {
            Dispatch(AreaCode.UI, UIEvent.ITEM_MSG, new ItemMsg()
            {
                itemMsg = String.Format("任务描述：\n{0}\n{1}{2}",
                task.Info, task.RequireInfo(), task.RewardInfo()),
                position = new Vector3(Screen.width / 2 - 500, Screen.height / 2 + 200)
            });
        }

    }

    public void SetTask(Task task)
    {
        this.task = task;
        Dispatch(AreaCode.UI, UIEvent.TASK_FINISH, task);
        UpdateUI();
    }

    public bool IsExistTask()
    {
        return task == null ? false : true;
    }

    private void UpdateUI()
    {
        if (task != null)
        {
            TxtName.text = task.Name;
            UpdateType();
        }
    }

    private void UpdateType()
    {
        if (task.Finished)
        {
            ImgType.sprite = Resources.Load<Sprite>("Task/1");
            BtnGet.interactable = true;
        }
        else
        {
            ImgType.sprite = Resources.Load<Sprite>("Task/2");
            BtnGet.interactable = false;
        }
    }

    public void ClearUp()
    {
        task = null;
        TxtName.text = "";
        ImgType.sprite = sprite;
        BtnGet.interactable = false;
    }
}
