using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassPicker : MonoBehaviour
{
    [SerializeField] private List<ClassSO> _classSo;

    private ClassSO _choosed;

    public void ChooseClass(int classNum)
    {
        _choosed = _classSo[classNum];
        StaticData.classSO = _choosed;
        SceneManager.LoadSceneAsync("Scenes/Hub");
        GameObject.Find("SoundManager").GetComponent<AmbientController>().PlayPassiveAmbient();
    }
}
