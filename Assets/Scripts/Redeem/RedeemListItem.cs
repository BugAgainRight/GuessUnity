using System;
using System.Collections;
using System.Collections.Generic;
using CircleOfLife.General;
using GuessUnity;
using Milease.Core.Animator;
using Milease.Core.UI;
using Paraparty.Colors;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ColorUtils = Milease.Utils.ColorUtils;

namespace CircleOfLife
{
    public class RedeemListItem : MilListViewItem
    {
        public TMP_Text Title, Storage, Need, BtnText;
        public Sprite ActiveSprite, DeactiveSprite;
        public Image BtnImage;
        
        protected override IEnumerable<MilStateParameter> ConfigDefaultState()
            => ArraySegment<MilStateParameter>.Empty;

        protected override IEnumerable<MilStateParameter> ConfigSelectedState()
            => ArraySegment<MilStateParameter>.Empty;

        public override void OnSelect(PointerEventData eventData)
        {
            
        }

        public async void Redeem()
        {
            var data = (Prize)Binding;
            var state = await Server.Get<StatusData>("/api/prizes/redeem",
                ("account", LoginManager.Account), ("id", data.ID));
            if (state.Success)
            {
                data.Redeemed = true;
                data.Stock--;
                UpdateAppearance();
                RedeemUI.Instance.FetchUserInfo();
                MessageBox.Open(("兑换成功", "将发货至您账号信息中的地址。"));
            }
            else
            {
                MessageBox.Open(("兑换失败", state.Message));
            }
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
            var data = (Prize)Binding;
            Title.text = data.Name;
            Storage.text = $"库存数量：{data.Stock}";
            Need.text = $"所需积分：{data.PointsRequired}";

            if (data.Redeemed)
            {
                BtnImage.sprite = DeactiveSprite;
                BtnImage.color = ColorUtils.RGB(204, 204, 204);
                BtnText.text = "已兑换";
            } 
            else if (data.Stock <= 0){
                BtnImage.sprite = DeactiveSprite;
                BtnImage.color = ColorUtils.RGB(204, 204, 204);
                BtnText.text = "库存不足";
            }
            else{
                BtnImage.sprite = ActiveSprite;
                BtnImage.color = Color.white;
                BtnText.text = "兑换";
            }
        }

        public override void AdjustAppearance(float pos)
        {
 
        }
    }
}
