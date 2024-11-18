using System;
using UnityEngine;
using UnityEngine.UI;
using static CardSOData;
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
    public static string GenerateUuid()
    {
        Guid uuid = Guid.NewGuid();
        return uuid.ToString();
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
    public static bool CheckIfLateBy10Minutes(DateTime dateTime)
    {
                // Convert scheduled time to local time zone
        DateTime localScheduledTime = dateTime.ToLocalTime();
                
                // Get current local time
        DateTime currentLocalTime = DateTime.Now;
                // Calculate the difference in minutes
        double minutesLate = (currentLocalTime - localScheduledTime).TotalMinutes;
        if (minutesLate > 10)
        {
            return true;
        }
        else
        {
                        return false;
        }
    }
    public static string FormatTimeRemaining(TimeSpan timeRemaining)
    {
        if (timeRemaining.TotalMinutes >= 1 && timeRemaining.TotalMinutes <= 10)
        {
                int minutesRemaining = Mathf.FloorToInt((float)timeRemaining.TotalMinutes);
                return $"{minutesRemaining} minute{(minutesRemaining > 1 ? "s" : "")}";
        }
                // If less than 1 minute remains, show it in seconds
        else if (timeRemaining.TotalSeconds < 60)
        {
                int secondsRemaining = Mathf.FloorToInt((float)timeRemaining.TotalSeconds);
                return $"{secondsRemaining} second{(secondsRemaining > 1 ? "s" : "")}";
        }
        return "Fetching...";
    }
    public static Cards cardtoCards(CardSO selectedCard)
    {
        Cards newCard = new Cards();

        newCard.item = selectedCard;
        newCard.quantity = 1;

        return newCard;
    }
    public static string FormatNumber(int number)
    {
        return string.Format("{0:N0}", number);
    }
    public static string FormatSolana(double number)
    {
        return number.ToString("F9");
    }
}
