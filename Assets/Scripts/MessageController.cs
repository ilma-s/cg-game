using UnityEngine;
using TMPro;

public class MessageController : MonoBehaviour
{
    public TextMeshProUGUI successText;
    public TextMeshProUGUI failureText;

    public float messageDuration = 2.0f; // Duration the message stays on screen

    private void Start()
    {
        // Ensure messages are hidden at the start
        if (successText != null)
        {
            successText.gameObject.SetActive(false);
            Debug.Log("MessageController: SuccessText disabled at start.");
        }
        else
        {
            Debug.LogError("MessageController: SuccessText is not assigned!");
        }

        if (failureText != null)
        {
            failureText.gameObject.SetActive(false);
            Debug.Log("MessageController: FailureText disabled at start.");
        }
        else
        {
            Debug.LogError("MessageController: FailureText is not assigned!");
        }
    }

    public void ShowSuccessMessage()
    {
        if (successText != null)
        {
            successText.gameObject.SetActive(true);
            Invoke(nameof(HideSuccessMessage), messageDuration);
            Debug.Log("MessageController: Success message shown.");
        }
    }

    public void ShowFailureMessage()
    {
        if (failureText != null)
        {
            failureText.gameObject.SetActive(true);
            Invoke(nameof(HideFailureMessage), messageDuration);
            Debug.Log("MessageController: Failure message shown.");
        }
    }

    private void HideSuccessMessage()
    {
        if (successText != null)
        {
            successText.gameObject.SetActive(false);
            Debug.Log("MessageController: Success message hidden.");
        }
    }

    private void HideFailureMessage()
    {
        if (failureText != null)
        {
            failureText.gameObject.SetActive(false);
            Debug.Log("MessageController: Failure message hidden.");
        }
    }
}
