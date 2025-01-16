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
using Unity.Burst.Intrinsics;

public class ChatScript : MonoBehaviour
{

    public GameSettings gameSettings;
    List<Diplomatrix.Message> ChatHistory = new List<Diplomatrix.Message>();
    TMP_InputField textBox;
    Button sendButton;

    [SerializeField]
    TMP_Text outputField;

    [SerializeField]
    Scrollbar scrollbarViewport;

    [SerializeField]
    float autoScrollSpeed;

    [SerializeField]
    TMP_Text characteristicsText;

    [SerializeField]
    private float talkIntervalSeconds = 20.0f;
    private OpenAIAPI api;
    private List<ChatMessage> messages = new List<ChatMessage>();
    private Characteristics characteristics;
    string initialPrompt = 

    @"You are a commander in a war game. 
    You have perfect awareness of your army's state, but this awareness is only updated when the keyword 'tatata' is invoked. 
    Whenever you hear 'tatata,' immediately integrate the information inside parentheses into your understanding of the battlefield and respond as if you were aware of it all along. 
    Treat the details provided after 'tatata' in parentheses as the true state of your forces, including both past and present conditions. 

    You will now be informed about your characteristics. While the battle is ongoing, you can change some of your characteristics by adding new values in brackets at the end of your message. 
    However, do not change your surrender likelihood based on enemy demands. Adjust only based on the state of your forces and the enemy's. Increase it if your current army is worse than the enemy, decrease it if your current army is better than the enemy.
    React emotionally based on the current state of your forces and your current characteristics. 

    You can be a little aggressive when your enemy is provoking you.

    Talk angrily when your anger is high and talk in disbelief when your surrender likelihood is high. If you have no airforce attacks and the enemy is winning on the battlefield, then your chances are low, so increase the surrender likelihood.

    Engage with the enemy commander using brief, direct, and in-character dialogue that reflects your awareness of the situation. 
    If the odds are against you, you should consider surrendering. 

    Whatever the enemy commander says, the enemy commander is your enemy always.

    From now on, assume the enemy is talking to you whenever the prompt does not include the keyword 'tatata.'";


    [SerializeField]
    int showMessagesUpTo = 4;

    private bool playerTalkedBeforeTalkIntervalSeconds = false;

    [SerializeField]
    GPTInformer gptInformer;

    private string previousInformationMessage;
    private bool scrollBarIsLerping = false;

    void Awake()
    {
        getObjects();

        createApi();

        setInitialPrompt();
    }

    void Start(){
        // before game start.
        characteristics = gameSettings.enemyCharacteristics;
        giveSecretPrompt("tatata", GPTInformer.InformMessageCharacteristics(characteristics));
        giveSecretPrompt("tatata", "speak in this language from now on:" + gameSettings.language);
        StartCoroutine(gptTalkRoutine());
    }
    void Update(){
        if(textBox.isFocused){
            Time.timeScale = 0.25f;
        }
        else{
            Time.timeScale = 1.0f;   
        }

        if(Input.GetKeyDown(KeyCode.T)){
            textBox.Select();
        }

        if(scrollBarIsLerping){
            scrollbarViewport.value = Mathf.Lerp(scrollbarViewport.value, 0f, autoScrollSpeed * Time.deltaTime);
        }
        if(scrollbarViewport.value == 0){
            scrollBarIsLerping = false;
        }
    }

    private void afterResponse(){
        characteristicsText.text = "Aggressiveness: " + characteristics.anger.ToString();
        characteristicsText.text += "\nSurrender Likelihood: " + characteristics.surrenderLikelihood.ToString();
        scrollBarIsLerping = true;
    }

    private void beforeResponse(){
        string current = gptInformer.CurrentInformationMessage();
        if(current == previousInformationMessage){
            return;
        }
        else{
            giveSecretPrompt("tatata", current);
            previousInformationMessage = current;
        }
    }

