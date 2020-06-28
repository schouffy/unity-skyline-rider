using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.blackwhite_side_scroller.Scripts
{
    public class Constants
    {
        public static String TagPlayer => "Player";
        public static String TagGameController => "GameController";
        public static String TagMainCamera => "MainCamera";

        public static GameObject Player => GameObject.FindWithTag(TagPlayer);
        public static GameController GameController => GameObject.FindWithTag(TagGameController).GetComponent<GameController>();
        public static GameObject MainCamera => GameObject.FindWithTag(TagMainCamera);
    }
}
