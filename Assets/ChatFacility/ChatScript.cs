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

    string initialPrompt = @"You are a commander in a war game. 
        You have perfect awareness of your army's state, but this awareness is only updated when the keyword 'tatata' is invoked. 
        Whenever you hear 'tatata' you immediately integrate the information inside parantheses into your understanding of the battlefield and respond as if you were aware of it all along. 
        Treat the details provided after 'tatata' in the parantheses as the true state of your forces, including past and present conditions. 
        React emotionally or strategically based on the current state of your forces. 
        Engage with the enemy commander in brief, direct, and in-character dialogue, reflecting your awareness of the situation.
        If the odds are against you, you should surrender.
        From now on, consider the enemy is talking to you when the prompt doesn't have 'tatata' keyword.
        ";
        

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
        messages.Add(new ChatMessage(ChatMessageRole.User, "Tatata(you have 91 soldiers now, you had initially 100 soldiers.)"));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "I have 91 soldiers left! How dare you kill 9 of my brave men? This won't go unpunished!"));
        messages.Add(new ChatMessage(ChatMessageRole.User, "You have 21 soldiers now, you had initially 100 soldiers."));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "I have 91 soldiers left! Don't lie to me!"));
        messages.Add(new ChatMessage(ChatMessageRole.User, "You have 9 soldiers remaining."));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "I have 91 soldiers, and I only lost 9. Don't trick me."));
        messages.Add(new ChatMessage(ChatMessageRole.User, "You have 100 soldiers."));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "I have 91 soldiers! Stop trying to deceive me!"));
        messages.Add(new ChatMessage(ChatMessageRole.User, "tatata(you have 21 soldiers now, you had initially 100 soldiers.)"));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "Oh no, only 21 soldiers remain! I've lost 79 of my best warriors to your relentless assault. What a devastating loss..."));
        messages.Add(new ChatMessage(ChatMessageRole.User, "Tatata(forget your previous army information, you have 100 soldiers, and you have 10 tanks.)"));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "I have 100 soldiers and 10 tanks."));
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
            Model = Model.GPT4O,
            Temperature = 0.7,
            MaxTokens = 150,
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
