using UnityEngine;

public class DestroySoundOnComplete : MonoBehaviour
{
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        if (_source == null) Destroy(gameObject);
    }

    private void Update()
    {
        if (!_source.isPlaying) Destroy(gameObject);
    }
}
