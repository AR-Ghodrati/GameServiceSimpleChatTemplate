using FiroozehGameService.Handlers;
using FiroozehGameService.Models.GSLive.Chat;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Controllers
{
    public class ListController : MonoBehaviour
    {
        public GameObject ContentPanel;

        public GameObject ListItemPrefab;

        public ScrollRect ScrollRect;
        
        // Start is called before the first frame update
        void Start()
        {
            ScrollRect = GetComponent<ScrollRect>();
            SetEventHandlers();
        }
        
        private void MoveToEnd()
        {
            Canvas.ForceUpdateCanvases();
            ScrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
            ScrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            ScrollRect.verticalNormalizedPosition = 0;
        }

        private void SetEventHandlers()
        {
            ChatEventHandlers.OnChatReceived += OnChatReceived;
            ChatEventHandlers.OnUnSubscribeChannel += OnUnSubscribeChannel;
        }

        
        private void OnUnSubscribeChannel(object sender, string channelName)
        {
            MenuController.ChannelNameSubscribed = null;
        }

       
        /// <summary>
        /// When new Chat Message Received , Instantiate new Chat Item and Set Properties On it 
        /// </summary>
        private void OnChatReceived(object sender, Chat chat)
        {
            Debug.Log("OnChatReceived : " + chat.Message);
            var newChatItem = Instantiate(ListItemPrefab);
            var controller = newChatItem.GetComponent<ChatItemController>();
            
            controller.UserName.text = chat.Sender.Name;
            controller.Message.text = chat.Message;
            StartCoroutine(ImageUtil.DownloadImage(chat.Sender.Logo,controller.UserIcon));
            
            
            newChatItem.transform.parent = ContentPanel.transform;
            newChatItem.transform.localScale = Vector3.one;
            
            MoveToEnd();
        }
    }
}
