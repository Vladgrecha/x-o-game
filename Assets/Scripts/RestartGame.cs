using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    private string _mainScene = "SampleScene";

    // Start is called before the first frame update
    public void RestartLevel()
    {
        SceneManager.LoadScene(_mainScene);
    }
}
