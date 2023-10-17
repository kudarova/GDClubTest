using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Updatable
{
    [System.NonSerialized] private static List<Updatable> updatable = new List<Updatable>();

    public static void Reset() => updatable.Clear();

    protected static void LinkToUpdate(Updatable obj)
    {
        if (!obj.isLinked)
        {
            obj.isLinked = true;
            updatable.Add(obj);
        }
    }
    protected static void UnlinkToUpdate(Updatable obj)
    {
        if (obj.isLinked)
        {
            obj.isLinked = false;
            updatable.Remove(obj);
        }
    } 

    public static void Tick()
    {
        for (int i = 0; i < updatable.Count; i++) updatable[i].Update();
    }

    [field: System.NonSerialized] private bool isLinked;

    protected Updatable()
    {
        LinkToUpdate(this);
    }

    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }
}