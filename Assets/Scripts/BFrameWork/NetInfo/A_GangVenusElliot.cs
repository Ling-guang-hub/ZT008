using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
public class A_GangVenusElliot : ASingletonBehaviour<A_GangVenusElliot>
{
    public string version = "1.2";
    public string AeroItem= A_RodDumpBee.instance.AeroItem;
    //channel
#if UNITY_IOS
    private string Flannel= "AppStore";
#elif UNITY_ANDROID
    private string Channel = "GooglePlay";
#else
    private string Channel = "GooglePlay";
#endif


    private void OnApplicationPause(bool pause)
    {
        SlipAeroCreditor();
    }

    public Text Loss;

    protected override void OnLoad()
    {
        base.OnLoad();
        version = Application.version;
        StartCoroutine(nameof(KeelDweller));
    }
    
    IEnumerator KeelDweller()
    {
        while (true)
        {
            yield return new WaitForSeconds(120f);
            Instance.SlipAeroCreditor();
        }
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("event_day") != DateTime.Now.Day && PlayerPrefs.GetString("user_servers_id").Length != 0)
        {
            PlayerPrefs.SetInt("event_day", DateTime.Now.Day);
        }
    }
    public void AtomOxRoofVenus(string event_id)
    {
        AtomVenus(event_id);
    }
    public void SlipAeroCreditor(List<string> valueList = null)
    {
        if (PlayerPrefs.GetFloat(BConsumer.ArchiveKey.It_ChimpanzeeGripJust) == 0)
        {
            PlayerPrefs.SetFloat(BConsumer.ArchiveKey.It_ChimpanzeeGripJust, PlayerPrefs.GetFloat(BConsumer.ArchiveKey.It_GripJust));
        }
        if (PlayerPrefs.GetFloat(BConsumer.ArchiveKey.It_ChimpanzeeSmite) == 0)
        {
            PlayerPrefs.SetFloat(BConsumer.ArchiveKey.It_ChimpanzeeSmite, PlayerPrefs.GetFloat(BConsumer.ArchiveKey.It_Smite));
        }
        if (valueList == null)
        {
            valueList = new List<string>() {
                PlayerPrefs.GetInt(BConsumer.ArchiveKey.It_ChimpanzeeGripJust).ToString(),
                PlayerPrefs.GetInt(BConsumer.ArchiveKey.It_TransitCallRevise).ToString(),
                PlayerPrefs.GetString(BConsumer.ArchiveKey.It_ChimpanzeeSmite),
                PlayerPrefs.GetFloat(BConsumer.ArchiveKey.It_HavenAeroDust).ToString()
                //SaveDataManager.GetInt(SlotConfig.sv_SlotSpinCount).ToString()
            };
        }

        if (PlayerPrefs.GetString(BConsumer.ArchiveKey.It_GuessMexicoSo) == null)
        {
            return;
        }
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("gameCode", AeroItem);
        wwwForm.AddField("userId", PlayerPrefs.GetString(BConsumer.ArchiveKey.It_GuessMexicoSo));

        wwwForm.AddField("gameVersion", version);

        wwwForm.AddField("channel", Flannel);

        for (int i = 0; i < valueList.Count; i++)
        {
            wwwForm.AddField("resource" + (i + 1), valueList[i]);
        }



        StartCoroutine(AtomGang(A_RodDumpBee.instance.MintGun + "/api/client/game_progress", wwwForm,
        (error) =>
        {
            Debug.Log(error);
        },
        (message) =>
        {
            Debug.Log(message);
        }));
    }
    public void AtomVenus(string event_id, string p1 = null, string p2 = null, string p3 = null)
    {
        if (Loss != null)
        {
            if (int.Parse(event_id) < 9100 && int.Parse(event_id) >= 9000)
            {
                if (p1 == null)
                {
                    p1 = "";
                }
                Loss.text += "\n" + DateTime.Now.ToString() + "id:" + event_id + "  p1:" + p1;
            }
        }
        if (PlayerPrefs.GetString(BConsumer.ArchiveKey.It_GuessMexicoSo) == null)
        {
            A_RodDumpBee.instance.Karst();
            return;
        }
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("gameCode", AeroItem);
        wwwForm.AddField("userId", PlayerPrefs.GetString(BConsumer.ArchiveKey.It_GuessMexicoSo));
        //Debug.Log("userId:" + SaveDataManager.GetString(CConfig.sv_LocalServerId));
        wwwForm.AddField("version", version);
        //Debug.Log("version:" + version);
        wwwForm.AddField("channel", Flannel);
        //Debug.Log("channel:" + channal);
        wwwForm.AddField("operateId", event_id);
        Debug.Log("operateId:" + event_id);


        if (p1 != null)
        {
            wwwForm.AddField("params1", p1);
        }
        if (p2 != null)
        {
            wwwForm.AddField("params2", p2);
        }
        if (p3 != null)
        {
            wwwForm.AddField("params3", p3);
        }
        StartCoroutine(AtomGang(A_RodDumpBee.instance.MintGun + "/api/client/log", wwwForm,
        (error) =>
        {
            Debug.Log(error);
        },
        (message) =>
        {
            Debug.Log(message);
        }));
    }
    IEnumerator AtomGang(string _url, WWWForm wwwForm, Action<string> fail, Action<string> success)
    {
        //Debug.Log(SerializeDictionaryToJsonString(dic));
        using UnityWebRequest request = UnityWebRequest.Post(_url, wwwForm);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isNetworkError)
        {
            fail(request.error);
            MowMeeting();
            request.Dispose();
        }
        else
        {
            success(request.downloadHandler.text);
            MowMeeting();
            request.Dispose();
        }
    }
    private void MowMeeting()
    {
        StopCoroutine(nameof(AtomGang));
    }


}