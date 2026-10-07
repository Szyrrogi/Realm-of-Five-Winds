using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Obiekt w scenie Main: odpala dialogi i kończy misję. Bez aktywnej misji nic nie robi.</summary>
public class StoryDirector : MonoBehaviour
{
    public static StoryDirector Instance;

    public DialogueBox dialogue;
    [Tooltip("Scena z menu misji (domyślnie 0 = menu główne).")]
    public int storyMenuScene = 0;

    void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        if (!StoryManager.Active) yield break;
        while (!SaveService.Ready) yield return null;
        yield return PlayBeforeCurrentWave();
    }

    IEnumerator PlayBeforeCurrentWave()
    {
        int r = StatsManager.Round;
        if (StoryManager.DialogueShownRound >= r) yield break;
        StoryManager.DialogueShownRound = r;

        MissionWave w = StoryManager.Wave(r);
        if (w != null) yield return dialogue.Play(w.dialogueBefore);
        SaveService.SaveAll();
    }

    /// <summary>Wołane na końcu FightManager.StartBattle(), gdy plansza jest już wczytana po walce.</summary>
    public IEnumerator AfterBattle()
    {
        if (StoryManager.LastResult > 0)
        {
            MissionWave finished = StoryManager.Wave(StatsManager.Round - 1);
            if (finished != null) yield return dialogue.Play(finished.dialogueAfterWin);

            if (StoryManager.IsMissionWon)
            {
                yield return dialogue.Play(StoryManager.Mission.onVictory);
                StoryManager.MarkCompleted();
                EndMission();
                yield break;
            }
        }

        if (StatsManager.life <= 0)
        {
            yield return dialogue.Play(StoryManager.Mission.onDefeat);
            EndMission();
            yield break;
        }

        yield return PlayBeforeCurrentWave();
    }

    void EndMission()
    {
        SaveService.DeleteRun();
        StoryManager.Active = false;
        SceneManager.LoadScene(storyMenuScene);
    }
}
