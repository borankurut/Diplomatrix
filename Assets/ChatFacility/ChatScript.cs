using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class ChatScript : MonoBehaviour
{

    List<DiplomatrixMessage> ChatHistory = new List<DiplomatrixMessage>();
    GameObject TextBox;
    GameObject Button;

    void Start()
    {
        Transform tbTr = transform.Find("TextBox");
        Transform bTr = transform.Find("Button");

        if (tbTr != null)
        {
            TextBox = tbTr.gameObject;
        }
        else
        {
            Debug.Log("Text Box object not found.");
        }

        if (bTr != null)
        {
            Button = bTr.gameObject;
        }
        else
        {
            Debug.Log("Button object not found.");
        }

    }

    void Update()
    {
        
    }

    public void onEndEdit(){
        if (!Input.GetKeyDown(KeyCode.Return)) 
        return;

        Debug.Log("editted.");
        TMP_InputField inputField = TextBox.GetComponent<TMP_InputField>();
        Debug.Log(inputField.text);

        handleInputText(inputField.text);

        inputField.text = "";

        inputField.ActivateInputField();
    }

    public void onButtonClick(){
        Debug.Log("clicked.");
        TMP_InputField inputField = TextBox.GetComponent<TMP_InputField>();
        Debug.Log(inputField.text);

        handleInputText(inputField.text);

        inputField.text = "";

        //inputField.ActivateInputField();
    }

    private void handleInputText(string input){
        DiplomatrixMessage message = new DiplomatrixMessage("You", input);
        ChatHistory.Add(message);
        printChatHistory();
    }

    private void printChatHistory(){
        foreach (var message in ChatHistory){
            Debug.Log(message);
        }
    }
}
