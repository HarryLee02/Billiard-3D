using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Singletons/MasterManager")]

public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField] private GameSettings _gameSettings;
    public static GameSettings GameSettings { get { return Instance._gameSettings; } }

    private List<NetworkedPrefab> _networkedPrefabs = new List<NetworkedPrefab>();

    [SerializeField] private Ball ball;

    public static GameObject NetWorkInstantiate(GameObject obj, Vector3 position, Quaternion rotation)
    {
        foreach (NetworkedPrefab networkedPrefab in Instance._networkedPrefabs)
        {
            if (networkedPrefab.Prefab == obj)
            {
                if (networkedPrefab.Path != string.Empty)
                {
                    GameObject result = PhotonNetwork.Instantiate(networkedPrefab.Path, position, rotation);
                    return result;
                }
                else
                {
                    Debug.LogError("Path was empty for game object: " + networkedPrefab.Prefab);
                    return null;
                }
            }
        }
        return null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void PopulateNetworkedPrefabs()
    {
#if UNITY_EDITOR
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
#endif
    }
}
