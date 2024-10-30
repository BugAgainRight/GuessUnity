using System.Collections;
using System.Collections.Generic;
using CircleOfLife.General;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace CircleOfLife
{
    public enum ReadMode
    {
        Read, Update
    }
    public class UserInfoManager : MonoBehaviour
    {
        public List<TextMeshProUGUI> inputTextList;
        public List<TextMeshProUGUI> ContextList;
        public List<GameObject> readModeObjectList;
        public List<GameObject> updateModeObjectList;
        private UserInfo userInfo;
        private UpdateInfoRequest updateInfoRequest;
        private int childCount;
        private string userInfoPath = "api/user/info";
        private ReadMode readMode;
        public Transform ButtonPannel;
        private void Awake()
        {

            inputTextList = new List<TextMeshProUGUI>();
            ContextList = new List<TextMeshProUGUI>();
            readModeObjectList = new List<GameObject>();
            updateModeObjectList = new List<GameObject>();
            GetNecessaryCompoment();
            GetUserInfo();
        }
        void Update()
        {
            OnModeChange();
        }
        private void GetNecessaryCompoment()
        {
            childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Transform readObj = child.GetChild(0);
                Transform updateObj = child.GetChild(1);
                Transform context = readObj.GetChild(0);
                Transform inputArea = updateObj.GetChild(1).GetChild(0);
                int inputAreaChildCount = inputArea.childCount;
                Transform inputText = inputArea.GetChild(inputAreaChildCount - 1);

                inputTextList.Add(inputText.GetComponent<TextMeshProUGUI>());
                ContextList.Add(context.GetComponent<TextMeshProUGUI>());
                readModeObjectList.Add(readObj.gameObject);
                updateModeObjectList.Add(updateObj.gameObject);
            }
        }
        #region Sever
        public async void GetUserInfo()
        {
            //userInfo = await Server.Get<UserInfo>(userInfoPath, ("id", "1"));
            userInfo = new UserInfo()
            {
                Name = "张三",
                PhoneNumber = "123456789",
                Address = "北京市海淀区",
                //Account = "123456789"
            };
            SetContext();
        }

        public async void SendUpdateInfoRequest()
        {
            MessageBox.Open(("提交成功:\n", "提交内容:\n用户名" + updateInfoRequest.Name +
            "\n电话号码" + updateInfoRequest.PhoneNumber +
            "\n地址" + updateInfoRequest.Address));
            StatusData context = await Server.Post<StatusData>(userInfoPath, updateInfoRequest);
            if (context.Success)
            {
                MessageBox.Open(("修改成功!", context.Message));
                SetContextFromUpdateInfoRequest();
            }
            else
            {
                MessageBox.Open(("修改失败:", context.Message));
            }
        }
        // Add other public methods or properties here if needed
        #endregion
        #region UI
        public void SetContext()
        {
            ContextList[0].text = userInfo.Name;
            ContextList[1].text = userInfo.PhoneNumber;
            ContextList[2].text = userInfo.Address;
            //ContextList[3].text=userInfo.Account;
        }
        public void SetContextFromUpdateInfoRequest()
        {
            ContextList[0].text = updateInfoRequest.Name;
            ContextList[1].text = updateInfoRequest.PhoneNumber;
            ContextList[2].text = updateInfoRequest.Address;
            //ContextList[3].text=updateInfoRequest.Account;
        }
        public void GetInputText()
        {
            updateInfoRequest = new UpdateInfoRequest();
            updateInfoRequest.Name = inputTextList[0].text;
            updateInfoRequest.PhoneNumber = inputTextList[1].text;
            updateInfoRequest.Address = inputTextList[2].text;
            //updateInfoRequest.Account=inputTextList[3].text;
        }
        #endregion
        #region Mode
        public void SetReadMode()
        {
            readMode = ReadMode.Read;
            OnModeChange();
        }
        public void SetUpdateMode()
        {
            readMode = ReadMode.Update;
            OnModeChange();
        }
        public void OnModeChange()
        {
            switch (readMode)
            {
                case ReadMode.Read:
                    ButtonPannel.GetChild(0).gameObject.SetActive(true);
                    ButtonPannel.GetChild(1).gameObject.SetActive(false);
                    foreach (var item in readModeObjectList)
                    {
                        item.SetActive(true);
                    }
                    foreach (var item in updateModeObjectList)
                    {
                        item.SetActive(false);
                    }
                    break;
                case ReadMode.Update:
                    ButtonPannel.GetChild(0).gameObject.SetActive(false);
                    ButtonPannel.GetChild(1).gameObject.SetActive(true);
                    foreach (var item in readModeObjectList)
                    {
                        item.SetActive(false);
                    }
                    foreach (var item in updateModeObjectList)
                    {
                        item.SetActive(true);
                    }
                    break;
            }
        }
        #endregion
        #region AcceptOrDent
        public void OnAccept()
        {
            GetInputText();
            SendUpdateInfoRequest();
            SetReadMode();
        }

        public void OnDeny()
        {
            SetReadMode();
        }

        #endregion
    }
}
