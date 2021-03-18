using FiroozehGameService.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class SendMessageController : MonoBehaviour
    {
        public Button SendButton;
        public InputField MessageToSend;


        private void Start()
        {
            SendButton.onClick.AddListener(async () =>
            {
                if (!GameService.IsAuthenticated()) return;
                var message = MessageToSend.text.Trim();
                await GameService.GSLive.Chat().SendChannelMessage(MenuController.ChannelNameSubscribed, message);

                MessageToSend.text = null;
            });
        }
    }
}