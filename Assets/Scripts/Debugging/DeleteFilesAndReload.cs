using System.IO;
using Installers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Debugging
{
    public class DeleteFilesAndReload : MonoBehaviour
    {
        public void OnClick()
        {
            ValuesFromBootScene.DBValues.DeleteMyData();
            string path = Application.persistentDataPath;
            DirectoryInfo directory = new DirectoryInfo(path);

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }

            Debug.Log("Clear Data");

#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#elif UNITY_ANDROID
            Application.Quit();
#endif
        }
    }
}