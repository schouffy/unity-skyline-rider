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

        public static GameController GameController => GameObject.FindWithTag(TagGameController).GetComponent<GameController>();
    }
}
