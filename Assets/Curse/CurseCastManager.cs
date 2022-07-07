using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CurseCastManager : MonoBehaviour
{
    public CurseInfo[] CurseMetadata;
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public void CastCurses()
    {
        var castedCurses = CurseRepo.Load().ToList();

        foreach (var curse in CurseMetadata)
        {
            var same = castedCurses.FirstOrDefault(c => c.Id == curse.Id);

            if (same == null)
                castedCurses.Add(curse);
            else switch (curse.StackMode)
                {
                    case CurseInfo.StackModes.Levels:
                        same.LevelsLeft += curse.LevelsLeft;
                        break;
                    case CurseInfo.StackModes.Power:
                        castedCurses.Add(curse);
                        break;
                }
        }

        CurseRepo.Save(castedCurses.ToArray());
    }
}
