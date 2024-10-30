using Milease.Core.Animator;
using Milease.Core.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CircleOfLife
{
    public class PersonListItem : MilListViewItem
    {
        public TMP_Text Name;
        public override void AdjustAppearance(float pos)
        {

        }

        public override void OnSelect(PointerEventData eventData)
        {

        }

        public override void UpdateAppearance()
        {
            Name.text = Binding as string;
        }

        protected override MilInstantAnimator ConfigClickAnimation()
            => null;

        protected override IEnumerable<MilStateParameter> ConfigDefaultState()
            => Array.Empty<MilStateParameter>();

        protected override IEnumerable<MilStateParameter> ConfigSelectedState()
            => Array.Empty<MilStateParameter>();

        protected override void OnInitialize()
        {

        }

        protected override void OnTerminate()
        {
   
        }
    }
}
