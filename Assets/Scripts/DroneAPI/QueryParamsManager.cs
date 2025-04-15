using System.Runtime.InteropServices;
using System;
using UnityEngine;

public class QueryParamsManager : MonoBehaviour
{
    [Serializable]
    public class QueryParams
    {
        public int id;
        public int type;

        public QueryParams()
        {
            id = 0;
            type = 0;
        }
    }

    public static QueryParams queryParams;

    [DllImport("__Internal")]
    private static extern int GetQueryParams();

    void Start()
    {
        queryParams = new QueryParams();

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            int jsonPtr = GetQueryParams();
            string jsonString = Marshal.PtrToStringAnsi((IntPtr)jsonPtr);

            Debug.Log("Query Parameters: " + jsonString);

            JsonUtility.FromJsonOverwrite(jsonString, queryParams);
        }
    }
}