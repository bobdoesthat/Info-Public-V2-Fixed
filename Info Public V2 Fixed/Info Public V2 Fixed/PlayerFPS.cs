using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

public class PlayerFPS : MonoBehaviour
{
    public static List<VRRig> GetAllRemoteRigs()
    {
        List<VRRig> rigs = new List<VRRig>();
        foreach (VRRig rig in GameObject.FindObjectsOfType<VRRig>())
        {
            if (rig == null) continue;
            if (!rig.isOfflineVRRig)
                rigs.Add(rig);
        }
        return rigs;
    }

    public static VRRig GetRigByIndex(int index)
    {
        var rigs = GetAllRemoteRigs();
        if (rigs.Count == 0 || index < 0 || index >= rigs.Count)
            return null;
        return rigs[index];
    }

    public static int GetFPS(VRRig rig)
    {
        if (rig == null) return -1;
        var traverse = Traverse.Create(rig).Field("fps");
        if (traverse == null) return -1;
        object value = traverse.GetValue();
        if (value == null) return -1;
        return (int)value;
    }

    public int selectedIndex = 0;

    void Update()
    {
        VRRig selectedRig = GetRigByIndex(selectedIndex);
        if (selectedRig != null)
        {
            int fps = GetFPS(selectedRig);
            Debug.Log($"Player {selectedIndex} FPS = {fps}");
        }
    }
}
