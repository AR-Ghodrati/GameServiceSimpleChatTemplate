using System;
using System.Threading.Tasks;
using FiroozehGameService.Core;
using FiroozehGameService.Handlers;
using FiroozehGameService.Models;
using FiroozehGameService.Models.Command;
using FiroozehGameService.Utils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using LogType = FiroozehGameService.Utils.LogType;

namespace Controllers
{
    public class MenuController : MonoBehaviour
    {
        public GameObject StartMenu;
        public GameObject LoginMenu;
        public Button StartGameBtn;
        public Text StartMenuText;
        public Text Status;
        public InputField ChannelName;

    
    
        public InputField NickName;
        public InputField Email;
        public InputField Password;
        public Button Submit;
        public GameObject SwitchToRegisterOrLogin;
        public Text LoginErr;


        public static string ChannelNameSubscribed;
                
        
        private void Start()
        {
            if(GameService.IsAuthenticated()) return;

            SetEventHandlers();
            ConnectToGamesService();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            GameService.Logout();
            Application.Quit();
        }


        private void SetEventHandlers()
        {
            CoreEventHandlers.SuccessfullyLogined += SuccessfullyLogined;
            CoreEventHandlers.Error += Error;
            
            ChatEventHandlers.OnSubscribeChannel += OnSubscribeChannel;
            LogUtil.LogEventHandler += LogEventHandler;

        }

        private void LogEventHandler(object sender, Log e)
        {
            if(Status != null) Status.text += e.Txt + "\r\n";
            if(e.Type == LogType.Normal) Debug.Log(e.Txt);
            else Debug.LogError(e.Txt);
        }


        /// <summary>
        /// When Channel Subscribed , go To ChatScene
        /// </summary>
        private void OnSubscribeChannel(object sender, string channelName)
        {
            ChannelNameSubscribed = channelName;
            Debug.Log("OnSubscribeChannel, channelName : " + channelName);
            SceneManager.LoadScene("ChatScene"); 
        }

        
        /// <summary>
        /// SubscribeChannel With channelName
        /// </summary>
        private async Task SubscribeChannel(string channelName)
        {
           await GameService.GSLive.Chat.SubscribeChannel(channelName);
        }

        private void Error(object sender, ErrorEvent e)
        {
            Status.text = "Status : Error in : " + e.Type + ", txt : " + e.Error;
            Debug.LogError("Error in : " + e.Type + ", txt : " + e.Error);
        }

        
        /// <summary>
        /// Enable UI When SuccessfullyLogined
        /// </summary>
        private void SuccessfullyLogined(object sender, EventArgs e)
        {
            // Disable LoginUI
            StartMenu.SetActive(true);
            LoginMenu.SetActive(false);
            ChannelName.gameObject.SetActive(true);
            StartGameBtn.interactable = true;
            
            StartGameBtn.onClick.AddListener(async () => { await SubscribeChannel(ChannelName.text.Trim());});

            
            Status.text = "Status : Connected!";
            Debug.Log("SuccessfullyLogined!");
        }

        /// <summary>
        /// Connect To GameService -> Login Or SignUp
        /// It May Throw Exception
        /// </summary>
        private void ConnectToGamesService () {
        //connecting to GamesService
        Status.text = "Status : Connecting...";
        StartGameBtn.interactable = false;
        ChannelName.gameObject.SetActive(false);
            
        SwitchToRegisterOrLogin.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (NickName.IsActive())
            {
                NickName.gameObject.SetActive(false);
                SwitchToRegisterOrLogin.GetComponent<Text>().text = "Dont have an account? Register!";
            }
            else
            {
                NickName.gameObject.SetActive(true);
                SwitchToRegisterOrLogin.GetComponent<Text>().text = "Have an Account? Login!";
            }
        });

        
            // Enable LoginUI
            StartMenu.SetActive(false);
            LoginMenu.SetActive(true);
            
            Submit.onClick.AddListener(async () =>
            {
                try
                {
                    if (NickName.IsActive()) // is SignUp
                    {
                        var nickName = NickName.text.Trim();
                        var email = Email.text.Trim();
                        var pass = Password.text.Trim();

                        if (string.IsNullOrEmpty(nickName)
                            && string.IsNullOrEmpty(email)
                            && string.IsNullOrEmpty(pass))
                            LoginErr.text = "Invalid Input!";
                        else
                        {
                            var userToken = await GameService.SignUp(nickName, email, pass);
                            FileUtil.SaveUserToken(userToken);
                        }

                    }
                    else
                    {
                        var email = Email.text.Trim();
                        var pass = Password.text.Trim();

                        if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(pass))
                            LoginErr.text = "Invalid Input!";
                        else
                        {
                            var userToken = await GameService.Login(email, pass);
                            FileUtil.SaveUserToken(userToken);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (e is GameServiceException) LoginErr.text = "GameServiceException : " + e.Message;
                    else LoginErr.text = "InternalException2 : " + e.Message;
                }
               
            });
        }        
   
    }
}