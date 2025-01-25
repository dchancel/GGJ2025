using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SO_TutorialData", menuName = "Scriptable Objects/SO_TutorialData")]
public class SO_TutorialData : ScriptableObject
{
    public List<TutData> d = new List<TutData>();
}
