using System.Collections;
using System.Collections.Generic;
using GuessUnity;
using Milease.Core.UI;
using Milutools.Milutools.UI;
using TMPro;
using UnityEngine;

namespace CircleOfLife
{
    public class RedeemUI : ManagedUI<RedeemUI, PrizeList>
    {
        public static RedeemUI Instance;
        public MilListView PrizeListView;
        public TMP_Text Remain;
        
        protected override void Begin()
        {
            Instance = this;
        }

        protected override void AboutToClose()
        {

        }

        public override void AboutToOpen(PrizeList parameter)
        {
            foreach (var prize in parameter.Prizes)
            {
                PrizeListView.Add(prize);
            }

            FetchUserInfo();
        }

        public async void FetchUserInfo()
        {
            var userInfo = await Server.Get<UserInfo>("api/user/info", ("Account", LoginManager.Account));
            Remain.text = "积分余额：" + userInfo.Points.ToString("F2");
        }
    }
}
