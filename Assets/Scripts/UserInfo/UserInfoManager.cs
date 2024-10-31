using System.Collections;
using System.Collections.Generic;
using CircleOfLife.General;
using GuessUnity;
using TMPro;
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
        private List<TMP_InputField> inputTextList;
        private List<TextMeshProUGUI> contextList;
        private List<GameObject> readModeObjectList;
        private List<GameObject> updateModeObjectList;
        private UserInfo userInfo;
        private UpdateInfoRequest updateInfoRequest;
        private int childCount;
        private string userInfoPath = "api/user/info";
        private ReadMode readMode;
        public Transform ButtonPannel;
        public TMP_Text Account, Points;
        
        private void Awake()
        {
            Account.text = LoginManager.Account;
            inputTextList = new List<TMP_InputField>();
            contextList = new List<TextMeshProUGUI>();
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
                // 这个用 Inspector 绑定会比较好，Alpha 阶段后面可以改一下
                Transform child = transform.GetChild(i);
                Transform readObj = child.GetChild(0);
                Transform updateObj = child.GetChild(1);
                Transform context = readObj.GetChild(0);
                inputTextList.Add(updateObj.GetChild(1).GetComponent<TMP_InputField>());
                contextList.Add(context.GetComponent<TextMeshProUGUI>());
                readModeObjectList.Add(readObj.gameObject);
                updateModeObjectList.Add(updateObj.gameObject);
            }
        }
        #region Sever
        public async void GetUserInfo()
        {
            // 从服务器获取用户信息
            userInfo = await Server.Get<UserInfo>(userInfoPath, ("Account", LoginManager.Account));
            SetContext();
        }

        public async void SendUpdateInfoRequest()
        {
            if (string.IsNullOrEmpty(updateInfoRequest.Name))
            {
                MessageBox.Open(("错误", "用户名不能为空哦！"));
                return;
            }
            /**MessageBox.Open(("提交成功:\n", "提交内容:\n用户名" + updateInfoRequest.Name +
            "\n电话号码" + updateInfoRequest.PhoneNumber +
            "\n地址" + updateInfoRequest.Address +
            "\n账号" + updateInfoRequest.Account));**/
            updateInfoRequest.Account = LoginManager.Account;
            StatusData context = await Server.Post<StatusData>("/api/user/updateinfo", updateInfoRequest);
            if (context.Success)
            {
                MessageBox.Open(("修改成功!", context.Message));
                SetContextFromUpdateInfoRequest();
            }
            else
            {
                MessageBox.Open(("修改失败！", context.Message));
            }
        }
        // Add other public methods or properties here if needed
        #endregion
        #region UI
        public void SetContext()
        {
            contextList[0].text = userInfo.Name;
            contextList[1].text = userInfo.PhoneNumber;
            contextList[2].text = userInfo.Address;
            contextList[3].text = userInfo.Account;
            
            inputTextList[0].text = userInfo.Name;
            inputTextList[1].text = userInfo.PhoneNumber;
            inputTextList[2].text = userInfo.Address;
            inputTextList[3].text = userInfo.Account;

            Points.text = $"积分：{userInfo.Points:F2}";
        }
        public void SetContextFromUpdateInfoRequest()
        {
            contextList[0].text = updateInfoRequest.Name;
            contextList[1].text = updateInfoRequest.PhoneNumber;
            contextList[2].text = updateInfoRequest.Address;
            contextList[3].text = updateInfoRequest.Account;
        }
        public void GetInputText()
        {
            updateInfoRequest = new UpdateInfoRequest();
            updateInfoRequest.Name = inputTextList[0].text;
            updateInfoRequest.PhoneNumber = inputTextList[1].text;
            updateInfoRequest.Address = inputTextList[2].text;
            updateInfoRequest.Account = inputTextList[3].text;
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
