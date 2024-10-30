using System;
using System.Collections;
using System.Collections.Generic;
using Milease.Core.Animator;
using Milease.Core.UI;
using Milease.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CircleOfLife
{
    public class EventListItem : MilListViewItem
    {
        public TMP_Text Title, VS, Time, Start, Status;
        public GameObject Guessed;
        
        protected override IEnumerable<MilStateParameter> ConfigDefaultState()
            => ArraySegment<MilStateParameter>.Empty;

        protected override IEnumerable<MilStateParameter> ConfigSelectedState()
            => ArraySegment<MilStateParameter>.Empty;

        public override void OnSelect(PointerEventData eventData)
        {

        }

        protected override void OnInitialize()
        {

        }

        protected override void OnTerminate()
        {

        }

        protected override MilInstantAnimator ConfigClickAnimation()
            => null;

        public override void UpdateAppearance()
        {
            var data = (Event)Binding;
            Title.text = data.Name;
            VS.text = $"🔥 {data.PartyACountry} <b>VS</b> {data.PartyBCountry}";
            Start.text = $"⌚ {data.EventTime}";
            var color = Color.black;
            if (MainUIController.SimulatedTime < data.StartGuessTime)
            {
                Time.text = "开始竞猜时间：" + data.StartGuessTime;
                Status.text = "【未开始】";
            }
            else if (MainUIController.SimulatedTime >= data.StartGuessTime &&
                     MainUIController.SimulatedTime < data.EventTime)
            {
                Time.text = "竞猜截止时间：" + data.EventTime;
                Status.text = "【进行中】";
                color = ColorUtils.RGB(235, 68, 80);
            }
            else if (MainUIController.SimulatedTime >= data.EventTime &&
                      MainUIController.SimulatedTime < data.EndGuessTime)
            {
                Time.text = "等待公布结果，结果公布时间：" + data.StartGuessTime;
                Status.text = "【等待结果】";
            }
            else
            {
                Time.text = "已结束竞猜，胜方：<color=red>" + (data.Winner == 0 ? data.PartyACountry : data.PartyBCountry);
                Status.text = "【已结束】";
                color = ColorUtils.RGB(65, 88, 208);
            }
            Start.color = Status.color = Title.color = color;
            
            Guessed.SetActive(MainUIController.UserGuessList.Guesses.Exists(x => x.EventID == data.ID));
        }

        public override void AdjustAppearance(float pos)
        {
 
        }
    }
}
