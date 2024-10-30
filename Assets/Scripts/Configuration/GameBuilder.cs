using CircleOfLife.General;
using Milease.Configuration;
using Milutools.Audio;
using Milutools.Milutools.UI;
using Milutools.SceneRouter;
using UnityEngine;

namespace CircleOfLife.Configuration
{
    public class GameBuilder : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        public static void Setup()
        {
            MileaseConfiguration.Configuration.DefaultColorTransformationType = ColorTransformationType.RGB;
            SetupSceneRouter();
            SetupUIManager();
            AudioManager.Setup("AudioResources");
        }

        private static void SetupSceneRouter()
        {
            // 配置场景路由
            SceneRouter.Setup(new SceneRouterConfig()
            {
                SceneNodes = new []
                {
                    // 场景节点：场景ID，自定义路径，场景名
                    SceneRouter.Root(SceneIdentifier.WeatherTest, "WeatherSystem"), // 设置根节点
                    SceneRouter.Node(SceneIdentifier.MilutoolsSample, "sample", "MilutoolsSample"),
                    SceneRouter.Node(SceneIdentifier.SceneRouterSample, "sample/scene-router", "SceneRouterSample")
                },
                LoadingAnimators = new []
                {
                    // 设置可用的自定义加载动画
                    SceneRouter.LoadingAnimator(LoadingAnimatorIdentifier.GenshinLoading, 
                        Resources.Load<GameObject>("Loading/GenshinLoading"))
                }
            });
            
            // 设置默认加载动画，不设置则是黑屏过渡
            SceneRouter.SetLoadingAnimator(LoadingAnimatorIdentifier.GenshinLoading);
        }

        private static void SetupUIManager()
        {
            // 注册 UI
            UIManager.Setup(new []
            {
                // 从 Resources/UI/ 中加载 UI 预制体
                UI.FromResources(UIIdentifier.MessageBox, "UI/MessageBox"),
                UI.FromResources(UIIdentifier.UISample, "UI/UISample")
            });
        }
    }
}
