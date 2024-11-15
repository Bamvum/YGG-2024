using UnityEngine;
using UnityEngine.UI;

public class Utilities : MonoBehaviour
{
    public static string GenerateCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char[] code = new char[length];

        for (int i = 0; i < length; i++)
        {
            code[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
        }

        return new string(code);
    }
    public static void EnableAllButtons(GameObject gameObject)
    {
        // Example: Enable all buttons in the mainMenu GameObject
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
    public static void DisableAllButtons(GameObject gameObject)
    {
        // Example: Enable all buttons in the mainMenu GameObject
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }
}
