using CircleOfLife.Configuration;
using CircleOfLife.General;
using Milutools.SceneRouter;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace GuessUnity
{
    public class LoginManager : MonoBehaviour
    {

        public TMP_InputField AccountInput, PasswordInput;
        public class LoginData
        {
            public string Account;
            public string Password;
        }
        public class LoginResponse
        {
            public bool Success;
            public string Message;
        }

        // Start is called before the first frame update
        public void Start()
        {
            string account = " ";
            string password = " ";
            string judgeAccount = JudgeFormat(AccountInput.text, true);
            string judgePassword = JudgeFormat(PasswordInput.text, false);


            if (judgeAccount == "legal" && judgePassword == "legal")
            {
                account = AccountInput.text;
                password = PasswordInput.text;
                
                Verify(account, password);
            }
            else
            {
                string errorContent="";

                if (judgeAccount != "legal") errorContent += judgeAccount;
                if(judgePassword!="legal") errorContent += judgePassword;

                MessageBox.Open(("输入错误", errorContent));
   
                Clear();
            }

            //对用户名和密码的初步校验
            //合法性要求：长度大于6，小于20
            string JudgeFormat(string content, bool isAccount)
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
                else
                {
                    //格式正确
                    message = "legal";
                }


                return message;
            }

            //清除错误输入
            void Clear()
            {
                AccountInput.text = "";
                PasswordInput.text = "";
            }

            //向后端发送请求校验用户名密码
            async Task Verify(string account, string password)
            {
                var state = await Server.Post<LoginResponse>("/api/user/login", new LoginData()
                {
                    Account = account,
                    Password = password
                });
                if (!state.Success)
                {
                    MessageBox.Open(("登录失败", state.Message));
                    Clear();
                }
                else
                {
                    //校验成功，进入主界面
                    //SceneRouter.GoTo
                }

            }

         
        }
    }
}
