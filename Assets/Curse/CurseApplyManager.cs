using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurseApplyManager : MonoBehaviour
{
    public CurseBase[] Curses = new CurseBase[0];
    public bool AutoCurses = true;

    private Player _player;
    private bool _applied = false;
    private List<CurseInfo> _castedCurses = new List<CurseInfo>();

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _castedCurses = CurseRepo.Load().ToList();

        if (AutoCurses)
            Curses = GetComponentsInChildren<CurseBase>();

        var manager = FindObjectOfType<GameManager>();

        if (manager != null && !manager.isInteractableScene)
            ApplyCastedCurses(Curses);
    }

    private void ApplyCastedCurses(CurseBase[] handlers)
    {
        foreach (var curseInfo in _castedCurses)
        {
            var curseHandler = Curses.FirstOrDefault(c => c.Id == curseInfo.Id);
            if (curseHandler != null)
            {
                curseHandler.Apply(_player);
                curseInfo.LevelsLeft--;
                _applied = true;
            }
            else
                Debug.Log($"WARNING: can't find curse handler with id={curseInfo.Id}");
        }
    }

    public void OnLevelCompleted()
    {
        if (_applied)
        {
            _castedCurses.RemoveAll(c => c.LevelsLeft == 0);

            CurseRepo.Save(_castedCurses);
        }
    }
}
