using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDK3.Components;
using VRC.SDK3.Video.Components.Base;

public class protect_ownership : UdonSharpBehaviour
{   
    // Replace this with your own RAW text file from github or pastebin
    VRCUrl names = new VRCUrl("https://pastebin.com/raw/eeeee");

    string[] allowList;

    public VRCUrlInputField urlInputField;

    public BaseVRCVideoPlayer videoPlayer;

    [SerializeField] private VRCUrl videoUrl;

    void Start()
    {
        // make sure OUR OnStringLoadSuccess handler gets the OnStringLoadSuccess event 
        VRCStringDownloader.LoadUrl(names, (IUdonEventReceiver)this);
    }

    void OnStringLoadSuccess(IVRCStringDownload result) {
        Debug.Log(result.Result);
        allowList =  result.Result.Split(char.Parse("\n"));
    }

     public void OnURLChanged() {
        VRCPlayerApi localPlayer = VRC.SDKBase.Networking.LocalPlayer;
        string displayName = localPlayer.displayName;
        
        Debug.Log("Ownership requested by: " + displayName);
  
        bool allowed = false;

        for (int i = 0; i < allowList.Length; i++)
        {
            Debug.Log("Is player: " + this.allowList[i]);

            if(displayName.Equals(this.allowList[i].Trim())) {
                allowed = true;
                Debug.Log("User is: " + displayName);
                break;
            }
        }

        if (!allowed) return;

        // will be ignored if already owner
        VRC.SDKBase.Networking.SetOwner(localPlayer, videoPlayer.gameObject);

        this.videoUrl = urlInputField.GetUrl();

        this.RequestSerialization();

        // even if user is owner of the player, if they are not in this list they cannot play videos
        this.videoPlayer.PlayURL(this.videoUrl);
    }
}
