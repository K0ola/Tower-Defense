using UnityEngine;

public class ProgressResetter : MonoBehaviour
{
    public void ResetGameProgress()
    {
        ProgressManager.ResetLevelProgress();
        Debug.Log("Progression réinitialisée !");
    }
}
