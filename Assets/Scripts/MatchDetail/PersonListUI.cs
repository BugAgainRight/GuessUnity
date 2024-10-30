using CircleOfLife.Configuration;
using Milease.Core.UI;
using Milutools.Milutools.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CircleOfLife
{
    public class PersonListUI : ManagedUI<PersonListUI, PersonListData>
    {
        public TextMeshProUGUI teamIntroduce;
        public MilListView MilListView;
        public override void AboutToOpen(PersonListData parameter)
        {
            teamIntroduce.text = parameter.Introduce;
            foreach(var a in parameter.PartyNames)
            {
                MilListView.Add(a);
            }

        }

        protected override void AboutToClose()
        {
          
        }

        protected override void Begin()
        {
            
        }

    }
}
