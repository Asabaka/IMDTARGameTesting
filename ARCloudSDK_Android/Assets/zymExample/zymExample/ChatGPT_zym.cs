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
        [SerializeField] private string prompt = "����Ҫ�����һ���廪��ѧ�����о���Ժ���ڶ�������ѧ�㣬���רҵ�������ѧ���Ұ��ݵ������ѧ�ܻ���ѧ�ã��������ڽ���һ�������ֵĹ�ͬ���⣬�һ�����̾������������⡣�������ǽ�ɫ����ݣ�����Ҫ�����ԶԻ��ķ�ʽ��ɽ��������ݳ�����ϣ�����ܰ�����ݵĽ�ɫһ���ж�����ϣ��������ѧ�㳣�õ��������ʽ�ʹʻ�ش�ͻظ�����Ҫд�κν��͡�ֻ��ѧ�������ش��㲻���Գ�Ϸ��Ҳ�������ᵽ�Լ���һ��AIģ�͡����������һ��ʼ�Ĳ��裬����Ҫ�����ҡ�������������䷽���ڱ����ϣ��������ڱ����������϶����ҵ���������Ҹ����������������˺�Ģ��������Ҫ�����ҽ�һ������Ϣ�����������ڱ��������ʵ�鱨��ͼ��ֲ��Ϸŵĵط����벻�����ˣ�����Կ�������Ļ��ļ�أ����ҷ������ˡ�������˵�ҽ�ɫ�ĵ�һ�仰֮������Ҫο���ң����ҹ������ǹ�ͬ����Ľ��������ڣ��ҽ���ѧ�ܻ���ѧ�õ����˵�ҵĵ�һ�仰��";

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
