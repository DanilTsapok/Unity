using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMechanism : MonoBehaviour
{
    [SerializeField] private string scene;
    [SerializeField] private string LevelStatus;
    public TMP_Text level1Info;
    public TMP_Text level2Info;
    public TMP_Text level3Info;
    public TMP_Text level4Info;
    public TMP_Text level5Info;
    public TMP_Text level6Info;
    public static LevelMechanism Instance { get; private set; }

  

    void Start()
    {
      
        if (level1Info != null) level1Info.text = PlayerPrefs.GetString("LevelStatus" + 1, "Play");
        else Debug.LogError("level1Info is not assigned!");

        if (level2Info != null) level2Info.text = PlayerPrefs.GetString("LevelStatus" + 2, "Play");
        else Debug.LogError("level2Info is not assigned!");

        if (level3Info != null) level3Info.text = PlayerPrefs.GetString("LevelStatus" + 3, "Play");
        else Debug.LogError("level3Info is not assigned!");

        if (level4Info != null) level4Info.text = PlayerPrefs.GetString("LevelStatus" + 4, "Play");
        else Debug.LogError("level4Info is not assigned!");

        if (level5Info != null) level5Info.text = PlayerPrefs.GetString("LevelStatus" + 5, "Play");
        else Debug.LogError("level5Info is not assigned!");

        if (level6Info != null) level6Info.text = PlayerPrefs.GetString("LevelStatus" + 6, "Play");
        else Debug.LogError("level6Info is not assigned!");
    }

    public void Level(int level)
    {
        if (UnitRoot.Instance == null)
        {
            Debug.LogError("UnitRoot.Instance is null!");
            return;
        }
        switch (level)
        {
            case 1:
                UnitRoot.Instance.transform.position = new Vector3(108, 10, 0);
                break;
            case 2:
                UnitRoot.Instance.transform.position = new Vector3(-34, 2, 1);
                break;
            case 3:
                UnitRoot.Instance.transform.position = new Vector3(5, -6.25f, 0);
                break;
            case 4:
                UnitRoot.Instance.transform.position = new Vector3(-31.32f, -10.72f, 0);
                break;
            case 5:
                UnitRoot.Instance.transform.position = new Vector3(-52.04f, -7.84f, 0);
                break;
            case 6:
                UnitRoot.Instance.transform.position = new Vector3(-58.47f, 3.21f, 0);
                break;
            default:
                Debug.LogWarning("Invalid level number: " + level);
                return;
        }

        if (!string.IsNullOrEmpty(scene))
        {
            SceneManager.LoadScene(scene);
        }
        else
        {
            Debug.LogError("Scene name is not assigned!");
        }
    }

    public void ChangeScene(string scene)
    {
  
        if (!string.IsNullOrEmpty(scene))
        {
            SceneManager.LoadScene(scene);
        }
        else
        {
            Debug.LogError("Scene name is not assigned!");
        }
    }
}
