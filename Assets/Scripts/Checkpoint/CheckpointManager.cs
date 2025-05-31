using UnityEngine;

[DisallowMultipleComponent]
public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private EndManager _endManager;
    
    private Checkpoint[] _checkpoints;
    private PlayerDataSaverController _dataSaver;

    void Start()
    {
        _dataSaver = PlayerController.GetInstance().DataSaver;
        _checkpoints = GetComponentsInChildren<Checkpoint>();
        InitCheckpoints();
    }

    public void ReloadGame()
    {
        foreach (var npcInitSaver in FindObjectsByType<NPCInitSaverController>(FindObjectsSortMode.None))
            npcInitSaver.ApplyInitData();
        
        _dataSaver.Load();
        PlayerController.Inputs.Unpause();
        PlayerController.GetInstance().OnRevive();
        _endManager.IncrementDeath();
    }

    private void InitCheckpoints()
    {
        foreach (var checkpoint in _checkpoints)
            checkpoint.OnActivate += () => SaveData(checkpoint);
    }

    private void SaveData(Checkpoint checkpoint)
    {
        _dataSaver.Save(checkpoint.Position);
    }
}