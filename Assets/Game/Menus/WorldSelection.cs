using System;
using System.IO;
using Game.Core;
using Game.Generation;
using Game.Serialization;
using Game.Tiles;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Game.Menus
{
    public class WorldSelection : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private GameObject mainMenu = default;
        [SerializeField] private GameObject newWorldMenu = default;
        [SerializeField] private GameObject loadWorldMenu = default;

        [Header("New World Menu")]
        [SerializeField] private TMP_InputField worldNameInput = default;
        [SerializeField] private Button createWorldButton = default;
        
        [Space]
        [SerializeField] private WorldGenSettings[] genSettings = default;
        [SerializeField] private Transform genPresetsHolder = default;
        [SerializeField] private GameObject genPreset = default;

        [Header("Load World Menu")]
        [SerializeField] private GameObject noWorldsText = default;
        [SerializeField] private Transform worldsHolder = default;
        [SerializeField] private GameObject loadableWorldPrefab = default;

        private WorldGenSettings _activeGenSettings = null;

        private void Awake()
        {
            BackToMainMenu();

            foreach (WorldGenSettings genSetting in genSettings)
            {
                GameObject preset = Instantiate(genPreset, genPresetsHolder);
                preset.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = genSetting.name;
                Button button = preset.transform.GetChild(2).GetComponent<Button>();
                
                // TODO: Change text to say that it is selected
                button.onClick.AddListener(delegate { _activeGenSettings = genSetting; });
            }

            var worlds = new string[0];
            try
            {
                worlds = Directory.GetDirectories($"{Application.persistentDataPath}/worlds");
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory($"{Application.persistentDataPath}/worlds");
            }

            if (worlds.Length == 0)
                noWorldsText.SetActive(true);

            foreach (string world in worlds)
            {
                var directory = world.Split('\\');
                string worldName = directory[directory.Length - 1];
                
                GameObject worldGameObject = Instantiate(loadableWorldPrefab, worldsHolder);
                worldGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = worldName;
                Button button = worldGameObject.transform.GetChild(2).GetComponent<Button>();
                button.onClick.AddListener(delegate { LoadWorld(worldName); });
                
                // TODO: Create option to delete a world
            }
        }

        private void LateUpdate()
        {
            createWorldButton.interactable = _activeGenSettings;
        }

        // TODO: Add checks to ensure that a world name has been entered
        // TODO: If world name already exists, either don't allow creation, or create an alias
        public void CreateWorld()
        {
            TileManager.CreateTileSet();
            
            WorldGen generator = new WorldGen();
            var tileMap = generator.GenerateWorld(_activeGenSettings);
            
            World.Data = new WorldData
            {
                Width = _activeGenSettings.worldWidth,
                Height = _activeGenSettings.worldHeight,
                TileMap = tileMap
            };
            
            Directory.CreateDirectory($"{Application.persistentDataPath}/worlds/{worldNameInput.text}");
            if (!SaveLoadManager.Save(World.Data, $"worlds/{worldNameInput.text}/world.bin"))
                Debug.LogError("Saving Error: Unable to save");

            SceneManager.LoadScene(1);
        }

        private static void LoadWorld(string world)
        {
            World.Data = SaveLoadManager.Load<WorldData>($"worlds/{world}/world.bin");
            TileManager.CreateTileSet();
            SceneManager.LoadScene(1);
        }

        public void BackToMainMenu()
        {
            newWorldMenu.SetActive(false);
            loadWorldMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        public void ShowNewWorldMenu()
        {
            newWorldMenu.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void ShowLoadWorldMenu()
        {
            loadWorldMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
    }
}