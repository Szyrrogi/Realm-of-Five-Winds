using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Menu misji. Przyciski wołają StartMission(numer) i ContinueMission().</summary>
public class StoryMenu : MonoBehaviour
{
    public List<MissionData> missions = new List<MissionData>();
    [Tooltip("Ta sama scena, którą ładuje zwykła gra (PlayerManager.ReadInput ładuje 4).")]
    public int gameScene = 4;
    public GameObject continueButton;

    void Update()
    {
        if (continueButton != null)
            continueButton.SetActive(FindSaved() != null);
    }

    /// <summary>Misja n jest odblokowana, gdy poprzednia została ukończona.</summary>
    public bool IsUnlocked(int index) => index == 0 || StoryManager.IsCompleted(missions[index - 1]);

    public void StartMission(int index)
    {
        if (index < 0 || index >= missions.Count || !IsUnlocked(index)) return;
        StoryManager.Begin(missions[index], false);
        SceneManager.LoadScene(gameScene);
    }

    public void ContinueMission()
    {
        MissionData m = FindSaved();
        if (m == null) return;
        StoryManager.Begin(m, true);
        SceneManager.LoadScene(gameScene);
    }

    MissionData FindSaved()
    {
        string id = StoryManager.SavedMissionId();
        return id == null ? null : missions.Find(m => m != null && m.missionId == id);
    }
}
