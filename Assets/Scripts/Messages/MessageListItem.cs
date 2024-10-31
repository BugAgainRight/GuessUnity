using System;
using System.Collections;
using System.Collections.Generic;
using GuessUnity;
using Milease.Core.Animator;
using Milease.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CircleOfLife
{
    public class MessageListItem : MilListViewItem
    {
        public TMP_Text Content, Time;
        public GameObject NewMessage;
        
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
            var data = (MessageData)Binding;
            Content.text = data.Content;
            Time.text = data.Time.ToString();
            
            NewMessage.SetActive(MainUIController.ReadList.ReadMessages.Contains(data.ID));
        }

        public override void AdjustAppearance(float pos)
        {
 
        }
    }
}
