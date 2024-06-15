using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
      public int _level = 1;
      public int _exp;
      
      private const int XP_FOR_NEXT_LEVEL = 100;

      [SerializeField] private AudioClip lvlUPSound;
      
      public static LevelController instance;
      private void Awake()
      {
            instance = this;

            if (StaticData.userData != null)
            {
                  StaticData.lvl = StaticData.userData.level;
                  StaticData.exp = StaticData.userData.exp;
            }
            
            if (StaticData.lvl != 0)
            {
                  _level = StaticData.lvl;
                  _exp = StaticData.exp;
            }
            
            GameObject.Find("Level").GetComponentInChildren<TMP_Text>().text = _level.ToString();
      }
      
      public void AddExperience(int amount)
      {
            _exp += amount;
            
            if (IsReadyForNextLevel() == true)
            {
                  UpdateLevel();
            }
      }

      private void UpdateLevel()
      {
            _level++;
            if (_exp == XP_FOR_NEXT_LEVEL)
                  _exp = 0;
            else
                  _exp = _exp - XP_FOR_NEXT_LEVEL;
            
            AudioSource.PlayClipAtPoint(lvlUPSound, gameObject.transform.position);
            NotifyController.instance.AddToQueue("Ваш уровень повышен!", 0f);
            GameObject.Find("Level").GetComponentInChildren<TMP_Text>().text = _level.ToString();
      }

      private bool IsReadyForNextLevel()
      {
            return _exp >= XP_FOR_NEXT_LEVEL * _level;
      }
}