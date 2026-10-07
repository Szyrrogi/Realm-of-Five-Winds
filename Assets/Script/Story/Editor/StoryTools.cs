#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>Pomocnik do tworzenia fal: zapisuje twoją aktualną planszę jako plik składu.</summary>
public static class StoryTools
{
    [MenuItem("Fabuła/Zapisz moją planszę jako falę")]
    static void SaveBoardAsWave()
    {
        string src = Application.dataPath + "/Save/Zapis.json";
        if (!File.Exists(src))
        {
            EditorUtility.DisplayDialog("Fabuła", "Brak Assets/Save/Zapis.json. Zagraj zwykłą grę w edytorze i ustaw planszę.", "OK");
            return;
        }
        Directory.CreateDirectory(Application.dataPath + "/Story/Waves");
        string path = EditorUtility.SaveFilePanelInProject("Zapisz falę", "Fala", "json", "Gdzie zapisać skład?", "Assets/Story/Waves");
        if (string.IsNullOrEmpty(path)) return;
        File.Copy(src, path, true);
        AssetDatabase.ImportAsset(path);
        Debug.Log("Zapisano falę: " + path);
    }
}
#endif
