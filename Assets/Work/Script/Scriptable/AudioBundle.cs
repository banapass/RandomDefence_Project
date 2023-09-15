using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New AudioBundle", menuName = "Scriptable/AuduiBundle")]
public class AudioBundle : ScriptableObject
{
    public string audioName;
    public SoundType audioType;
    public AudioClip clip;
}
