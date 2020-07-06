using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{
    public GameObject Player;
    public LevelLoader LevelLoader;
    public AudioClip DeadGuyHitsGroundSound;
    public AudioClip DramaticMusic;
    public AudioClip RollingTinCanSound;
    public AudioClip ElevatorButtonSound;
    public AudioClip ElevatorOpensSound;
    public AudioClip ElevatorClosesSound;
    public AudioSource CarsSounds;
    public Text UIHoldSpaceToSkip;


    private float? _spacePressedSinceTime;
    private float? _promptLastRequestAtTime;
    private Color _uiColor;

    private void Start()
    {
        _uiColor = UIHoldSpaceToSkip.color;
    }

    private void LateUpdate()
    {
        if (Input.anyKeyDown)
        {
            UIHoldSpaceToSkip.color = new Color(_uiColor.r, _uiColor.g, _uiColor.b, 1);
            _promptLastRequestAtTime = Time.time;

            if (Input.GetButtonDown("Jump"))
            {
                _spacePressedSinceTime = Time.time;
            }
        }
        if (!Input.GetButton("Jump"))
        {
            _spacePressedSinceTime = null;
        }
        if (!Input.anyKey)
        {
            if (Time.time > _promptLastRequestAtTime + 0.5f)
                UIHoldSpaceToSkip.color = new Color(_uiColor.r, _uiColor.g, _uiColor.b, 0);
        }
        if (_spacePressedSinceTime.HasValue && Time.time - _spacePressedSinceTime > 2f)
        {
            LevelEnds();
        }
    }

    public void CameraApproachingStreet()
    {
        CarsSounds.volume = 0.2f;
    }

    public void DeadGuyHitsGround()
    {
        GetComponent<AudioSource>().PlayOneShot(DeadGuyHitsGroundSound);
        MusicPlayer.instance.PlayMusic(DramaticMusic);
    }

    public void PlayerReactFromDeadGuy()
    {
        Player.GetComponent<Animator>().SetFloat("AnimSpeed", 1);
    }

    public void TinCanRollsToPlayer()
    {
        GetComponent<AudioSource>().PlayOneShot(RollingTinCanSound);
    }

    public void PlayerLooksAtDeadGuy()
    {
        Player.GetComponent<Animator>().SetFloat("AnimSpeed", 1);
    }

    public void PlayerStandupAndCallsElevator()
    {
        Player.GetComponent<Animator>().SetFloat("AnimSpeed", 1);
    }

    public void PlayerPushesElevatorButton()
    {
        GetComponent<AudioSource>().PlayOneShot(ElevatorButtonSound);
    }

    public void ElevatorOpens()
    {
        GetComponent<AudioSource>().PlayOneShot(ElevatorOpensSound);
    }

    public void PlayerEntersElevator()
    {
        Player.GetComponent<Animator>().SetFloat("AnimSpeed", 1);
    }

    public void ElevatorCloses()
    {
        GetComponent<AudioSource>().PlayOneShot(ElevatorClosesSound);
    }

    public void LevelEnds()
    {
        LevelLoader.LoadNextLevel();
    }
}
