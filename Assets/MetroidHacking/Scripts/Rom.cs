using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rom : MonoBehaviour
{
    public static RomMan smRom;
    public string superMetroidPath;
    private void OnEnable()
    {
        smRom = new RomMan(Application.dataPath+"/"+superMetroidPath);
    }
}
