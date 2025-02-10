using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatSO : ScriptableObject
{
	[SerializeField]
	private float _pipeValue;
    public float PipeValue
	{
		get { return _pipeValue; }
		set { _pipeValue = value; }
	}

    [SerializeField]
    private float _playerSpeed;
    public float PlayerSpeed
    {
        get { return _playerSpeed; }
        set { _playerSpeed = value; }
    }

    [SerializeField]
    private float _money;
    public float Money
    {
        get { return _money; }
        set { _money = value; }
    }

    [SerializeField]
    private bool _camPurchased;
    public bool CamPurchased
    {
        get { return _camPurchased; }
        set { _camPurchased = value; }
    }

    [SerializeField]
    private bool _stealthPurchased;
    public bool StealthPurchased
    {
        get { return _stealthPurchased; }
        set { _stealthPurchased = value; }
    }

}
