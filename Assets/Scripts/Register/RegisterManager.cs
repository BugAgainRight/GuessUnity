using CircleOfLife.Configuration;
using CircleOfLife.General;
using Milutools.SceneRouter;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


namespace GuessUnity
{
    public class RegisterManager : MonoBehaviour
    {
        public TMP_InputField AccountInput, PasswordInput1, PasswordInput2, IDInput;
        public class RegisterData
        {
            public string Account;
            public string Password;
            public string IDNumber;
        }
        public class StatusData
        {
            public bool Success;
            public string Message;
        }

        public void OnClick()
        {
            string account, password1, password2, id;

            account = AccountInput.text;
            password1 = PasswordInput1.text;
            password2 = PasswordInput2.text;
            id = IDInput.text;

            //JudgeFormat(account, true);
            //JudgeFormat(password1,false);
            //IsMatch(password1,password2);
            //JudgeID(id);

            if (JudgeFormat(account, true) && JudgeFormat(password1, false) && IsMatch(password1, password2) && JudgeID(id))
            {
                Register(account, password1, id);
            }
            
        }

        //返回登录界面（不注册的情况）
        public void GoToLogin()
        {
            Debug.Log("此处应前往登录界面");
        }

        //对用户名和密码的初步校验
        //合法性要求：长度大于6，小于20
        bool JudgeFormat(string content, bool isAccount)
        {
            string message;

            if (content.Length <= 6 || content.Length >= 20)
            {
                if (isAccount)
                {
                    message = "不合法的用户名！";
                    Clear("account");
                }
                else {
                    message = "不合法的密码！";
                    Clear("password1");
                }
            }
            else { return true; }

            MessageBox.Open(("输入的内容不符合要求", message));
            return false;
        }

        //清除错误输入
        void Clear(string type)
        {
            if(type=="account") AccountInput.text = "";
            if (type == "password1") PasswordInput1.text = "";
            if (type == "password2") PasswordInput2.text = "";
            if (type == "id") IDInput.text = "";
        }

        //判断两次输入的密码是否相同
        bool IsMatch(string p1,string p2)
        {
            if (p1 != p2)
            {
                MessageBox.Open(("注册失败", "两次输入的密码不一致！"));
                Clear("password1");
                Clear("password2");
                return false;
            }
            return true;
        }

        //对身份证长度的合法性进行校验
        bool JudgeID(string id)
        {
            bool check;

            if (id.Length == 18)
            {
                //进入18位身份证校验过程
                check = CheckIDCard18(id);
            }
            else if (id.Length == 15)
            {
                //进入15位身份证校验过程
                check = CheckIDCard15(id);
            }
            else
            {
               MessageBox.Open(("注册失败", "身份证长度错误！")); 
               return false;
            }
            if (!check)
            {
                MessageBox.Open(("注册失败", "身份证无效！"));
                return false;
            }
            return true;
        }

        //18位身份证校验
        private bool CheckIDCard18(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber.Remove(17), out n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = idNumber.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
            {
                return false;//校验码验证  
            }
            return true;//符合GB11643-1999标准  
        }


        //15位身份证校验
        private bool CheckIDCard15(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            return true;
        }

        //向后端发送注册请求
        async Task Register(string account, string password,string id)
        {
            var state = await Server.Post<StatusData>("/api/user/register", new RegisterData()
            {
                Account = account,
                Password = password,
                IDNumber = id
            });
            if (!state.Success)
            {
                MessageBox.Open(("注册失败", state.Message));
                Clear("account");
                Clear("password1");
                Clear("password2");
                Clear("id");
            }
            else
            {
                //注册成功，进入登录界面
                //SceneRouter.GoTo
            }

        }
    }

}

