using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using System.IO;
using Diplomatrix;

public class ChatScript : MonoBehaviour
{

    List<Diplomatrix.Message> ChatHistory = new List<Diplomatrix.Message>();
    TMP_InputField textBox;
    Button sendButton;

    [SerializeField]
    TMP_Text outputField;

    private OpenAIAPI api;
    private List<ChatMessage> messages = new List<ChatMessage>();

    [SerializeField]
    string initialPrompt = "You are an enemy commander, in a grid based strategy war game. Talk short and directly.";

    [SerializeField]
    int showMessagesUpTo = 4;

    void Start()
    {
        getObjects();

        createApi();

        setInitialPrompt();
    }

    private void setInitialPrompt(){
        messages.Add(new ChatMessage(ChatMessageRole.System, initialPrompt));
    }

    private void getObjects(){
        Transform tbTr = transform.Find("TextBox");
        Transform bTr = transform.Find("Button");

        if (tbTr != null)
        {
            textBox = tbTr.gameObject.GetComponent<TMP_InputField>();
        }
        else
        {
            Debug.Log("Text Box object not found.");
        }

        if (bTr != null)
        {
            sendButton = bTr.gameObject.GetComponent<Button>();
        }
        else
        {
            Debug.Log("Button object not found.");
        }
    }

    private void createApi(){
        // read the api key:
        string key = "";
        string apiPath = Application.dataPath + "/../Private/OpenAI_API_Key.txt";
        if (File.Exists(apiPath))
        {
            string content = File.ReadAllText(apiPath);
            key = content;
        }
        else
        {
            Debug.LogError("File not found at: " + apiPath);
        }

        api =  new OpenAIAPI(key);
    }

    public void onEndEdit(){
        if (!Input.GetKeyDown(KeyCode.Return) || !sendButton.IsInteractable()) 
        return;

        handleInputText(textBox.text);

        textBox.text = "";

        textBox.ActivateInputField();
    }

    public void onButtonClick(){
        Debug.Log(textBox.text);

        handleInputText(textBox.text);

        textBox.text = "";

        //inputField.ActivateInputField();
    }

    private void handleInputText(string input){
        Diplomatrix.Message message = new Diplomatrix.Message("You", input);
        ChatHistory.Add(message);
        getResponse(message.Content);
        fillOutputField(showMessagesUpTo);
    }

    private void printChatHistory(){
        foreach (var message in ChatHistory){
            Debug.Log(message);
        }
    }
    private void fillOutputField(int lastMessagesAmount) {
        string[] messages = new string[lastMessagesAmount];

        outputField.text = "";
        int j = 0;

        for (int i = ChatHistory.Count - 1; i >= 0 && j < lastMessagesAmount; --i, j++) {
            messages[j] = ChatHistory[i].ToString();
        }

        if (j > 0) {
            for (int i = j - 1; i >= 0; --i) {
                outputField.text += messages[i] + "\n";
                Debug.Log(messages[i]);
            }
        }
    }


    private async void getResponse(string lastPrompt){
        sendButton.interactable = false;

        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = lastPrompt;

        messages.Add(userMessage);

        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo0125,
            Temperature = 0.9,
            MaxTokens = 50,
            Messages = messages
        });

        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;

        messages.Add(responseMessage);
        ChatHistory.Add(new Message("Enemy", responseMessage.Content));

        sendButton.interactable = true;
        fillOutputField(showMessagesUpTo);
    }
}
