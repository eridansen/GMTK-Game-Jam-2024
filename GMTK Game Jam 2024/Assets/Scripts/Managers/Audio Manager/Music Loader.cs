using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoader : MonoBehaviour
{
    [SerializeField]private Sound _music;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusicClip( _music );
    }

    private void OnDestroy()
    {
        AudioManager.Instance.StopMusic();
    }
}
