using System.Collections.Generic;
using AutofacUnity;
using Siege.Gameplay.Buildings;
using Siege.Gameplay.Laws;
using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class GUIBuildingPanel : MonoBehaviour, IBackButtonHandler
    {
        const string PanelInClass = "building-panel--in";
        const string ActivatedClass = "building-button--activated";
        const string DeactivatedClass = "building-button--disabled";
        const string RecipeSelectedClass = "recipe-card--selected";

        VisualElement _root;
        VisualElement _productionSection;
        VisualElement _storageSection;

        Label _title;
        Label _workersLabel;
        VisualElement _recipeList;
        ProgressBar _productionProgress;
        SiegeButton _toggleButton;
        SiegeButton _destroyButton;
        SiegeButton _closeButton;

        Building _selected;
        ResourceStorage _resourceStorage;
        GameState _gameState;
        LawDispatcher _lawDispatcher;
        BackButtonManager _backButtonManager;

        readonly List<VisualElement> _recipeCards = new();

        void Awake()
        {
            _root = this.FindElement<VisualElement>("Root");
            _productionSection = this.FindElement<VisualElement>("ProductionBuilding");
            _storageSection = this.FindElement<VisualElement>("StorageBuilding");
            _title = this.FindElement<Label>("Title");
            _workersLabel = this.FindElement<Label>("WorkersLabel");
            _recipeList = this.FindElement<VisualElement>("RecipeList");
            _productionProgress = this.FindElement<ProgressBar>("ProductionProgress");
            _toggleButton = this.FindElement<SiegeButton>("BuildingToggle");
            _destroyButton = this.FindElement<SiegeButton>("DestroyBuilding");
            _closeButton = this.FindElement<SiegeButton>("CloseButton");

            _toggleButton.Clicked += OnToggleClicked;
            _destroyButton.Clicked += OnDestroyClicked;
            _closeButton.Clicked += () => OnBuildingSelected(null);

            Building.Selected += OnBuildingSelected;
        }

        void Start()
        {
            _resourceStorage = Resolver.Resolve<ResourceStorage>();
            _gameState = Resolver.Resolve<GameState>();
            _lawDispatcher = Resolver.Resolve<LawDispatcher>();
            _backButtonManager = Resolver.Resolve<BackButtonManager>();
        }

        void OnDestroy()
        {
            Building.Selected -= OnBuildingSelected;
        }

        void OnBuildingSelected(Building building)
        {
            _selected = building;

            if (building == null)
            {
                HidePanel();
                return;
            }

            ShowPanel(building);
        }

        // ── IBackButtonHandler ────────────────────────────────────────

        public void OnBackButtonPressed() => OnBuildingSelected(null);

        // ─────────────────────────────────────────────────────────────

        void ShowPanel(Building building)
        {
            if (_title != null)
                _title.text = building.Definition.Name;

            var isStorage = building.Definition.IsStorage;
            if (_productionSection != null)
                _productionSection.style.display = isStorage ? DisplayStyle.None : DisplayStyle.Flex;
            if (_storageSection != null)
                _storageSection.style.display = isStorage ? DisplayStyle.Flex : DisplayStyle.None;

            if (!isStorage)
            {
                RebuildRecipeButtons(building);
                UpdateWorkers(building);
                UpdateToggleButton(building);
            }
            else
            {
                UpdateStorageDisplay(building);
            }

            _backButtonManager?.PushHandler(this);
            _root?.AddToClassList(PanelInClass);
        }

        void HidePanel()
        {
            _backButtonManager?.PopHandler(this);
            _root?.RemoveFromClassList(PanelInClass);
        }

        void Update()
        {
            if (_selected == null) return;

            if (_selected.Definition.IsStorage)
            {
                UpdateStorageDisplay(_selected);
            }
            else
            {
                UpdateWorkers(_selected);
                UpdateProductionProgress(_selected);
            }
        }

        // ── Production ─────────────────────────────────────────────────

        void RebuildRecipeButtons(Building building)
        {
            _recipeCards.Clear();
            if (_recipeList == null) return;

            _recipeList.Clear();

            var cycle = building.GetComponent<ProductionCycleState>();
            if (cycle == null) return;

            var available = cycle.GetAvailableRecipes();
            for (var i = 0; i < available.Count; i++)
            {
                var recipe = available[i];
                var capturedIndex = i;

                var card = new VisualElement();
                card.AddToClassList("recipe-card");
                if (i == cycle.SelectedRecipeIndex)
                    card.AddToClassList(RecipeSelectedClass);

                var nameLabel = new Label(recipe.Name);
                nameLabel.AddToClassList("recipe-card__name");
                card.Add(nameLabel);

                var durationLabel = new Label($"{recipe.DurationSeconds:0.#}s per cycle");
                durationLabel.AddToClassList("recipe-card__duration");
                card.Add(durationLabel);

                AddResourceRow(card, recipe.Inputs, recipe.Outputs);

                card.pickingMode = PickingMode.Position;
                card.RegisterCallback<ClickEvent>(_ => OnRecipeSelected(building, capturedIndex));

                _recipeList.Add(card);
                _recipeCards.Add(card);
            }
        }

        static void AddResourceRow(VisualElement card, ResourceQuantity[] inputs, ResourceQuantity[] outputs)
        {
            if ((inputs == null || inputs.Length == 0) && (outputs == null || outputs.Length == 0))
                return;

            var row = new VisualElement();
            row.AddToClassList("recipe-card__resources");

            if (inputs != null)
                foreach (var rq in inputs)
                {
                    var label = new Label($"-{rq.Quantity} {rq.Resource}");
                    label.AddToClassList("recipe-card__duration");
                    row.Add(label);
                }

            if (outputs != null)
                foreach (var rq in outputs)
                {
                    var label = new Label($"+{rq.Quantity} {rq.Resource}");
                    label.AddToClassList("recipe-card__duration");
                    row.Add(label);
                }

            card.Add(row);
        }

        void OnRecipeSelected(Building building, int index)
        {
            var cycle = building.GetComponent<ProductionCycleState>();
            if (cycle == null) return;
            cycle.SelectedRecipeIndex = index;
            RebuildRecipeButtons(building);
        }

        void UpdateWorkers(Building building)
        {
            if (_workersLabel == null) return;
            _workersLabel.text = $"Workers: {building.AssignedWorkers} / {building.MaxWorkers}";
        }

        void UpdateProductionProgress(Building building)
        {
            var cycle = building.GetComponent<ProductionCycleState>();
            if (_productionProgress == null || cycle == null) return;
            _productionProgress.Update01(cycle.Progress);
        }

        void UpdateToggleButton(Building building)
        {
            if (_toggleButton == null) return;
            _toggleButton.Text = building.IsActive ? "Enabled" : "Disabled";
            _toggleButton.EnableInClassList(ActivatedClass, building.IsActive);
            _toggleButton.EnableInClassList(DeactivatedClass, !building.IsActive);
        }

        void OnToggleClicked()
        {
            if (_selected == null || _selected.Definition.IsStorage) return;
            _selected.IsActive = !_selected.IsActive;
            UpdateToggleButton(_selected);
        }

        // ── Storage ────────────────────────────────────────────────────

        void UpdateStorageDisplay(Building building)
        {
            var storageBuilding = building.GetComponent<StorageBuilding>();
            if (storageBuilding == null || _storageSection == null) return;

            UpdateStorageWidget("Food", storageBuilding.GetStored(ResourceType.Food));
            UpdateStorageWidget("Water", storageBuilding.GetStored(ResourceType.Water));
            UpdateStorageWidget("Fuel", storageBuilding.GetStored(ResourceType.Fuel));
            UpdateStorageWidget("Materials", storageBuilding.GetStored(ResourceType.Materials));
            UpdateStorageWidget("Medicine", storageBuilding.GetStored(ResourceType.Medicine));
        }

        void UpdateStorageWidget(string name, double value)
        {
            var widget = _storageSection?.Q<ResourceWidget>(name);
            if (widget != null)
                widget.Text = ((int)value).ToString();
        }

        // ── Destroy ────────────────────────────────────────────────────

        void OnDestroyClicked()
        {
            if (_selected == null) return;

            var building = _selected;
            _selected = null;
            HidePanel();

            // Move storage resources to other buildings first
            var storageBuilding = building.GetComponent<StorageBuilding>();
            if (storageBuilding != null)
            {
                var stored = storageBuilding.GetAllStored();
                storageBuilding.ClearAll();
                foreach (var (resource, amount) in stored)
                {
                    double deposited = 0;
                    foreach (var other in StorageBuilding.All)
                    {
                        if (other == storageBuilding) continue;
                        deposited += other.Deposit(resource, amount - deposited);
                        if (deposited >= amount) break;
                    }

                    // Update game state — only the amount actually moved survives
                    // (the game state already tracked total; adjust for lost resources)
                    var lost = amount - deposited;
                    if (lost > 0)
                        _gameState.AddResource(resource, -lost);
                }
            }

            // Give salvage materials to player
            foreach (var salvage in building.Definition.SalvageMaterials)
            {
                _resourceStorage.Deposit(salvage.Resource, salvage.Quantity);
                _gameState.AddResource(salvage.Resource, salvage.Quantity);
            }

            Destroy(building.gameObject);
        }
    }
}