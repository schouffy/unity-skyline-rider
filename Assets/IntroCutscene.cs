using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    public GameObject Player;
    public LevelLoader LevelLoader;

    public void PlayerReactFromDeadGuy()
    {
        Player.GetComponent<Animator>().SetFloat("AnimSpeed", 1);
    }

    public void PlayerLooksAtDeadGuy()
    {
        Player.GetComponent<Animator>().SetFloat("AnimSpeed", 1);
    }

    public void PlayerStandupAndCallsElevator()
    {
        Player.GetComponent<Animator>().SetFloat("AnimSpeed", 1);
    }

    public void ElevatorOpens()
    {

    }

    public void PlayerEntersElevator()
    {
        Debug.Log("PlayerEntersElevator");
        Player.GetComponent<Animator>().SetFloat("AnimSpeed", 1);
    }

    public void ElevatorCloses()
    {

    }

    public void LevelEnds()
    {
        LevelLoader.LoadNextLevel();
    }
}
