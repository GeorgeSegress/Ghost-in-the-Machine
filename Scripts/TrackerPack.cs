using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TrackerPack
{
    Transform tracked;
    Transform player;
    Sprite visual;

    public TrackerPack(Transform _player, Transform _tracked, Sprite mine)
    {
        tracked = _tracked;
        player = _player;
        visual = mine;
    }

    public Sprite GetSprite()
    {
        return visual;
    }

    public Vector3 DistFromPlayer()
    {
        return tracked.position - player.position;
    }

    public Transform GetTracked()
    {
        return tracked;
    }
}
