using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;

public class TokenGateManager : MonoBehaviour
{
    public Button tokenGateNFTButton;
    public Button buttonStartGame;
    public string Address { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        tokenGateNFTButton.gameObject.SetActive(false);
        buttonStartGame.gameObject.SetActive(false);
        tokenGateNFTButton.interactable = true;
        buttonStartGame.interactable = true;
    }

    public async void Login()
    {
        tokenGateNFTButton.interactable = true;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Debug.Log(Address);
        Contract contract = ThirdwebManager.Instance.SDK.GetContract("0xB046ae6bf7152C2d8697262E440f236eD6683b39");
        List<NFT> nftList = await contract.ERC721.GetOwned(Address);
        if (nftList.Count == 0)
        {
            tokenGateNFTButton.gameObject.SetActive(true);
            buttonStartGame.gameObject.SetActive(false);
        }
        else
        {
            tokenGateNFTButton.gameObject.SetActive(false);
            buttonStartGame.gameObject.SetActive(true);
        }
    }

    public async void ClaimNFTPass()
    {
        tokenGateNFTButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract("0xB046ae6bf7152C2d8697262E440f236eD6683b39");
        var result = await contract.ERC721.ClaimTo(Address, 1);
        tokenGateNFTButton.gameObject.SetActive(false);
        buttonStartGame.gameObject.SetActive(true);
    }

}
