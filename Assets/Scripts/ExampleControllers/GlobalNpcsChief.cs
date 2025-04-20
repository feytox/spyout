using System;
using System.Collections.Generic;
using System.Linq;
using Classes.Character;
using Player;
using UnityEngine;
using Utils;

public class GlobalNpcsChief : MonoBehaviour
{
    public List<Manager> npcs;
    private List<Vector2> _spawnPoints = new();

    private Cooldown _nextCommandCooldown;
    private Cooldown _commandCallCooldown;
    private Action<NpcCommandData> _currentCommand = _ => { };
    private string _currentCommandName = "Nothing";
    private List<NpcCommandData> _data = new();

    void Awake()
    {
        npcs.AddRange(transform.GetComponentsInChildren<Manager>());
        _spawnPoints.AddRange(npcs.Select(npc => npc.Position));
        _nextCommandCooldown = new(10f);
        _commandCallCooldown = new(0.7f);
        _commandsList.AddRange(_commands.Keys);
    }

    void Start()
    {
        Player.Input.InteractAction.started += ctx => _nextCommandCooldown.SetExpired();
    }

    void Update()
    {
        if (npcs.Contains(Player.Movement.PlayerManager))
            npcs.Remove(Player.Movement.PlayerManager);

        if (_nextCommandCooldown.ResetIfExpired())
        {
            string newCommandName;
            while (true)
            {
                newCommandName = _commandsList[UnityEngine.Random.Range(0, _commandsList.Count)];
                if (newCommandName != _currentCommandName)
                    break;
            }
            _currentCommandName = newCommandName;
            DebugUI.SetExtraInfo("Current command", newCommandName);
            _data.Clear();
            _data.AddRange(
                npcs.Select(npc => new NpcCommandData(
                    this,
                    npc,
                    _spawnPoints[npcs.IndexOf(npc)],
                    null,
                    null,
                    null,
                    null
                ))
            );
            _currentCommand = _commands[newCommandName];
        }

        if (_commandCallCooldown.ResetIfExpired())
        {
            for (var i = 0; i < _data.Count; i++)
                _currentCommand(_data[i]);
        }
    }

    private List<string> _commandsList = new();

    private Dictionary<string, Action<NpcCommandData>> _commands = new()
    {
        {
            "Move directly to player",
            data => data.npc.Movement.MoveTo(Player.Movement.PlayerManager.Position)
        },
        {
            "Move away from player",
            data =>
            {
                data.npc.Movement.MoveInDirection(
                    data.npc.Position - Player.Movement.PlayerManager.Position
                );
            }
        },
        {
            "Find path to player",
            data =>
                data.npc.Movement.MoveAlongPath(
                    GridController
                        .FindPath(
                            data.npc.Movement.gameObject,
                            data.npc.Position,
                            Player.Movement.PlayerManager.Position
                        )
                        .Select(cellPos => cellPos.ToCenterPos())
                )
        },
        {
            "Find path to random npc",
            data =>
            {
                if (data.first == null)
                {
                    Manager target;
                    while (true)
                    {
                        target = data.chief.npcs[
                            UnityEngine.Random.Range(0, data.chief.npcs.Count)
                        ];
                        if (target != data.npc)
                            break;
                    }
                    data.first = target;
                }
                data.npc.Movement.MoveAlongPath(
                    GridController
                        .FindPath(
                            data.npc.Movement.gameObject,
                            data.npc.Position,
                            ((Manager)data.first).Position
                        )
                        .Select(cellPos => cellPos.ToCenterPos())
                );
            }
        },
        {
            "Find path to own spawn point",
            data =>
            {
                data.npc.Movement.MoveAlongPath(
                    GridController
                        .FindPath(data.npc.Movement.gameObject, data.npc.Position, data.spawnPoint)
                        .Select(cellPos => cellPos.ToCenterPos())
                );
            }
        },
    };

    private struct NpcCommandData
    {
        // any type of data for flexibility
        public GlobalNpcsChief chief;
        public Manager npc;
        public Vector2 spawnPoint;
        public object first;
        public object second;
        public object third;
        public object fourth;

        public NpcCommandData(
            GlobalNpcsChief chief,
            Manager npc,
            Vector2 spawnPoint,
            object first,
            object second,
            object third,
            object fourth
        )
        {
            this.chief = chief;
            this.npc = npc;
            this.spawnPoint = spawnPoint;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
        }
    }
}
