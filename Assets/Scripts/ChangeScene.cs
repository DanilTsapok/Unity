using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void ChangeSceneState(){
        if (sceneName == "PauseScene")
        {
            UnitRoot.Instance.isPaused = true;
          
        }
    
        SceneManager.LoadScene(sceneName);
     }


}
