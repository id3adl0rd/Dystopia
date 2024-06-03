using UnityEngine;

[CreateAssetMenu]
public class ClassSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _uid;
    [SerializeField] private float _hpBoost;
    [SerializeField] private float _speedBoost;

    public string GetName()
    {
        return this._name;
    }
    
    public string GetUID()
    {
        return this._uid;
    }
    
    public float GetHPBoost()
    {
        return this._hpBoost;
    }
    
    public float GetSpeedBoost()
    {
        return this._speedBoost;
    }
}