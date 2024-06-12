using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

[CreateAssetMenu(menuName = "Singletons/MasterManager")]

public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField] private GameSetting _gameSettings;
    public static GameSetting GameSettings { get { return Instance._gameSettings; } }

    private List<NetworkedPrefab> _networkedPrefabs = new List<NetworkedPrefab>();

    public static GameObject NetworkInstantiate(GameObject obj, Vector3 pos, Quaternion rot)
    {
        foreach (NetworkedPrefab networkedPrefab in Instance._networkedPrefabs)
        {
            if (networkedPrefab.Prefab == obj)
            {
                if (networkedPrefab.Path != string.Empty)
                {
                    GameObject result = PhotonNetwork.Instantiate(networkedPrefab.Path, pos, rot);
                    return result;
                }
                else
                {
                    Debug.LogError("Path is empty for gameobject name: " + networkedPrefab.Prefab);
                    return null;
                }
            }
        }
        return null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    private static void PopulateNetworkedPrefabs()
    {
        if (!Application.isEditor)
        {
            return;
        }

        GameObject[] results = Resources.LoadAll<GameObject>("");
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].GetComponent<PhotonView>() != null)
            {
                string path = AssetDatabase.GetAssetPath(results[i]);
                Instance._networkedPrefabs.Add(new NetworkedPrefab(results[i], path));
            }
        }

        for (int i = 0; i < Instance._networkedPrefabs.Count; i++)
        {
            UnityEngine.Debug.Log(Instance._networkedPrefabs[i].Prefab.name + " | " + Instance._networkedPrefabs[i].Path);
        }
    }
}
