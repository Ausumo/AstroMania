using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationEvent : MonoBehaviour
{
    [FormerlySerializedAs("_sounds")]
    [SerializeField] private Sound[] _sounds;

    private int _soundRandom;

    private string _soundName;

    [FormerlySerializedAs("_footRight")]
    [SerializeField]
    private GameObject _footRight;

    [FormerlySerializedAs("_footLeft")]
    [SerializeField]
    private GameObject _footLeft;

    [FormerlySerializedAs("_distance")]
    [SerializeField]
    private float _distance = 0.2f;

    [FormerlySerializedAs("_playerObj")]
    [SerializeField]
    private GameObject _playerObj;

    [FormerlySerializedAs("_footPrint")]
    [SerializeField]
    private GameObject _footPrint;

    [FormerlySerializedAs("_deformer")]
    [SerializeField]
    private FootstepDeformer _deformer;

    /// <summary>
    /// Called from animation event. foot: 0 = right, 1 = left
    /// Plays a random footstep sound and triggers the footstep deformer at the foot world position.
    /// For regular walking footsteps this uses a small default impact so terrain isn't deeply deformed.
    /// Landing/fall impacts are handled by PlayerMovement (uses actual impact force).
    /// </summary>
    public void FootstepsEvent(int foot)
    {
        // pick random sound name safely
        if (_sounds != null && _sounds.Length > 0)
        {
            _soundRandom = Random.Range(0, _sounds.Length);
            _soundName = _sounds[_soundRandom].name;
        }
        else
        {
            _soundName = "footstep"; // fallback name
        }

        // Play sound (keep existing behavior)
        AudioManager.Instance.PlaySound(_soundName);

        // Determine world position for the foot
        Vector3 footPos = transform.position;
        if (foot == 0 && _footRight != null)
            footPos = _footRight.transform.position;
        else if (foot == 1 && _footLeft != null)
            footPos = _footLeft.transform.position;

        // For regular footsteps use a small default impact value so deformation is subtle
        float impactForce = 1f;

        // Trigger deformer at foot position
        if (_deformer != null)
        {
            _deformer.TriggerStepAtPosition(footPos, impactForce);
        }

        // Optional: spawn footprint decal (if assigned)
        if (_footPrint != null)
        {
            // instantiate briefly at foot position aligned to ground normal
            Instantiate(_footPrint, footPos, Quaternion.identity);
        }
    }
}
