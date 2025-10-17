using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool PlayerInRange => _detectedPlayer != null;

    private float _range;

    private Player _detectedPlayer;

    LayerMask _playerLayerMask;

    Color _gizmoIdleColor = Color.yellow;
    Color _gizmoDetectedColor = Color.red;

    private void Start()
    {
        _playerLayerMask = LayerMask.GetMask("Player");
    }
    private void Update()
    {
       DetectInRange();
    }
    private void DetectInRange()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, _range, _playerLayerMask);
        
        if (collider != null)
        {
            _detectedPlayer = collider.GetComponent<Player>();
        }
        else if(_detectedPlayer != null)
        {
            _detectedPlayer = null;
        }
    }

    public GameObject GetPlayer()
    {
        return _detectedPlayer.gameObject;
    }

    public bool SearchInRange(float range)
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, range, _playerLayerMask);

        if (collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetRange(float range)
    {
        _range = range;
    }
    public Vector3 GetPlayerPosition()
    {
        return _detectedPlayer?.transform.position ?? Vector3.zero; //if detected player is null return (0,0,0), otherwise return players position
    }
    private void OnDrawGizmos()
    {
       if(!PlayerInRange)
        {
            Gizmos.color = _gizmoIdleColor;
        }
        else
        {
            Gizmos.color = _gizmoDetectedColor;
        }

       Gizmos.DrawWireSphere(transform.position, _range);
    }
}
