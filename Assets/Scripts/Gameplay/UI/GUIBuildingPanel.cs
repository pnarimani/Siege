using Siege.UI;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class GUIBuildingPanel : UIToolkitView
    {
        const string ScrollViewClass = "building-panel__scroll-view";

        void Awake()
        {
            var tabView = Root.Q<TabView>();

            for (var j = 0; j < 6; j++)
            {
                var tab = new Tab
                {
                    label = $"Tab {j + 1}",
                };

                tabView.Add(tab);

                var scrollView = new ScrollView(ScrollViewMode.Horizontal)
                {
                    style = { flexGrow = 1 },
                };
                scrollView.AddToClassList(ScrollViewClass);
                tab.Add(scrollView);

                for (var i = 0; i < 5; i++)
                {
                    scrollView.Add(new BuildingButton { Label = "Building " + (i + 1) });
                }
            }
        }
    }
}