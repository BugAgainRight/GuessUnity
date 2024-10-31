using System.Collections;
using System.Collections.Generic;
using Milease.Core.UI;
using Milutools.Milutools.UI;
using Newtonsoft.Json;
using UnityEngine;

namespace CircleOfLife
{
    public class MessageUI : ManagedUI<MessageUI, MessageList>
    {
        public MilListView MessageList;

        private MessageList list;
        
        protected override void Begin()
        {

        }

        protected override void AboutToClose()
        {

        }

        public void ReadAndClose()
        {
            foreach (var msg in list.Messages)
            {
                if (!MainUIController.ReadList.ReadMessages.Contains(msg.ID))
                {
                    MainUIController.ReadList.ReadMessages.Add(msg.ID);
                }
            }

            PlayerPrefs.SetString("read", JsonConvert.SerializeObject(MainUIController.ReadList));
            Close();
        }

        public override void AboutToOpen(MessageList parameter)
        {
            parameter.Messages.Reverse();
            list = parameter;
            foreach (var msg in parameter.Messages)
            {
                MessageList.Add(msg);
            }
        }
    }
}
