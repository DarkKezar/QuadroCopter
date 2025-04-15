using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using DroneScripts;
using System.IO;
using Unity.VisualScripting.FullSerializer;

[Serializable]
public class GetData
{
    public string user;
    public string location;
    public string game_mode;
    public int level;
    public int location_enum;
    public int gamemode_enum;

    public GetData()
    {
        user = default;
        location = default;
        game_mode = default;
        level = 0;
        location_enum = 0;
        gamemode_enum = 0;
    }
}

[Serializable]
public class PostData
{
    public int id;
    public int type;

    [Serializable]
    public class Checkpoints
    {
        public int all;
        public int get;

        public Checkpoints()
        {
            all = 0;
            get = 0;
        }
    }
    [Serializable]
    public class Damage
    {
        public int max;
        public int get;

        public Damage()
        {
            max = 0;
            get = 0;
        }
    }
    [Serializable]
    public class Result
    {
        public int date;
        public bool is_finished;
        public int time;
        public Damage damage;
        public int collisions;
        public Checkpoints check_points;

        public Result()
        {
            date = default;
            is_finished = false;
            time = default;
            damage = new();
            collisions = 0;
            check_points = new();
        }
    }

    public Result result;

    public PostData()
    {
        id = default;
        type = default;
        result = new Result();
    }
}

public class DroneRequests : MonoBehaviour
{
    public static GetData getData;
    public static PostData postData;
    //public static ApiConfig apiConfig;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //StartCoroutine(LoadApiConfig());
        StartCoroutine(GetData());

        getData = new GetData();

        DroneEventManager.onGameOver.AddListener(SendRequest);
    }

    private void SendRequest()
    {
        StartCoroutine(PostData());
    }

    IEnumerator GetData()
    {
        string id = default;
        string type = default;

        if (QueryParamsManager.queryParams == null)
        {
            Debug.Log("QueryParams is null");

            id = "0";
            type = "0";
        }
        else
        {
            id = Convert.ToString(QueryParamsManager.queryParams.id);
            type = Convert.ToString(QueryParamsManager.queryParams.type);
        }

        Debug.Log("ID: " + id);
        Debug.Log("TYPE: " + type);

        UnityWebRequest www = UnityWebRequest.Get(string.Format("https://umius.ru/wp-content/themes/umius/cabinet/quadcopter/api/get.php?id={0}&type={1}", id, type));

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            getData = new GetData();
            JsonUtility.FromJsonOverwrite(json, getData);

            Debug.Log("GET: " + json);
        }

        DroneEventManager.RequestsDone();
    }

    IEnumerator PostData()
    {
        yield return new WaitForSeconds(1);

        string json = JsonUtility.ToJson(postData);

        UnityWebRequest www = UnityWebRequest.PostWwwForm("https://umius.ru/wp-content/themes/umius/cabinet/quadcopter/api/post.php", json);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("POST: " + www.result);
        }
    }
}
