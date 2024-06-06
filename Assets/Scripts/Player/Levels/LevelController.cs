using UnityEngine;

public class LevelController : MonoBehaviour
{
      public int _level = 1;
      public int _exp;
      
      private const int XP_FOR_NEXT_LEVEL = 100;
      
      public static LevelController instance;
      private void Awake()
      {
            instance = this;
      }
      
      public void AddExperience(int amount)
      {
            _exp += amount;
            
            Debug.Log(IsReadyForNextLevel());
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
      }

      private bool IsReadyForNextLevel()
      {
            return _exp >= XP_FOR_NEXT_LEVEL * _level;
      }
}