using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Example : MonoBehaviour
{
    static List<Type> s_VolumeComponents;
    static Example()
    {
        s_VolumeComponents = TypeCache.GetTypesDerivedFrom<AttackDash>().ToList();
    }

    private void Awake()
    {
        foreach (var type in s_VolumeComponents)
            Debug.Log(type);
    }

}
