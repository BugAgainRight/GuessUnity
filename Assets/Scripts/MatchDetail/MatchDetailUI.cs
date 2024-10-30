using CircleOfLife.General;
using CircleOfLife.Sample;
using Milease.Core.UI;
using Milutools.Milutools.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CircleOfLife
{
    public class PersonListData
    {
        public string Introduce;
        public string[] PartyNames;
    }
    public class MatchDetailUI : ManagedUI<MatchDetailUI, Event>
    {
        public static string TmpAccount = "123456";
        public static System.DateTime NowTime=>MainUIController.SimulatedTime;
        public TextMeshProUGUI MatchName;
        public TextMeshProUGUI PartyANames;
        public TextMeshProUGUI PartyBNames;
        public TextMeshProUGUI PartyACountry;
        public TextMeshProUGUI PartyBCountry;
        public TextMeshProUGUI ARate;
        public TextMeshProUGUI BRate;
        public TextMeshProUGUI FakeGuessButtonText;
        public GameObject GuessButton, FakeGuessButton;
        public GameObject WinnerA, WinnerB;
        public Toggle AToggle, BToggle;
        private bool alreadySelect = false;
        private bool finishGuess = false;
        private bool beginGuess = false;
        private string fakeButtonWord = "竞猜未开始!";

        private GuessData guessData = new();

        private Event eventData;

        public bool CanGuess => !alreadySelect && !finishGuess && beginGuess;

        public override void AboutToOpen(Event parameter)
        {
            eventData = parameter;
            MatchName.text = parameter.Name;

            PartyANames.text = eventData.PartyANames[0];
            PartyBNames.text = eventData.PartyBNames[0];

            PartyACountry.text = parameter.PartyACountry;
            PartyBCountry.text = parameter.PartyBCountry;




            UpdateComponentState();


        }
        /// <summary>
        /// 更新一些组件的显示
        /// </summary>
        private async void UpdateComponentState()
        {
            var eventGuessData = await Server.Get<EventGuessData>("api/guess/event", ("account", TmpAccount), ("eventID", eventData.ID));

            guessData.EventID = eventData.ID;
            guessData.GuessWinner = eventGuessData.UserGuess;
            int allCount = eventGuessData.GuessCount[0] + eventGuessData.GuessCount[1];
            allCount = Mathf.Max(1, allCount);
            ARate.text = "当前赔率：" + (((int)(100f * eventGuessData.GuessCount[0] / allCount)) / 100f).ToString();
            BRate.text = "当前赔率：" + (((int)(100f * eventGuessData.GuessCount[1] / allCount)) / 100f).ToString();

            alreadySelect = guessData.GuessWinner != -1;

            finishGuess = eventData.EventTime < NowTime;
            beginGuess = eventData.StartGuessTime < NowTime;

            if (eventData.EndGuessTime < NowTime)
            {
                fakeButtonWord = "已结束";
                if (eventData.Winner == 0) WinnerA.SetActive(true);
                else if (eventData.Winner == 1) WinnerB.SetActive(true);
            }
            else if (finishGuess) fakeButtonWord = "结束竞猜";
            else if (alreadySelect) fakeButtonWord = "已经竞猜";
            else if (!beginGuess) fakeButtonWord = "竞猜未开始";

            FakeGuessButtonText.text = fakeButtonWord;

            if (CanGuess)
            {
                GuessButton.SetActive(true);
                FakeGuessButton.SetActive(false);
            }
            else
            {
                GuessButton.SetActive(false);
                FakeGuessButton.SetActive(true);
            }

            if (alreadySelect)
            {
                bool aState = guessData.GuessWinner == 0;
                AToggle.isOn = aState;
                BToggle.isOn = !aState;
                return;
            }
            else if(!CanGuess)
            {
                AToggle.isOn = false;
                BToggle.isOn = false;
            }


        }
        protected override void AboutToClose()
        {

        }

        protected override void Begin()
        {

        }

        public void Select(bool isA)
        {

            if (alreadySelect)
            {
                bool aState = guessData.GuessWinner == 0;
                AToggle.isOn = aState;
                BToggle.isOn = !aState;
                return;
            }
            else if (!CanGuess)
            {
                AToggle.isOn = false;
                BToggle.isOn = false;
                return;

            }

            if (isA)
            {
                if (AToggle.isOn) BToggle.isOn = false;
            }
            else
            {
                if (BToggle.isOn) AToggle.isOn = false;
            }

        }

        public void Quiz()
        {
            bool isGuessA = AToggle.isOn;
            bool isTeamWork = eventData.PartyANames.Length > 1;
            string guessWinnerName;
            if (isTeamWork)
            {
                guessWinnerName = isGuessA ? eventData.PartyACountry : eventData.PartyBCountry;
            }
            else
            {
                guessWinnerName = isGuessA ? eventData.PartyANames[0] : eventData.PartyBNames[0];
            }
            MessageBox.Open(("请确认竞猜选择", "你竞猜的获胜方是：" + guessWinnerName + "，你确认竞猜吗？确认后将无法修改！"), async (operation) =>
            {
                if (operation == MessageBox.Operation.Deny)
                {
                    return;
                }

                //调用后端api

                StatusData statusData = await Server.Get<StatusData>("api/guess/confirm", ("account", TmpAccount), ("id", eventData.ID), ("winner", (isGuessA ? 0 : 1).ToString()));
                if (statusData.Success)
                {
                    if (isGuessA) guessData.GuessWinner = 0;
                    else guessData.GuessWinner = 1;

                    alreadySelect = true;
                    UpdateComponentState();
                }
                else
                {
                    MessageBox.Open(("错误！", statusData.Message));
                }

            });
        }
        public void ShowPersonList(bool isA)
        {
            PersonListData personListData = new PersonListData();

            if (isA)
            {
                personListData.Introduce = eventData.PartyACountry + " 队伍名单";
                personListData.PartyNames = eventData.PartyANames;
            }
            else
            {
                personListData.Introduce = eventData.PartyBCountry + " 队伍名单";
                personListData.PartyNames = eventData.PartyBNames;

            }
            PersonListUI.Open(personListData);

        }


    }
}
