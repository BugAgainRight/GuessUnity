using Milease.Enums;
using Milease.Utils;
using Milutools.Milutools.UI;
using UnityEngine;

namespace GuessUnity
{
    public class RegisterUI : ManagedUI<RegisterUI, object>
    {
        public RectTransform Panel;
        protected override void Begin()
        {
            
        }

        protected override void AboutToClose()
        {
            Panel.Milease(UMN.AnchoredPosition, new Vector2(0f, -160f),new Vector2(0, -1500f), 
                    0.25f, 0f, EaseFunction.Bezier, EaseType.In)
                .Play();
        }

        public override void AboutToOpen(object parameter)
        {
            Panel.Milease(UMN.AnchoredPosition, new Vector2(0, -1500f), new Vector2(0f, -160f),
                    0.5f, 0f, EaseFunction.Bezier, EaseType.Out)
                .Play();
        }
    }
}
