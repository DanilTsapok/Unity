using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void ChangeSceneState(){
        if (sceneName == "PauseScene")
        {
            PlayerPrefs.SetFloat("Hero_PosX_Slot" + 0, UnitRoot.Instance.transform.position.x);
            PlayerPrefs.SetFloat("Hero_PosY_Slot" + 0, UnitRoot.Instance.transform.position.y);
            PlayerPrefs.SetFloat("Hero_PosZ_Slot" + 0, UnitRoot.Instance.transform.position.z);
            UnitRoot.Instance.isPaused = true;
            UnitRoot.Instance.rb.simulated = false;
        }
        SceneManager.LoadScene(sceneName);
     }

    public void disabledPause(string scene)
    {

        float posX = PlayerPrefs.GetFloat("Hero_PosX_Slot" + 0);
        float posY = PlayerPrefs.GetFloat("Hero_PosY_Slot" + 0);
        float posZ = PlayerPrefs.GetFloat("Hero_PosZ_Slot" + 0);
        if (UnitRoot.Instance != null)
        {
            UnitRoot.Instance.transform.position = new Vector3(posX, posY, posZ);
            UnitRoot.Instance.isPaused = false;
            UnitRoot.Instance.rb.simulated = true;
            UnitRoot.Instance.LoadKeyBindings();
            //UnitRoot.Instance.UpdateHealthUI();
        }
        SceneManager.LoadScene(scene);

    }


}
