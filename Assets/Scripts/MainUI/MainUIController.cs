using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CircleOfLife.General;
using Milease.Core.UI;
using Milease.Enums;
using Milease.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CircleOfLife
{
    public class MainUIController : MonoBehaviour
    {
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

        private void Awake()
        {
            FetchEventList();
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

        private async void FetchEventList()
        {
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
            
            foreach (var e in events)
            {
                EventListView.Add(e);
            }
        }
    }
}
