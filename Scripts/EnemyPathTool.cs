using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EnemyPathTool : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Transform[] path = GetComponentsInChildren<Transform>();
        for (int i = 1; i < path.Length; i++)
        {
            if(i == path.Length - 1)
            {
                Gizmos.DrawLine(path[i].position, path[1].position);
            } else
            {
                Gizmos.DrawLine(path[i].position, path[i + 1].position);
            }
        }
    }

    public Transform[] GetLoop()
    {
        Transform[] path = GetComponentsInChildren<Transform>();
        Transform[] final = new Transform[path.Length - 1];
        for(int i = 1; i < path.Length; i++)
        {
            final[i - 1] = path[i];
        }
        return final;
    }
}
