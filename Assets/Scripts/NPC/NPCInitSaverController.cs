using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NPCController))]
public class NPCInitSaverController : MonoBehaviour
{
    private NPCController _npc;
    private Vector3 _position;
    
    public void ApplyInitData()
    {
        _npc.transform.position = _position;
    }
    
    void Start()
    {
        _npc = GetComponent<NPCController>();
        SaveInitData();
    }
    
    private void SaveInitData()
    {
        _position = _npc.Position;
    }
}