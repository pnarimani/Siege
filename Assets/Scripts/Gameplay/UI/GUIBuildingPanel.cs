using System.Linq;
using AutofacUnity;
using Siege.Gameplay.Buildings;
using Siege.UI;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class GUIBuildingPanel : UIToolkitView
    {
        const string ScrollViewClass = "building-panel__scroll-view";

        void Start()
        {
            var buildings = Resolver.Resolve<GameData>().Buildings;
            var assets = Resolver.Resolve<BuildingAssets>();

            var tabView = Root.Q<TabView>();

            foreach (var group in buildings.GroupBy(b => b.Category))
            {
                var tab = new Tab { label = group.Key.ToString() };
                tabView.Add(tab);

                var scrollView = new ScrollView(ScrollViewMode.Horizontal)
                {
                    style = { flexGrow = 1 },
                };
                scrollView.AddToClassList(ScrollViewClass);
                tab.Add(scrollView);

                foreach (var building in group)
                {
                    var localizedName = assets.GetName(building.Id);
                    var button = new BuildingButton
                    {
                        Label = localizedName,
                        Icon = assets.GetIcon(building.Id),
                    };

                    var buildingId = building.Id;
                    button.RegisterCallback<ClickEvent>(_ => assets.Spawn(buildingId));

                    scrollView.Add(button);
                }
            }
        }
    }
}