using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enumerations
{

    public enum GAME_STATE
    {
        MAIN_MENU,
        PRE_GAME,
        IN_GAME,
        POST_GAME,
        GAME_LOST,
        GAME_WON,
        QUIT
    }

    public enum MAP_CELL_TYPE
    {
        ROAD,
        WALL,
        OBSTACLE,
        BUILDING
    }

    public enum UNIT_FACTION
    {
        PLAYER,
        ENEMY
    }

    public enum UNIT_TYPE
    {
        TANK,
        BASE
    }


    public enum UI_VIEW
    {
        MAIN_MENU
    }
    
}
