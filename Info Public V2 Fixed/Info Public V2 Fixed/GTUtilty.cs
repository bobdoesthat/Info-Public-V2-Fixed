using GorillaLocomotion;
using GorillaNetworking;
using HarmonyLib;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000004 RID: 4
public static class GTUtility
{
    // Token: 0x06000003 RID: 3 RVA: 0x00002069 File Offset: 0x00000269
    public static VRRig GetLocalVRRig()
    {
        GTPlayer instance = GTPlayer.Instance;
        return (instance != null) ? instance.GetComponentInChildren<VRRig>() : null;
    }

    // Token: 0x06000004 RID: 4 RVA: 0x0000207C File Offset: 0x0000027C
    public static string GetPlayerName()
    {
        VRRig localVRRig = GTUtility.GetLocalVRRig();
        string text;
        if (localVRRig == null)
        {
            text = null;
        }
        else
        {
            NetPlayer owningNetPlayer = localVRRig.OwningNetPlayer;
            text = ((owningNetPlayer != null) ? owningNetPlayer.NickName : null);
        }
        return text ?? "Unknown";
    }

    // Token: 0x06000005 RID: 5 RVA: 0x000020A4 File Offset: 0x000002A4
    public static string GetUserId(VRRig rig)
    {
        string text;
        if (rig == null)
        {
            text = null;
        }
        else
        {
            NetPlayer owningNetPlayer = rig.OwningNetPlayer;
            text = ((owningNetPlayer != null) ? owningNetPlayer.UserId : null);
        }
        return text ?? "Unknown";
    }

