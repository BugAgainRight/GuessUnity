using System;
using System.Collections.Generic;
using Milutools.Milutools.UI;
using TMPro;


namespace CircleOfLife
{
    public class TimeSelecterUI : ManagedUI<TimeSelecterData, DateTime, DateTime>
    {
        private DateTime receivedDateTime;
        public TextMeshProUGUI YearLabel, MonthLabel, DayLabel, HourLabel, MinuteLabel;
        public TMP_Dropdown YearDropdown, MonthDropdown, DateDropdown, HourDropdown, MinuteDropdown;
        //获取所有dropdown的option
        private List<TMP_Dropdown.OptionData> yearDropdownOptionData = TimeDropdownOptionData.YearDropdownOptionData;
        private List<TMP_Dropdown.OptionData> monthDropdownOptionData = TimeDropdownOptionData.MonthDropdownOptionData;
        private List<TMP_Dropdown.OptionData> julyDateDropdownOptionData = TimeDropdownOptionData.JulyDateDropdownOptionData;
        private List<TMP_Dropdown.OptionData> augustDateDropdownOptionData = TimeDropdownOptionData.AugustDateDropdownOptionData;
        private List<TMP_Dropdown.OptionData> hourDropdownOptionData = TimeDropdownOptionData.HourDropdownOptionData;
        private List<TMP_Dropdown.OptionData> minuteDropdownOptionData = TimeDropdownOptionData.MinuteDropdownOptionData;
        public override void AboutToOpen(DateTime parameter)
        {
            receivedDateTime = parameter;
            //设置所有dropdown的option
            ClearAllDropdown();
            SetDropdownOptions();
            //应用参数
            SetDateTime();
        }

        protected override void AboutToClose()
        {
            Close(new DateTime(
                int.Parse(YearLabel.text),
                int.Parse(MonthLabel.text),
                int.Parse(DayLabel.text),
                int.Parse(HourLabel.text),
                int.Parse(MinuteLabel.text),
                0));
        }

        protected override void Begin()
        {

        }


        //根据月份改变日期的optionitem
        public void OnMonthLabelChange()
        {

            switch (MonthLabel.text)
            {
                case "7":
                    DateDropdown.ClearOptions();
                    DateDropdown.AddOptions(julyDateDropdownOptionData);
                    break;
                case "8":
                    DateDropdown.ClearOptions();
                    DateDropdown.AddOptions(augustDateDropdownOptionData);
                    break;
                default:
                    break;
            }
        }

        public void ClearAllDropdown()
        {
            YearDropdown.ClearOptions();
            MonthDropdown.ClearOptions();
            DateDropdown.ClearOptions();
            HourDropdown.ClearOptions();
            MinuteDropdown.ClearOptions();
        }

        public void SetDropdownOptions()
        {
            YearDropdown.AddOptions(yearDropdownOptionData);
            MonthDropdown.AddOptions(monthDropdownOptionData);
            switch (receivedDateTime.Month)
            {
                case 7:
                    DateDropdown.AddOptions(julyDateDropdownOptionData);
                    break;
                case 8:
                    DateDropdown.AddOptions(augustDateDropdownOptionData);
                    break;
                default:
                    break;
            }
            HourDropdown.AddOptions(hourDropdownOptionData);
            MinuteDropdown.AddOptions(minuteDropdownOptionData);
        }
        public void SetDateTime()
        {
            YearDropdown.value = receivedDateTime.Year - 2024;
            MonthDropdown.value = receivedDateTime.Month - 7;
            switch (receivedDateTime.Month)
            {
                case 7:
                    DateDropdown.value = receivedDateTime.Day - 23;
                    break;
                case 8:
                    DateDropdown.value = receivedDateTime.Day - 1;
                    break;
                default:
                    break;
            }
            HourDropdown.value = receivedDateTime.Hour;
            MinuteDropdown.value = receivedDateTime.Minute;

        }
    }
}
