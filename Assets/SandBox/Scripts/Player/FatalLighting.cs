using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class FatalLighting : MonoBehaviour
{
    public float MaxDistance;
    public float DamageAmount;
    public KindaLine VisualEffect;
    public int MaxRecursionDepth = 3;
    public float Cooldown = 0.2f;

    private List<GameObject> _buffer = new List<GameObject>();
    private NDamageReciever[] _toProcess;

    public void Emit(Transform from)
    {
        if (!ActionEx.CheckCooldown((Action<Transform>)Emit, Cooldown)) return;

        _toProcess = FindObjectsOfType<AchievementHandler>()
       .Select(o => o.GetComponentInChildren<NDamageReciever>())
       .ToArray();

        EmitRecursive(from, MaxRecursionDepth);
        _buffer.Clear();
    }

    // TODO: OPTIMIZE THAT PIECE OF SHIT
    private void EmitRecursive(Transform from, int depth)
    {
        if (depth <= 0 || _buffer.Contains(from.gameObject)) return;

        _buffer.Add(from.gameObject);

        var objects = _toProcess
        .Where(o => Vector2.Distance(o.transform.position, from.transform.position) <= MaxDistance)
        .Where(o => !_buffer.Contains(o.gameObject));


        foreach (var o in objects)
        {
            _buffer.Add(o.gameObject);

            var visual = Instantiate(VisualEffect, from.transform.position, Quaternion.identity);
            visual.From = from;
            visual.To = o.transform;
            o.SendMessage("Damage", new[] { transform.position.x, DamageAmount });
        }

        foreach (var o in objects)
            EmitRecursive(o.transform, depth - 1);
    }
}
