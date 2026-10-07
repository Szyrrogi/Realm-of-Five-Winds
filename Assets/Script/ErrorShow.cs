using System.Diagnostics;
using System.IO;
using UnityEngine;

public class ErrorShow : MonoBehaviour
{

    public void OpenSaveFiles()
    {
        OpenInNotepad(SavePaths.Board);
        OpenInNotepad(SavePaths.Run);
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
