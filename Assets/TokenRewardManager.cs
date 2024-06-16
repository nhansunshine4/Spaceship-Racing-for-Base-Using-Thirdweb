using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;

namespace SpeederRunGame
{
    public class TokenRewardManager : MonoBehaviour
    {
        public Button tokenClaimButton;
        public Button rankingButton;
        public Button buttonMainMenu;
        public Button buttonRestart;

        public Text tokenBalanceText;

        public Text bronzeBalanceAmountText;
        public Text silverBalanceAmountText;
        public Text goldBalanceAmountText;

        public Text rankingText;

        public Text claimingStatusText;

        public string Address { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            tokenClaimButton.gameObject.SetActive(true);
            tokenClaimButton.interactable = true;
            rankingButton.gameObject.SetActive(true);
            rankingButton.interactable = false;
            buttonMainMenu.gameObject.SetActive(true);
            buttonMainMenu.interactable = true;
            buttonRestart.gameObject.SetActive(true);
            buttonRestart.interactable = true;
            claimingStatusText.text = "";
            GetTokenBalance();
            GetRewardBalance();
            GetRank();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public async void GetTokenBalance()
        {
            Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            var contract = ThirdwebManager.Instance.SDK.GetContract("0x00f618A043e4cC4C7E06FCA4c5D669837eB2F7dA");
            var balance = await contract.ERC20.BalanceOf(Address);
            tokenBalanceText.text = "Token Owned: " + balance.displayValue;
            claimingStatusText.text = "";
        }

        public async void ClaimToken()
        {
            tokenClaimButton.interactable = false;
            rankingButton.interactable = false;
            buttonMainMenu.interactable = false;
            buttonRestart.interactable = false;
            Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            var contract = ThirdwebManager.Instance.SDK.GetContract("0x00f618A043e4cC4C7E06FCA4c5D669837eB2F7dA");
            claimingStatusText.text = "Claiming!";
            GameObject gameController = GameObject.Find("GameControllerStage01");
            if (gameController != null)
            {                
                SRGGameController srgGameController = gameController.GetComponent<SRGGameController>();
                if (srgGameController != null)
                {
                    if (srgGameController.score == 0f) {
                        buttonMainMenu.interactable = true;
                        buttonRestart.interactable = true;
                        return;
                    }
                    var result = await contract.ERC20.ClaimTo(Address, srgGameController.score.ToString());
                    ClaimReward(srgGameController.score.ToString());
                    GetTokenBalance();
                }
                else
                {
                    Debug.LogError("SRGGameController component not found on GameControllerStage01");
                }
            }
            else
            {
                GameObject gameController2 = GameObject.Find("GameControllerStage02");
                if (gameController2 != null)
                {                   
                    SRGGameController srgGameController = gameController2.GetComponent<SRGGameController>();
                    if (srgGameController != null)
                    {
                        if (srgGameController.score == 0f)
                        {
                            buttonMainMenu.interactable = true;
                            buttonRestart.interactable = true;
                            return;
                        }
                        var result = await contract.ERC20.ClaimTo(Address, srgGameController.score.ToString());
                        ClaimReward(srgGameController.score.ToString());
                        GetTokenBalance();
                    }
                    else
                    {
                        Debug.LogError("SRGGameController component not found on GameControllerStage02");
                    }
                }
                else
                {
                    GameObject gameController3 = GameObject.Find("GameControllerStage03");
                    if (gameController3 != null)
                    {
                        SRGGameController srgGameController = gameController3.GetComponent<SRGGameController>();
                        if (srgGameController != null)
                        {
                            if (srgGameController.score == 0f)
                            {
                                buttonMainMenu.interactable = true;
                                buttonRestart.interactable = true;
                                return;
                            }
                            var result = await contract.ERC20.ClaimTo(Address, srgGameController.score.ToString());
                            ClaimReward(srgGameController.score.ToString());
                            GetTokenBalance();
                        }
                        else
                        {
                            Debug.LogError("SRGGameController component not found on GameControllerStage03");
                        }
                    }
                }
            }
        }

        public async void GetRewardBalance()
        {
            Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            var contract = ThirdwebManager.Instance.SDK.GetContract("0x26F972675F0C0BE15b1e0bC5c200282F035db1C5");
            var bronzeBalance = await contract.ERC1155.BalanceOf(Address, "0");
            var silverBalance = await contract.ERC1155.BalanceOf(Address, "1");
            var goldBalance = await contract.ERC1155.BalanceOf(Address, "2");

            bronzeBalanceAmountText.text = bronzeBalance.ToString();
            silverBalanceAmountText.text = silverBalance.ToString();
            goldBalanceAmountText.text = goldBalance.ToString();
            claimingStatusText.text = "";
        }

        public async void ClaimReward(string _distanceTravelled)
        {
            Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            var contract = ThirdwebManager.Instance.SDK.GetContract("0x26F972675F0C0BE15b1e0bC5c200282F035db1C5");
            if (int.Parse(_distanceTravelled) >= 2000)
            {
                await contract.ERC1155.ClaimTo(Address, "2", 1);
                rankingButton.interactable = true;
                buttonMainMenu.interactable = true;
                buttonRestart.interactable = true;
                claimingStatusText.text = "Claimed!";
            }
            else if (int.Parse(_distanceTravelled) >= 1000)
            {
                await contract.ERC1155.ClaimTo(Address, "1", 1);
                rankingButton.interactable = true;
                buttonMainMenu.interactable = true;
                buttonRestart.interactable = true;
                claimingStatusText.text = "Claimed!";
            }
            else if (int.Parse(_distanceTravelled) >= 500)
            {
                await contract.ERC1155.ClaimTo(Address, "0", 1);
                rankingButton.interactable = true;
                buttonMainMenu.interactable = true;
                buttonRestart.interactable = true;
                claimingStatusText.text = "Claimed!";
            }
            else {
                rankingButton.interactable = true;
                buttonMainMenu.interactable = true;
                buttonRestart.interactable = true;
                claimingStatusText.text = "Claimed!";
            }
            GetRewardBalance();
        }

        public async void SubmitScore()
        {
            rankingButton.interactable = false;
            buttonMainMenu.interactable = false;
            buttonRestart.interactable = false;
            claimingStatusText.text = "Claiming!";
            Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            var contract = ThirdwebManager.Instance.SDK.GetContract(
                    "0x53eBE0120369740eaa27E095bb9dA4c2A6034478",
                    "[{\"type\":\"event\",\"name\":\"ScoreAddedd\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"score\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"_scores\",\"inputs\":[{\"type\":\"address\",\"name\":\"\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getRank\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"rank\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"submitScore\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"score\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]"
                );

            GameObject gameController = GameObject.Find("GameControllerStage01");
            if (gameController != null)
            {
                SRGGameController srgGameController = gameController.GetComponent<SRGGameController>();
                if (srgGameController != null)
                {
                    await contract.Write("submitScore", (int)srgGameController.score);
                    buttonMainMenu.interactable = true;
                    buttonRestart.interactable = true;
                    GetRank();
                }
                else
                {
                    Debug.LogError("SubmitScore1 Error");
                }
            }
            else
            {
                GameObject gameController2 = GameObject.Find("GameControllerStage02");
                if (gameController2 != null)
                {
                    SRGGameController srgGameController = gameController2.GetComponent<SRGGameController>();
                    if (srgGameController != null)
                    {
                        await contract.Write("submitScore", (int)srgGameController.score);
                        buttonMainMenu.interactable = true;
                        buttonRestart.interactable = true;
                        GetRank();
                    }
                    else
                    {
                        Debug.LogError("SubmitScore2 Error");
                    }
                }
                else
                {
                    GameObject gameController3 = GameObject.Find("GameControllerStage03");
                    if (gameController3 != null)
                    {
                        SRGGameController srgGameController = gameController3.GetComponent<SRGGameController>();
                        if (srgGameController != null)
                        {
                            await contract.Write("submitScore", (int)srgGameController.score);
                            buttonMainMenu.interactable = true;
                            buttonRestart.interactable = true;
                            GetRank();
                        }
                        else
                        {
                            Debug.LogError("SubmitScore3 Error");
                        }
                    }
                }
            }          
        }

        internal async void GetRank()
        {
            Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            var contract = ThirdwebManager.Instance.SDK.GetContract(
                "0x53eBE0120369740eaa27E095bb9dA4c2A6034478",
                "[{\"type\":\"event\",\"name\":\"ScoreAddedd\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"score\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"_scores\",\"inputs\":[{\"type\":\"address\",\"name\":\"\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getRank\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"rank\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"submitScore\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"score\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]"
                );
            var rank = await contract.Read<int>("getRank", Address);
            Debug.Log($"Rank for address {Address} is {rank}");
            rankingText.text = "Global Ranking: " + rank.ToString();
            claimingStatusText.text = "Claimed!";
            claimingStatusText.text = "";
        }
    }
}

