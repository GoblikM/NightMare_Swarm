using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelectorUI : MonoBehaviour
{
    [SerializeField]
    private CharacterSO[] characterData;

    [SerializeField]
    private GameObject characterCardPrefab;

    [SerializeField]
    private GameObject charactersGrid; // The grid that will hold the character cards (parent)

    private void Start()
    {
        CreateCharacterCards();
    }

    private void CreateCharacterCards()
    {
        foreach (CharacterSO character in characterData)
        {
            GameObject characterCard = Instantiate(characterCardPrefab, charactersGrid.transform);
            characterCard.GetComponent<CharacterCardUI>().SetCharacterData(character);

            Button selectCharacterButton = characterCard.GetComponentInChildren<Button>();
            if (selectCharacterButton != null)
            {
                selectCharacterButton.onClick.AddListener(() =>
                {
                    CharacterSelector.instance.SelectCharacter(character);
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
                });
            }


        }
    }
}


