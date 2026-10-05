using System.Diagnostics;
using System.IO;
using UnityEngine;

public class ErrorShow : MonoBehaviour
{

    public void OpenSaveFiles()
    {
        string savePath1 = Application.dataPath + "/Save/Zapis.json";
        string savePath2 = Application.dataPath + "/Save/Save2.json";

        if (RankedManager.Ranked)
        {
            savePath1 = Application.dataPath + "/Save/ZapisR.json";
            savePath2 = Application.dataPath + "/Save/Save2R.json";
        }

        OpenInNotepad(savePath1);
        OpenInNotepad(savePath2);
    }

    void OpenInNotepad(string path)
    {
        if (File.Exists(path))
        {
            Process.Start("notepad.exe", path);
        }
        else
        {
            UnityEngine.Debug.LogWarning("Plik nie istnieje: " + path);
        }
    }
}
