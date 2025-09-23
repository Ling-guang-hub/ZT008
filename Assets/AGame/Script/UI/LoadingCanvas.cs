using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingCanvas :MonoBehaviour
{
    public Text ProgressText;
    public Image ProgressImage;
    private float m_Progress;
    private float timeTatal = 3f;
    private float profress;
    private void Start()
    {
        m_Progress = 0;
        timeTatal = 3f;
        ProgressText.text = "0%";
        ProgressImage.fillAmount = 0;
    }
    
    private void Update()
    {
        m_Progress += Time.deltaTime;
        profress = m_Progress / timeTatal;
        if (profress < 0.9f)
        {
            ProgressImage.fillAmount = profress;
            ProgressText.text = (int)(profress * 100) + "%";
        }
        
        if (profress >= 1 && A_RodDumpBee.instance.Fatal)
        {
            ProgressImage.fillAmount = 1f;
            ProgressText.text = 100 + "%";
            A_DrenchDoom.AtomVenus();
            SceneManager.LoadScene("Base");
            // if (CommonUtil.IsApple())
            // {
            //     
            //     SceneManager.LoadScene("AGame");
            // }
            // else
            // {
            //     
            // }
        }
    }
}