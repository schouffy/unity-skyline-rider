using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
