using UnityEngine;


public static class ProgressManager
{
    private const string LevelKey = "LevelCompleted";

    public static void SetLevelCompleted(int levelIndex)
    {
        int current = GetHighestLevel();
        if (levelIndex > current)
        {
            PlayerPrefs.SetInt(LevelKey, levelIndex);
            PlayerPrefs.Save();
        }
    }

    public static int GetHighestLevel()
    {
        return PlayerPrefs.GetInt(LevelKey, 0); 
    }

    public static bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex <= GetHighestLevel() + 1;
    }

    public static void ResetLevelProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

}
