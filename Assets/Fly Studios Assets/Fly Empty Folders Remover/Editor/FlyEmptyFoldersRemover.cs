using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FlyStudiosAssets.Utilities.Editor
{
    public class FlyEmptyFoldersRemover : EditorWindow
    {
        protected Vector2 scrollPosition;
        protected Vector2 foldersTabScrollPosition;
        protected bool[] selectedFolders;
        protected List<string> emptyFoldersList = new List<string>(); // Lista pentru folderele goale
        protected int selectedTab = 0;
        protected string[] tabs = new string[] { "Empty Folders", "Settings" };
        protected string searchFolder = "Assets";

        [MenuItem("Tools/Fly Studios Assets/Fly Empty Folders Remover")]
        public static void Init()
        {
            var window = EditorWindow.GetWindow<FlyEmptyFoldersRemover>("Fly Empty Folders Remover");
            window.minSize = new Vector2(400f, 200f);
            window.Show();
        }

        protected virtual void PlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }

        protected virtual void OnEnable()
        {
            EditorApplication.playModeStateChanged += PlayModeStateChanged;
            this.searchFolder = EditorPrefs.GetString("EmptyFoldersRemover.searchFolder", "Assets");
        }

        protected virtual void OnDisable()
        {
            EditorApplication.playModeStateChanged -= PlayModeStateChanged;
            EditorPrefs.SetString("EmptyFoldersRemover.searchFolder", this.searchFolder);
        }

        protected virtual void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            this.selectedTab = GUILayout.Toolbar(this.selectedTab, this.tabs, EditorStyles.toolbarButton);
            EditorGUILayout.EndHorizontal();
            this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);
            EditorGUILayout.BeginVertical();
            switch (this.selectedTab)
            {
                case 0:
                    FoldersTabGUI();
                    break;
                case 1:
                    SettingsTabGUI();
                    break;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            GUILayout.Label("Made with ❤️ by Fly Studios Assets", EditorStyles.centeredGreyMiniLabel);
        }

        protected virtual void SettingsTabGUI()
        {
            this.searchFolder = EditorGUILayout.TextField("Search Folder", this.searchFolder);
        }

        protected virtual void FoldersTabGUI()
        {
            if (emptyFoldersList == null)
                emptyFoldersList = new List<string>();

            if (selectedFolders == null || selectedFolders.Length != emptyFoldersList.Count)
            {
                selectedFolders = new bool[emptyFoldersList.Count];
            }

            this.foldersTabScrollPosition = EditorGUILayout.BeginScrollView(this.foldersTabScrollPosition, GUILayout.Height(150)); // Height to display a certain number of folders
            EditorGUILayout.BeginVertical();
            if (emptyFoldersList.Count == 0)
            {
                GUILayout.Label("No Empty Folders Found", EditorStyles.centeredGreyMiniLabel);
            }

            for (int i = 0; i < emptyFoldersList.Count; i++)
            {
                string folderPath = emptyFoldersList[i];
                string folderName = Path.GetFileName(folderPath);
                EditorGUILayout.BeginHorizontal();
                selectedFolders[i] = EditorGUILayout.Toggle(selectedFolders[i], GUILayout.Width(15));
                GUILayout.Label(folderName, EditorStyles.wordWrappedLabel);

                EditorGUI.BeginDisabledGroup(Application.isPlaying);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

                // Draw separator line after each folder representation
                Rect lineRect = GUILayoutUtility.GetLastRect();
                lineRect.y += lineRect.height;
                lineRect.height = 1f;
                EditorGUI.DrawRect(lineRect, Color.white * 0.5f); // Adjust color and opacity as needed
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select All"))
            {
                for (int i = 0; i < selectedFolders.Length; i++)
                {
                    selectedFolders[i] = true;
                }
            }
            if (GUILayout.Button("Deselect All"))
            {
                for (int i = 0; i < selectedFolders.Length; i++)
                {
                    selectedFolders[i] = false;
                }
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Find Empty Folders"))
            {
                FindEmptyFolders();
            }

            if (GUILayout.Button("Remove Empty Folders"))
            {
                RemoveEmptyFolders();
            }

            GUILayout.FlexibleSpace();

            bool anySelected = false;
            if (selectedFolders != null)
            {
                for (int i = 0; i < this.selectedFolders.Length; i++)
                {
                    anySelected |= this.selectedFolders[i];
                }
            }
            EditorGUI.BeginDisabledGroup(!anySelected);
            EditorGUI.EndDisabledGroup();
        }


        private void FindEmptyFolders()
        {
            emptyFoldersList.Clear(); // Clear existing list

            // Get all subdirectories under the searchFolder
            var projectSubfolders = Directory.GetDirectories(searchFolder, "*", SearchOption.AllDirectories);

            // Filter out non-empty directories
            emptyFoldersList = projectSubfolders.Where(IsEmpty).ToList();

            // Repaint the window to update the list
            Repaint();
        }

        private void RemoveEmptyFolders()
        {
            for (int i = 0; i < selectedFolders.Length; i++)
            {
                if (selectedFolders[i])
                {
                    string folderPath = emptyFoldersList[i];
                    Debug.Log("Deleting folder: " + folderPath);

                    // Remove the folder using AssetDatabase.DeleteAsset
                    if (AssetDatabase.DeleteAsset(folderPath))
                    {
                        Debug.Log("Successfully deleted folder: " + folderPath);


                    }
                    else
                    {
                        Debug.LogWarning("Failed to delete folder: " + folderPath);
                    }
                }
            }

            FindEmptyFolders();

            // Refresh the asset database once we're done
            AssetDatabase.Refresh();
        }

        private static bool IsEmpty(string folderPath)
        {
            var allFiles = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            var nonMetaFiles = allFiles.Where(file => !file.EndsWith(".meta")).ToArray();

            return nonMetaFiles.Length == 0;
        }
    }
}
