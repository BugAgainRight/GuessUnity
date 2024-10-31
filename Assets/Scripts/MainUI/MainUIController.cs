using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CircleOfLife.Configuration;
using CircleOfLife.General;
using GuessUnity;
using Milease.Core.UI;
using Milease.Enums;
using Milease.Utils;
using Milutools.Milutools.UI;
using Milutools.SceneRouter;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CircleOfLife
{
    public class MainUIController : MonoBehaviour
    {
        public static MainUIController Instance;
        public static MessageReadList ReadList;
        
        public static GuessList UserGuessList;
        public static DateTime SimulatedTime;
        
        private EventList allEvents;

        public TMP_Dropdown MonthDropDown, DayDropDown, StageDropDown;
        private readonly List<int> monthReference = new();
        private readonly List<int> dayReference = new();

        public Sprite UseSortSprite, NoSortSprite;

        private bool sortDate, sortStage;

        public MilListView EventListView;
        public RectTransform DateSortRect, StageSortRect;
        public Image DateSortIcon, StageSortIcon;

        public Toggle MineOnly;
        
        public TMP_Text SimulatedTimeText;

        private bool redeemOpen = false;

        private MessageList msgList = new MessageList();

        public GameObject Unseen;
        
        private void Awake()
        {
            Instance = this;
            
            if (ReadList == null)
            {
                ReadList = new MessageReadList();
                if (PlayerPrefs.GetString("read", "") != "")
                {
                    ReadList = JsonConvert.DeserializeObject<MessageReadList>(PlayerPrefs.GetString("read", ""));
                }
            }
            
            FetchEventList();
            GetSimulatedTime();
            FetchMessages();
        }
        
        private async void FetchMessages()
        {
            msgList = await Server.Get<MessageList>("/api/user/messages", ("account", LoginManager.Account));
            Unseen.SetActive(msgList.Messages.Any(x => !ReadList.ReadMessages.Contains(x.ID)));
        }

        public void OpenMessageUI()
        {
            MessageUI.Open(msgList);
            Unseen.SetActive(false);
        }

        public void OpenUserInfo()
        {
            UIManager.Get(UIIdentifier.UserInfo).Open();
        }

        public void Refresh()
        {
            SceneRouter.GoTo(SceneIdentifier.MainPage);
        }
        
        public void ModifySimulatedTime()
        {
            TimeSelecterUI.Open(SimulatedTime,  (time) =>
            {
                if (time.Equals(SimulatedTime))
                {
                    return;
                }

                if (time < SimulatedTime)
                {
                    MessageBox.Open(("时间倒流", "您设定了一个比当前更早的模拟时间，这个操作不会还原已经结算的竞猜操作和积分流动（仅供测试功能使用），您确定一定要这么做吗？"), (o) =>
                    {
                        if (o == MessageBox.Operation.Deny)
                        {
                            return;
                        }

                        ChangeTime(time);
                    });
                    return;
                }
                
                ChangeTime(time);
            });
        }

        public async void OpenRedeemUI()
        {
            if (redeemOpen)
            {
                return;
            }

            redeemOpen = true;
            var list = await Server.Get<PrizeList>("/api/prizes/info", ("account", LoginManager.Account));
            RedeemUI.Open(list);
            redeemOpen = false;
        }

        private async void ChangeTime(DateTime time)
        {
            var state = await Server.Get<StatusData>("/api/time/set", ("time", time.ToString("o")));
            if (state.Success)
            {
                MessageBox.Open(("修改成功", "服务器模拟时间已修改。\n<color=red>若当前同时有其他用户在测试，其他用户需要点击“刷新页面”更新配置。"), (_) =>
                {
                    SceneRouter.GoTo(SceneIdentifier.MainPage);
                });
            }
            else
            {
                MessageBox.Open(("修改失败！", state.Message));
            }
        }

        public void SwitchDateSort()
        {
            sortDate = !sortDate;
            DateSortIcon.sprite = sortDate ? UseSortSprite : NoSortSprite;
            DateSortRect.MileaseTo(UMN.SizeDelta, new Vector2(1263.09f, sortDate ? 390.7f : 158.83f), 0.5f,
                                0f, EaseFunction.Back, EaseType.Out).Play();
            UpdateEventListView();
        }
        
        public void SwitchStageSort()
        {
            sortStage = !sortStage;
            StageSortIcon.sprite = sortStage ? UseSortSprite : NoSortSprite;
            StageSortRect.MileaseTo(UMN.SizeDelta, new Vector2(1263.09f, sortStage ? 390.7f : 158.83f), 0.5f,
                0f, EaseFunction.Back, EaseType.Out).Play();
            UpdateEventListView();
        }

        private async void GetSimulatedTime()
        {
            var timeData = await Server.Get<TimeData>("/api/time/get");
            SimulatedTime = timeData.ServerTime;
            SimulatedTimeText.text = $"\u231a 当前服务器模拟时间\n     <color=grey>{SimulatedTime}";
        }
        
        private async void FetchEventList()
        {
            UserGuessList = await Server.Get<GuessList>("/api/guess/list", ("userID", LoginManager.Account));
            allEvents = await Server.Get<EventList>("/api/event/list");
            MonthDropDown.options.Clear();
            monthReference.Clear();
            foreach (var month in allEvents.Events.Select(x => x.EventTime.Month).Distinct())
            {
                monthReference.Add(month);
                MonthDropDown.options.Add(new TMP_Dropdown.OptionData(month + "月"));
            }

            MonthDropDown.value = 0;
        }

        public async void FetchGuessList()
        {
            UserGuessList = await Server.Get<GuessList>("/api/guess/list", ("userID", LoginManager.Account));
        }

        public void UpdateDayDropDown()
        {
            var month = monthReference[MonthDropDown.value];
            DayDropDown.options.Clear();
            dayReference.Clear();
            DayDropDown.options.Add(new TMP_Dropdown.OptionData("全部"));
            dayReference.Add(-1);
            foreach (var day in allEvents.Events.Where(x => x.EventTime.Month == month).Select(x => x.EventTime.Day).Distinct())
            {
                dayReference.Add(day);
                DayDropDown.options.Add(new TMP_Dropdown.OptionData(day + "日"));
            }

            DayDropDown.value = 0;
            UpdateEventListView();
        }
        
        public void UpdateEventListView()
        {
            EventListView.Clear();
            IEnumerable<Event> events = allEvents.Events;
            if (sortDate)
            {
                var month = monthReference[MonthDropDown.value];
                events = events.Where(x => x.EventTime.Month == month);
                if (DayDropDown.value != 0)
                {
                    var day = dayReference[DayDropDown.value];
                    events = events.Where(x => x.EventTime.Day == day);
                }
            }

            if (sortStage)
            {
                switch (StageDropDown.value)
                {
                    case 0:
                        events = events.Where(x => x.StartGuessTime <= SimulatedTime && SimulatedTime < x.EventTime);
                        break;
                    case 1:
                        events = events.Where(x => SimulatedTime >= x.EndGuessTime);
                        break;
                    case 2:
                        events = events.Where(x => SimulatedTime >= x.EventTime && SimulatedTime < x.EndGuessTime);
                        break;
                    case 3:
                        events = events.Where(x => SimulatedTime < x.StartGuessTime);
                        break;
                }
            }

            if (MineOnly.isOn)
            {
                events = events.Where(x => UserGuessList.Guesses.Exists(y => y.EventID == x.ID));
                events = events.Reverse();
            }
            
            foreach (var e in events)
            {
                EventListView.Add(e);
            }
        }
    }
}
