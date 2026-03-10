using Siege.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace Siege.MainMenu
{
    public class GUIMainMenu : UIToolkitView
    {
        const string PlayButtonName = "play";

        void Awake()
        {
            var playButton = Root.Q<Button>(PlayButtonName);
            playButton.clicked += () => Addressables.LoadSceneAsync("Game");
        }
    }
}