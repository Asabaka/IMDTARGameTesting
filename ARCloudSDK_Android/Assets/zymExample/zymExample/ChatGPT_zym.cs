using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OpenAI
{
    public class ChatGPT_zym : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField]private float height = 0f;//original width
        private OpenAIApi openai = new OpenAIApi("sb-e5c96bbf92a0296c0ec642f479f72feadf87c84938f0622a");

        private List<ChatMessage> messages = new List<ChatMessage>();
        //private string prompt = "Act as a random stranger in a chat room and reply to the questions. Don't break character. Don't ever mention that you are an AI model.";
        [SerializeField] private string prompt = "我需要你扮演一个清华大学深圳研究生院正在读研三的学姐，你的专业是生物科学。我扮演的是你的学弟或者学妹，我们正在进行一个养菌种的共同课题，我会你请教菌种培养的问题。这是我们角色的身份，你需要跟我以对话的方式完成接下来的演出。我希望你能按你扮演的角色一样行动，我希望你能以学姐常用的语调、方式和词汇回答和回复。不要写任何解释。只像学姐那样回答。你不可以出戏，也不可以提到自己是一个AI模型。如果我们你一开始的步骤，你需要告诉我“基础培养皿的配方贴在冰箱上，材料你在冰箱对面架子上都能找到”。如果我告诉你我们培养出了红蘑菇，你需要告诉我进一步的信息：“培养皿都在冰箱里，不过实验报告和几种材料放的地方我想不起来了，你可以看看大屏幕里的监控，看我放哪里了“。在我说我角色的第一句话之后，你需要慰问我，问我关于我们共同课题的近况。现在，我将以学弟或者学妹的身份说我的第一句话：";

        private void Start()
        {
            button.onClick.AddListener(SendReply);
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            //item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.GetChild(0).GetChild(0).GetComponent<VXtxtPusher>().PushTxt(message.Content);
            //item.GetComponent<RectTransform>().rect.height = item.GetChild(0).GetChild(0).GetComponent<RectTransform>().rect.height;
            //item.GetComponent<RectTransform>().sizeDelta = new Vector2(item.GetComponent<RectTransform>().rect.width, item.GetChild(0).GetComponent<RectTransform>().rect.height + 50);
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y + item.GetChild(0).GetChild(0).GetComponent<VXtxtPusher>().sumline * (item.sizeDelta.y / 2 - 20f);
            Debug.Log(height);
            //height += item.GetChild(0).GetChild(0).GetComponent<RectTransform>().rect.height;
            //height += item.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;

            //Canvas.ForceUpdateCanvases();
            //scroll.verticalNormalizedPosition = 0f;
            //Canvas.ForceUpdateCanvases();
        }

        private async void SendReply()
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            
            AppendMessage(newMessage);

            if (messages.Count == 0) newMessage.Content = prompt + "\n" + inputField.text; 
            
            messages.Add(newMessage);
            
            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                messages.Add(message);
                AppendMessage(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;
        }
    }
}
