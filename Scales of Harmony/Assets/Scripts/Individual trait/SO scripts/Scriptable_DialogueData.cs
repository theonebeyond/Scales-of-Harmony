using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueData : ScriptableObject
{
    public string DialogueName;
    public string DialogueSpeaker;
    public Sprite Avatar;
    public Boolean TrueTopFalseBottom;
    public List<string> Lines;

}