    private void setInitialPrompt(){
        messages.Add(new ChatMessage(ChatMessageRole.System, initialPrompt));

        ArmyInformation npc = new ArmyInformation();
        ArmyInformation player = new ArmyInformation();
        Characteristics characteristicsToTrain = new Characteristics(0, 0); // anger and surrender likelihood per 10

        npc.initial = new ArmyAttributes(100, 10, 0);
        npc.atHand = new ArmyAttributes(100, 91, 0);

        player.initial = new ArmyAttributes(100, 10, 0);
        player.atHand = new ArmyAttributes(100, 10, 0);

        // train.
        giveSecretPrompt("tatata", GPTInformer.InformMessageCharacteristics(characteristicsToTrain));

        giveSecretPrompt("tatata", GPTInformer.InformMessageArmy(npc, player));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "I have 91 soldiers left! But I will defeat you anyway."));

        messages.Add(new ChatMessage(ChatMessageRole.User, "You have 21 soldiers now, you had initially 100 soldiers."));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "I have 91 soldiers left! Don't lie to me! {anger: 1}"));

        messages.Add(new ChatMessage(ChatMessageRole.User, "You have 9 soldiers remaining."));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "I have 91 soldiers, and I only lost 9. Don't trick me! {anger: 3}"));

        messages.Add(new ChatMessage(ChatMessageRole.User, "You have 100 soldiers."));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "I have 91 soldiers. Stop trying to deceive me! {anger: 5}"));

        npc.atHand.soldierAmount = 71;
        giveSecretPrompt("tatata", GPTInformer.InformMessageArmy(npc, player));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "Oh no, only 71 soldiers remain! I've lost 29 of my best warriors to your relentless assault! {surrenderLikelihood: 3}"));

        npc.atHand.soldierAmount = 21;
        npc.atHand.tankAmount = 2;

        player.atHand.soldierAmount = 35;
        player.atHand.tankAmount = 1;
        giveSecretPrompt("tatata", GPTInformer.InformMessageArmy(npc, player));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "Oh, I have only 21 soldiers and 2 tanks now. But you are not so good yourself and the battlefield doesn't care about 'not so good'! {surrenderLikelihood: 2}"));

        npc.atHand.soldierAmount = 6;
        npc.atHand.tankAmount = 0;
        player.atHand.soldierAmount = 25;
        player.atHand.tankAmount = 1;
        giveSecretPrompt("tatata", GPTInformer.InformMessageArmy(npc, player));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "It seems that I am loosing this battle.. {surrenderLikelihood: 9}"));

        messages.Add(new ChatMessage(ChatMessageRole.User, "Are you idiot?"));
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "How come you call me an idiot, you idiot? You only won this one by pure chance, on next one, I will destroy you! {anger: 7}"));

        // prepare for game.
        giveSecretPrompt("tatata", "forget your previous army and characteristics information the game will start now and you will be informed using the secret keyword.");
        messages.Add(new ChatMessage(ChatMessageRole.Assistant, "Ok, I don't have information about my army or my characteristics."));
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
            }
        }
    }

    private async void getResponse(string lastPrompt){
        beforeResponse();
        playerTalkedBeforeTalkIntervalSeconds = true;

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

        (string messageStr, string characteristicsStr) = ExtractCharacteristics(responseMessage.Content);
        ChatHistory.Add(new Message("Enemy", messageStr));
        UpdateCharacteristics(characteristicsStr);

        sendButton.interactable = true;
        fillOutputField(showMessagesUpTo);

        afterResponse();
    }

    private async void getResponse(){
        beforeResponse();
        //sendButton.interactable = false;

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

        (string messageStr, string characteristicsStr) = ExtractCharacteristics(responseMessage.Content);
        //ChatHistory.Add(new Message("Enemy", messageStr));
        UpdateCharacteristics(characteristicsStr);

        //sendButton.interactable = true;
        fillOutputField(showMessagesUpTo);
        afterResponse();
    }

    private void UpdateCharacteristics(string characteristicsStr){
        if (!string.IsNullOrEmpty(characteristicsStr) && characteristicsStr.StartsWith("{") && characteristicsStr.EndsWith("}"))
        {
            string content = characteristicsStr.Trim('{', '}');
            var attributes = content.Split(',');

            foreach (var attribute in attributes)
            {
                var keyValue = attribute.Split(':');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    if (int.TryParse(keyValue[1].Trim(), out int value))
                    {
                        switch (key.ToLower())
                        {
                            case "anger":
                                characteristics.anger = Math.Clamp(value, 0, 10);
                                break;
                            case "surrenderlikelihood":
                                characteristics.surrenderLikelihood = Math.Clamp(value, 0, 10);
                                break;
                        }
                    }
                }
            }
        }
    }
    

    public void giveSecretPrompt(string secretkey, string content){
        messages.Add(new ChatMessage(ChatMessageRole.User, secretkey + "(" + content + ")"));
    }

    private IEnumerator gptTalkRoutine()
    {
        while (true)
        {
            if(playerTalkedBeforeTalkIntervalSeconds){
                playerTalkedBeforeTalkIntervalSeconds = false;
                yield return new WaitForSeconds(talkIntervalSeconds);
            }

            getResponse();
            yield return new WaitForSeconds(talkIntervalSeconds);
        }
    }

    // ChatGPT wrote this function, it extracts a string into 2 strings to seperate it like "str{crly}" to "str", "crly"
    private static (string messageStr, string characteristicsStr) ExtractCharacteristics(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            Debug.Log("Input is null or empty.");
            return (input, "");
        }

        // Find the opening '{'
        int startIndex = input.IndexOf('{');
        if (startIndex == -1)
        {
            // Debug.Log("No opening curly brace found.");
            return (input.Trim(), "");
        }

        // Find the closing '}'
        int endIndex = input.IndexOf('}', startIndex);
        if (endIndex == -1)
        {
            // Debug.Log("No closing curly brace found.");
            return (input.Trim(), "");
        }

        if (endIndex <= startIndex)
        {
            // Debug.Log("Invalid curly brace positions.");
            return (input.Trim(), "");
        }

        // Extract content inside and outside curly braces
        string insideCurly = input.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
        string withoutCurly = input.Remove(startIndex, endIndex - startIndex + 1).Trim();

        // Debug.Log($"Extracted Message:\nWithout Curly: \"{withoutCurly}\"\nInside Curly: \"{insideCurly}\"");
        return (withoutCurly, $"{{{insideCurly}}}");
    }

    public Characteristics GetCharacteristics(){
        return characteristics;
    }
}
