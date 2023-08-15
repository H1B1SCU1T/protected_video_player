# protected_video_player
You can modify the default UdonSharp video player to only be playable by those on a whitelist fetched via http/s. Remove anything from the udon graph that isn't involved in video syncing. Control of the input box, url, and playing the video should be guarded.

```csharp
// ...

public class protect_ownership : UdonSharpBehaviour
{   
    // Replace this with your own RAW text file from github or pastebin
    VRCUrl names = new VRCUrl("https://pastebin.com/raw/eeeee");

// ...

void Start()
{
    VRCStringDownloader.LoadUrl(names, (IUdonEventReceiver)this);

// ...
```

VRCUrl objects cannot be created on the fly, and new objects cannot be created in methods of the UdonSharpBehaviour. They must exist only as members of the class. You can however request the data anywhere in the class.

```csharp
     public void OnURLChanged() {

        // ... 

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

        // transfer ownership and play url
    }
```

Just check the local user against the list you pulled from github or pastebin. Pastebin and github are the only allowed urls. Others will require users to have "untrusted urls" enabled. 

```
user_1
user_2
user_3
```

New line delimited is easier. When you work with this kind of data, split on '\n' characters and `trim()` each element of the resulting array. 



This of course is just an example. Check out the entire file: "protect_ownership.cs".
