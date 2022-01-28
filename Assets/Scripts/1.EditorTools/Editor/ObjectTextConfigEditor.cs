using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ProductMadness.EditorTools
{
    public class ObjectTextConfigEditor : EditorWindow
    {
        #region Private fields
        private static ObjectTextConfigEditor _windowInstance;

        private const string CONFIG_FILE = "Configuration/1.EditorTools/data.json";

        private Dictionary<string, GameObject> modifiedPrefabs = new Dictionary<string, GameObject>();
        private List<GameObject> itemsUpdated = new List<GameObject>();
        private List<ConfigurablePrefab> prefabsConfiguration;
        private string errorMsg;
        private string warnMsg;
        private string searchPath = "";
        private bool prefabsConfigurated = false;
        #endregion

        [MenuItem("Tools/Object Text Configurator")]
        public static void ShowWindow()
        {
            _windowInstance = GetWindow<ObjectTextConfigEditor>(true, "Object Text Configurator");
        }

        #region Unity Methods
        private void OnEnable()
        {
            ReadConfigFile();
        }

        private void OnGUI()
        {
            // First check if there is something wrong to give the proper feedback
            if (!string.IsNullOrEmpty(errorMsg))
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField(errorMsg, GetMsgStyle(Color.red));
                return;
            }

            if (!string.IsNullOrEmpty(warnMsg))
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField(warnMsg, GetMsgStyle(Color.yellow));
                EditorGUILayout.Separator();
            }
            
            // Draw the functions to search and apply changes 
            searchPath = EditorGUILayout.TextField(new GUIContent("Prefabs folder", "Write folder path or leave empty to search in the whole project"), searchPath);

            if (!TryDrawSearchAction()) return;

            if (itemsUpdated.Count > 0)
            {
                EditorGUILayout.LabelField($"We found {itemsUpdated.Count} items to be updated.");
            }

            if (!prefabsConfigurated) return;
            
            DrawFinalActions();
        }
        #endregion

        #region Private Methods

        private void ReadConfigFile()
        {
            string path = Path.Combine(Application.dataPath, CONFIG_FILE);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                try
                {
                    prefabsConfiguration = JsonMapper.ToObject<List<ConfigurablePrefab>>(json);
                }
                catch (Exception e)
                {
                    errorMsg = $"Error inside JSON File. Bad configuration. Error: {e.Message}";
                }
            }
            else
                errorMsg = "No configuration file found";
        }
        
        private GameObject ConfigurePrefab(string textPrefabPath, ConfigurablePrefab configuration)
        {
            // Update Text component with configuration found
            GameObject textPrefab = PrefabUtility.LoadPrefabContents(textPrefabPath);
            Text textComponent = textPrefab.GetComponent<Text>();
            textComponent.text = configuration.text;
            if (ColorUtility.TryParseHtmlString(configuration.color, out var color))
                textComponent.color = color;

            // The task doesn't say anything about the image, but the config file has an image field, so... Check child and try to update it too
            var childObj = textPrefab.transform.GetChild(0).gameObject;
            Image imageComponent = childObj.GetComponent<Image>();
            if (imageComponent != null)
            {
                Texture2D spriteTexture = LoadTexture(Path.Combine(Application.dataPath, configuration.image));
                if (spriteTexture != null)
                {
                    Sprite newImg = Sprite.Create(spriteTexture,
                        new Rect(0, 0, spriteTexture.width, spriteTexture.height),
                        new Vector2(0, 0), 100f);
                    imageComponent.sprite = newImg;
                    EditorUtility.SetDirty(childObj);
                }
            }
            
            // Send prefab to be applied (or not)
            EditorUtility.SetDirty(textPrefab);
            itemsUpdated.Add(textPrefab);
            return textPrefab;
        }

        private bool TryDrawSearchAction()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Search Prefabs"))
            {
                itemsUpdated.Clear();
                string[] tempPrefabs = SearchPrefabsPaths(searchPath);
                //List<GameObject> textPrefabs = GetPrefabsWithTextComponent(tempPrefabs);
                List<string> textPrefabs = GetPrefabPathsWithTextComponent(tempPrefabs);

                if (textPrefabs.Count == 0) return false;

                int itemsToUpdate = GetItemsCountToBeUpdated(textPrefabs.Count);

                for (int i = 0; i < itemsToUpdate; i++)
                {
                    var modifiedPrefab = ConfigurePrefab(textPrefabs[i], prefabsConfiguration[i]);
                    modifiedPrefabs.Add(textPrefabs[i], modifiedPrefab);
                }
                
                Debug.Log($"Items to updated: {itemsUpdated.Count}");
                prefabsConfigurated = true;
            }
            
            EditorGUILayout.EndHorizontal();
            return true;
        }

        private void DrawFinalActions()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Apply"))
            {
                foreach (KeyValuePair<string, GameObject> modifiedPrefab in modifiedPrefabs)
                {
                    PrefabUtility.SaveAsPrefabAsset(modifiedPrefab.Value, modifiedPrefab.Key);
                }
                itemsUpdated.Clear();
                modifiedPrefabs.Clear();
                prefabsConfigurated = false;
            }

            if (GUILayout.Button("Revert"))
            {
                modifiedPrefabs.Clear();
                itemsUpdated.Clear();
                prefabsConfigurated = false;
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private int GetItemsCountToBeUpdated(int textPrefabsCount)
        {
            int count = textPrefabsCount;
            if (textPrefabsCount > prefabsConfiguration.Count) // We found more prefabs than our configuration
            {
                warnMsg = " Warning! There are more prefabs than items configured.";
                count = prefabsConfiguration.Count;
            } 
            else if (textPrefabsCount < prefabsConfiguration.Count) // We configured more items than the prefabs found
            {
                warnMsg = " Warning! There are more items configured than prefabs found.";
            }

            return count;
        }

        private List<string> GetPrefabPathsWithTextComponent(string[] prefabsPaths)
        {
            List<string> textPrefabs = new List<string>();

            for (int i = 0; i < prefabsPaths.Length; i++)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabsPaths[i]);
                GameObject prefab = AssetDatabase.LoadMainAssetAtPath(prefabPath) as GameObject;

                if (prefab.GetComponent<Text>() != null)
                {
                    textPrefabs.Add(prefabPath);
                }
            }

            return textPrefabs;
        }

        /// <summary>
        /// Search only prefabs in the desired path
        /// </summary>
        /// <param name="_searchPath"></param>
        /// <returns>Array with the path of each prefab</returns>
        private string[] SearchPrefabsPaths(string _searchPath) => string.IsNullOrEmpty(_searchPath) ? AssetDatabase.FindAssets("t:Prefab") 
            : AssetDatabase.FindAssets("t:Prefab", new []{ Path.Combine("Assets", _searchPath) });
        
        private Texture2D LoadTexture(string filePath) {
            if (!File.Exists(filePath)) return null;
            var fileData = File.ReadAllBytes(filePath);
            var tex2D = new Texture2D(2, 2);
            return tex2D.LoadImage(fileData) ? tex2D : null;
        }

        private GUIStyle GetMsgStyle(Color color)
        {
            return new GUIStyle
            {
                normal =
                {
                    textColor = color
                },
                wordWrap = true,
                padding = new RectOffset(20, 0, 5, 0)
            };
        }
        #endregion
    }

    // I added this struct inside this file because it's just used here for an specific deserialization
    [Serializable]
    public struct ConfigurablePrefab
    {
        public string text;
        public string color;
        public string image;
    }
}
