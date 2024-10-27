using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Stats/NewStats")]
public class CharacterStatsSO : ScriptableObject
{
    private string previousName;
    public string characterName;
    public int health;
    public int attackPower;
    [Range(0, 4)] public int walkingSpeed;
    [Range(4, 8)] public int joggingSpeed;
    [Range(8, 10)] public int sprintingSpeed;


    public bool NameChanged()
    {
        if (previousName != characterName)
        {
            previousName = characterName;  // Update previousName to the new characterName
            return true;
        }
        return false;
    }
}
