using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    /// PLAYER CONSTS//
    // controls
    public const string HORIZONTAL_AXIS = "Horizontal";
    public const string VERTICAL_AXIS = "Vertical";
    public const KeyCode MOVE_LEFT = KeyCode.A;
    public const KeyCode MOVE_RIGHT = KeyCode.D;
    public const KeyCode PIN_DROP = KeyCode.Q;
    // vars
    public const float MOVE_SPEED = 20f;
    public const int NEEDLE_COUNT = 3;
    public const float GROUND_RADIUS = 8f;

    /// TREE CONSTS ///
    public const float TREE_NUTRIENT_CHECK_TIMER = .5f;
    public const int LEAF_COUNT = 3;
    public const float NUTRIENT_ABSORB_TIME = 1;
    public const float NUTRIENT_MAKE_TIMER = 2;
    public const float NUTRIENT_CREATE_TIMER = .5f;
    public const float NUTRIENT_SIZE = .5f;
    public const float BUG_START_DIST = 3f;

    /// TIME CONSTS ///
    public const float DAY_LENGTH = 20f;
    public const float BUG_DROP_TIME = (3f * DAY_LENGTH / 4f);
    public const float BUG_SPAWN_TIME = (DAY_LENGTH / 2f);
    public const float BUG_EAT_TIME = DAY_LENGTH / 4f;
    public const float BUG_DIE_TIME = DAY_LENGTH / 4f;

    /// BUG CONSTS ///
    public static Dictionary<bugColor, Color> BUG_COLOR_LOOKUP = new Dictionary<bugColor, Color>() {
        { bugColor.BLUE, Color.blue },
        { bugColor.RED, Color.red },
        { bugColor.YELLOW, Color.yellow}
    };
}
