using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance;
    public CharacterSO characterData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

        }
    }

    /**
    This method is used to get the character data from the CharacterSelector class
     */
    public static CharacterSO GetCharacterData()
    {
        return instance.characterData;
    }

    public void SelectCharacter(CharacterSO characterSO)
    {
        characterData = characterSO;
    }

    /**
     *  This method is used to destroy the singleton instance of the CharacterSelector class
     */
    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }


}