    // Token: 0x06000006 RID: 6 RVA: 0x000020C8 File Offset: 0x000002C8
    public static string GetAccountCreationDate(VRRig rig)
    {
        bool flag = rig == null || rig.OwningNetPlayer == null;
        string result2;
        if (flag)
        {
            result2 = "UNKNOWN";
        }
        else
        {
            string userId = rig.OwningNetPlayer.UserId;
            bool flag2 = GTUtility.datePool.ContainsKey(userId);
            if (flag2)
            {
                result2 = GTUtility.datePool[userId];
            }
            else
            {
                GTUtility.datePool[userId] = "LOADING";
                PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
                {
                    PlayFabId = userId
                }, delegate (GetAccountInfoResult result)
                {
                    string value = result.AccountInfo.Created.ToString("MMM dd, yyyy HH:mm").ToUpperInvariant();
                    GTUtility.datePool[userId] = value;
                    rig.UpdateName();
                }, delegate (PlayFabError error)
                {
                    GTUtility.datePool[userId] = "ERROR";
                    rig.UpdateName();
                }, null, null);
                result2 = "LOADING";
            }
        }
        return result2;
    }

    // Token: 0x06000007 RID: 7 RVA: 0x000021A0 File Offset: 0x000003A0
    public static string GetCheats(VRRig rig)
    {
        bool flag = rig == null || rig.OwningNetPlayer == null;
        string result;
        if (flag)
        {
            result = "None";
        }
        else
        {
            NetPlayer owningNetPlayer = rig.OwningNetPlayer;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (DictionaryEntry dictionaryEntry in owningNetPlayer.GetPlayerRef().CustomProperties)
            {
                dictionary[dictionaryEntry.Key.ToString().ToLower()] = dictionaryEntry.Value;
            }
            string text = "";
            foreach (KeyValuePair<string, string> keyValuePair in GTUtility.specialModsList)
            {
                bool flag2 = dictionary.ContainsKey(keyValuePair.Key.ToLower());
                if (flag2)
                {
                    text = text + ((text == "") ? "" : ", ") + keyValuePair.Value.ToUpper();
                }
            }
            result = (string.IsNullOrEmpty(text) ? "None" : text);
        }
        return result;
    }

    // Token: 0x06000008 RID: 8 RVA: 0x000022E8 File Offset: 0x000004E8
    public static string GetFPS(VRRig rig)
    {
        bool flag = rig == null;
        string result;
        if (flag)
        {
            result = "N/A";
        }
        else
        {
            Traverse traverse = Traverse.Create(rig).Field("fps");
            bool flag2 = traverse != null;
            if (flag2)
            {
                result = "FPS " + traverse.GetValue().ToString();
            }
            else
            {
                result = "N/A";
            }
        }
        return result;
    }

    // Token: 0x06000009 RID: 9 RVA: 0x00002334 File Offset: 0x00000534
    public static string GetPlatform(VRRig rig)
    {
        bool flag = rig == null;
        string result;
        if (flag)
        {
            result = "Unknown";
        }
        else
        {
            string concatStringOfCosmeticsAllowed = rig.concatStringOfCosmeticsAllowed;
            bool flag2 = concatStringOfCosmeticsAllowed.Contains("game-purchase-bundle");
            if (flag2)
            {
                result = "Rift";
            }
            else
            {
                bool flag3 = concatStringOfCosmeticsAllowed.Contains("S. FIRST LOGIN");
                if (flag3)
                {
                    result = "Steam";
                }
                else
                {
                    bool flag4 = concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN");
                    if (flag4)
                    {
                        result = "PC";
                    }
                    else
                    {
                        result = "Unknown";
                    }
                }
            }
        }
        return result;
    }

    // Token: 0x0600000A RID: 10 RVA: 0x000023B0 File Offset: 0x000005B0
    public static List<VRRig> GetAllPlayers()
    {
        VRRig[] collection = UnityEngine.Object.FindObjectsOfType<VRRig>();
        return new List<VRRig>(collection);
    }

    // Token: 0x0600000B RID: 11 RVA: 0x000023D0 File Offset: 0x000005D0
    public static string GetCosmetics(VRRig rig)
    {
        bool flag = rig == null;
        string result;
        if (flag)
        {
            result = "None";
        }
        else
        {
            string text = "";
            foreach (KeyValuePair<string, string[]> keyValuePair in GTUtility.SpecialCosmetics)
            {
                bool flag2 = rig.concatStringOfCosmeticsAllowed.Contains(keyValuePair.Key);
                if (flag2)
                {
                    text = string.Concat(new string[]
                    {
                        text,
                        (text == "") ? "" : ", ",
                        "<color=#",
                        keyValuePair.Value[1],
                        ">",
                        keyValuePair.Value[0],
                        "</color>"
                    });
                }
            }
            bool flag3 = rig.cosmeticSet != null;
            if (flag3)
            {
                foreach (CosmeticsController.CosmeticItem cosmeticItem in rig.cosmeticSet.items)
                {
                    bool flag4 = !cosmeticItem.isNullItem && !rig.concatStringOfCosmeticsAllowed.Contains(cosmeticItem.itemName);
                    if (flag4)
                    {
                        text = text + ((text == "") ? "" : ", ") + "<color=green>COSMETX</color>";
                        break;
                    }
                }
            }
            result = ((text == "") ? "None" : text);
        }
        return result;
    }

    // Token: 0x04000002 RID: 2
    private static readonly Dictionary<string, string> datePool = new Dictionary<string, string>();

    // Token: 0x04000003 RID: 3
    private static Dictionary<string, string> specialModsList = new Dictionary<string, string>
    {
        {
            "genesis",
            "GENESIS"
        },
        {
            "HP_Left",
            "HOLDABLEPAD"
        },
        {
            "GrateVersion",
            "GRATE"
        },
        {
            "void",
            "VOID"
        },
        {
            "BANANAOS",
            "BANANAOS"
        },
        {
            "GC",
            "GORILLACRAFT"
        },
        {
            "CarName",
            "GORILLAVEHICLES"
        },
        {
            "6p72ly3j85pau2g9mda6ib8px",
            "CCMV2"
        },
        {
            "FPS-Nametags for Zlothy",
            "FPSTAGS"
        },
        {
            "cronos",
            "CRONOS"
        },
        {
            "ORBIT",
            "ORBIT"
        },
        {
            "Violet On Top",
            "VIOLET"
        },
        {
            "MP25",
            "MONKEPHONE"
        },
        {
            "GorillaWatch",
            "GORILLAWATCH"
        },
        {
            "InfoWatch",
            "GORILLAINFOWATCH"
        },
        {
            "BananaPhone",
            "BANANAPHONE"
        },
        {
            "Vivid",
            "VIVID"
        },
        {
            "RGBA",
            "CUSTOMCOSMETICS"
        },
        {
            "cheese is gouda",
            "WHOSICHEATING"
        },
        {
            "shirtversion",
            "GORILLASHIRTS"
        },
        {
            "gpronouns",
            "GORILLAPRONOUNS"
        },
        {
            "gfaces",
            "GORILLAFACES"
        },
        {
            "monkephone",
            "MONKEPHONE"
        },
        {
            "pmversion",
            "PLAYERMODELS"
        },
        {
            "gtrials",
            "GORILLATRIALS"
        },
        {
            "msp",
            "MONKESMARTPHONE"
        },
        {
            "gorillastats",
            "GORILLASTATS"
        },
        {
            "using gorilladrift",
            "GORILLADRIFT"
        },
        {
            "monkehavocversion",
            "MONKEHAVOC"
        },
        {
            "tictactoe",
            "TICTACTOE"
        },
        {
            "ccolor",
            "INDEX"
        },
        {
            "imposter",
            "GORILLAAMONGUS"
        },
        {
            "spectapeversion",
            "SPECTAPE"
        },
        {
            "cats",
            "CATS"
        },
        {
            "made by biotest05 :3",
            "DOGS"
        },
        {
            "fys cool magic mod",
            "FYSMAGICMOD"
        },
        {
            "colour",
            "CUSTOMCOSMETICS"
        },
        {
            "chainedtogether",
            "CHAINED TOGETHER"
        },
        {
            "goofywalkversion",
            "GOOFYWALK"
        },
        {
            "void_menu_open",
            "VOID"
        },
        {
            "violetpaiduser",
            "VIOLETPAID"
        },
        {
            "violetfree",
            "VIOLETFREE"
        },
        {
            "obsidianmc",
            "OBSIDIAN.LOL"
        },
        {
            "dark",
            "SHIBAGT DARK"
        },
        {
            "hidden menu",
            "HIDDEN"
        },
        {
            "oblivionuser",
            "OBLIVION"
        },
        {
            "hgrehngio889584739_hugb\n",
            "RESURGENCE"
        },
        {
            "eyerock reborn",
            "EYEROCK"
        },
        {
            "asteroidlite",
            "ASTEROID LITE"
        },
        {
            "elux",
            "ELUX"
        },
        {
            "cokecosmetics",
            "COKE COSMETX"
        },
        {
            "GFaces",
            "gFACES"
        },
        {
            "github.com/maroon-shadow/SimpleBoards",
            "SIMPLEBOARDS"
        },
        {
            "ObsidianMC",
            "OBSIDIAN"
        },
        {
            "hgrehngio889584739_hugb",
            "RESURGENCE"
        },
        {
            "GTrials",
            "gTRIALS"
        },
        {
            "github.com/ZlothY29IQ/GorillaMediaDisplay",
            "GMD"
        },
        {
            "github.com/ZlothY29IQ/TooMuchInfo",
            "TOOMUCHINFO"
        },
        {
            "github.com/ZlothY29IQ/RoomUtils-IW",
            "ROOMUTILS-IW"
        },
        {
            "github.com/ZlothY29IQ/MonkeClick",
            "MONKECLICK"
        },
        {
            "github.com/ZlothY29IQ/MonkeClick-CI",
            "MONKECLICK-CI"
        },
        {
            "github.com/ZlothY29IQ/MonkeRealism",
            "MONKEREALISM"
        },
        {
            "MediaPad",
            "MEDIAPAD"
        },
        {
            "GorillaCinema",
            "gCINEMA"
        },
        {
            "ChainedTogetherActive",
            "CHAINEDTOGETHER"
        },
        {
            "GPronouns",
            "gPRONOUNS"
        },
        {
            "CSVersion",
            "CustomSkin"
        },
        {
            "github.com/ZlothY29IQ/Zloth-RecRoomRig",
            "ZLOTH-RRR"
        },
        {
            "ShirtProperties",
            "SHIRTS-OLD"
        },
        {
            "GorillaShirts",
            "SHIRTS"
        },
        {
            "GS",
            "OLD SHIRTS"
        },
        {
            "6XpyykmrCthKhFeUfkYGxv7xnXpoe2",
            "CCMV2"
        },
        {
            "Body Tracking",
            "BODYTRACK-OLD"
        },
        {
            "Body Estimation",
            "HANBodyEst"
        },
        {
            "Gorilla Track",
            "BODYTRACK"
        },
        {
            "CustomMaterial",
            "CUSTOMCOSMETICS"
        },
        {
            "I like cheese",
            "RECROOMRIG"
        },
        {
            "silliness",
            "SILLINESS"
        },
        {
            "emotewheel",
            "EMOTEWHEEL"
        },
        {
            "untitled",
            "UNTITLED"
        }
    };

    // Token: 0x04000004 RID: 4
    public static readonly Dictionary<string, string[]> SpecialCosmetics = new Dictionary<string, string[]>
    {
        {
            "LBAAD.",
            new string[]
            {
                "ADMINISTRATOR",
                "FF0000"
            }
        },
        {
            "LBAAK.",
            new string[]
            {
                "FOREST GUIDE",
                "867556"
            }
        },
        {
            "LBADE.",
            new string[]
            {
                "FINGER PAINTER",
                "00FF00"
            }
        },
        {
            "LBAGS.",
            new string[]
            {
                "ILLUSTRATOR",
                "C76417"
            }
        },
        {
            "LMAPY.",
            new string[]
            {
                "FOREST GUIDE MOD STICK"
            }
        },
        {
            "LBANI.",
            new string[]
            {
                "AA CREATOR BADGE"
            }
        }
    };
}
