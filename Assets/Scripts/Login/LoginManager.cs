using CircleOfLife.General;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace GuessUnity
{
    public class LoginManager : MonoBehaviour
    {

        public TMP_InputField AccountInput, PasswordInput;
      
        // Start is called before the first frame update
        public void Start()
        {
            string account = " ";
            string password = " ";
            string judgeAccount = JudgeFormat(AccountInput.text,true);
            string judgePassword= JudgeFormat(PasswordInput.text,false);

            
            if (judgeAccount == "legal" && judgePassword == "legal")
            {
                account=AccountInput.text;
                password=PasswordInput.text;
            }
            else
            {
                Debug.Log(judgeAccount);
                Debug.Log(judgePassword);
                Clear();
            }
        }



        //对用户名和密码的初步校验
        //合法性要求：长度大于6，小于20
        string JudgeFormat(string content,bool isAccount)
        {
            string message;

            if (content == null)
            {
                if (isAccount) message = "用户名不能为空！";
                else message = "密码不能为空！";

                return message;
                
            }
            else if (content.Length <= 6 || content.Length >= 20)
            {
                if (isAccount) message = "不合法的用户名！";
                else message = "不合法的密码！";

                return message;
            }

            //格式正确
            message = "legal";
            return message;
        }

        //清除错误输入
        void Clear()
        {
            AccountInput.text = "";
            PasswordInput.text = "";
        }

    }
}
