﻿using System.Collections;
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
	public const float MOVE_SPEED = .2f; 
	public const int NEEDLE_COUNT = 3;

	/// TREE CONSTS ///
	public const float TREE_NUTRIENT_CHECK_TIMER = .5f; 
	public const int LEAF_COUNT = 3;
	public const float NUTRIENT_ABSORB_TIME = 1;
	public const float NUTRIENT_MAKE_TIMER = 2;
	public const float NUTRIENT_CREATE_TIMER = .5f;
	public const float NUTRIENT_SIZE = 1;
}
