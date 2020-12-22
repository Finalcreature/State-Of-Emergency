using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyPrefs : MonoBehaviour
{
    public enum Difficulies {Easy,Normal,Hard,Expert};
    public static Difficulies difficulty = Difficulies.Easy;

    public static void SetDifficulty(int chosendifficulty)
    {
        if (chosendifficulty == 0)
        {
            difficulty = Difficulies.Easy;
        }
        else if (chosendifficulty == 1)
        {
            difficulty = Difficulies.Normal;
        }
        else if (chosendifficulty == 2)
        {
            difficulty = Difficulies.Hard;
        }
        else
        {
            difficulty = Difficulies.Expert;
        }
        FindObjectOfType<OptionsHandler>().SetDifficultyText(chosendifficulty);
    }

    public static Difficulies GetDifficuly()
    {
        return difficulty;
    }
}
