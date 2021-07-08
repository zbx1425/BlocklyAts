Blockly.defineBlocksWithJsonArray([

  // Category: BVE Hats
  {
    type: "bve_hat_elapse",
    style: "bve_blocks",
    message0: "%{BKY_BVE_ELAPSE}",
    nextStatement: null,
  },
  {
    type: "bve_hat_initialize",
    style: "bve_blocks",
    message0: "%{BKY_BVE_INITIALIZE}",
    nextStatement: null,
  },
  {
    type: "bve_hat_keydown_any",
    style: "bve_blocks",
    message0: "%{BKY_BVE_KEYDOWN_ANY}",
    nextStatement: null,
  },
  {
    type: "bve_hat_keyup_any",
    style: "bve_blocks",
    message0: "%{BKY_BVE_KEYUP_ANY}",
    nextStatement: null,
  },
  {
    type: "bve_hat_horn_blow",
    style: "bve_blocks",
    message0: "%{BKY_BVE_HORN_BLOW}",
    nextStatement: null,
  },
  {
    type: "bve_hat_door_change",
    style: "bve_blocks",
    message0: "%{BKY_BVE_DOOR_CHANGE}",
    nextStatement: null,
  },
  {
    type: "bve_hat_set_signal",
    style: "bve_blocks",
    message0: "%{BKY_BVE_SET_SIGNAL}",
    nextStatement: null,
  },
  {
    type: "bve_hat_set_beacon",
    style: "bve_blocks",
    message0: "%{BKY_BVE_SET_BEACON}",
    nextStatement: null,
  },
  {
    type: "bve_hat_load",
    style: "bve_blocks",
    message0: "%{BKY_BVE_LOAD}",
    nextStatement: null,
  },
  {
    type: "bve_hat_dispose",
    style: "bve_blocks",
    message0: "%{BKY_BVE_DISPOSE}",
    nextStatement: null,
  },

  // Category: BVE General
  {
    type: "bve_vehicle_spec",
    style: "bve_blocks",
    message0: "%{BKY_BVE_VEHICLE_SPEC}",
    args0: [
      {
        type: "field_dropdown",
        name: "FIELD_SEL",
        options: [
          ["%{BKY_BVE_VSPEC_BRAKE}", "BrakeNotches"],
          ["%{BKY_BVE_VSPEC_POWER}", "PowerNotches"],
          ["%{BKY_BVE_VSPEC_ATS}", "AtsNotch"],
          ["%{BKY_BVE_VSPEC_B67}", "B67Notch"],
          ["%{BKY_BVE_VSPEC_CAR}", "Cars"],
        ],
      },
    ],
    output: "Number",
  },
  {
    type: "bve_location",
    style: "bve_blocks",
    message0: "%{BKY_BVE_LOCATION}",
    output: "Number",
  },
  {
    type: "bve_speed",
    style: "bve_blocks",
    message0: "%{BKY_BVE_SPEED}",
    output: "Number",
  },
  {
    type: "bve_time",
    style: "bve_blocks",
    message0: "%{BKY_BVE_TIME}",
    output: "Number",
  },
  {
    type: "bve_vehicle_state",
    style: "bve_blocks",
    message0: "%{BKY_BVE_VEHICLE_STATE}",
    args0: [
      {
        type: "field_dropdown",
        name: "FIELD_SEL",
        options: [
          ["%{BKY_BVE_VSTATE_BC}", "BcPressure"],
          ["%{BKY_BVE_VSTATE_MR}", "MrPressure"],
          ["%{BKY_BVE_VSTATE_ER}", "ErPressure"],
          ["%{BKY_BVE_VSTATE_BP}", "BpPressure"],
          ["%{BKY_BVE_VSTATE_SAP}", "SapPressure"],
          ["%{BKY_BVE_VSTATE_AMP}", "Current"],
        ],
      },
    ],
    output: "Number",
  },
  {
    type: "bve_get_handle",
    style: "bve_blocks",
    message0: "%{BKY_BVE_GET_HANDLE}",
    args0: [
      {
        type: "field_dropdown",
        name: "FIELD_SEL",
        options: [
          ["%{BKY_BVE_HND_BRAKE}", "Brake"],
          ["%{BKY_BVE_HND_POWER}", "Power"],
          ["%{BKY_BVE_HND_REVERSER}", "Reverser"],
          ["%{BKY_BVE_HND_CONSTSPD}", "ConstSpeed"],
        ],
      },
    ],
    output: "Number",
  },
  {
    type: "bve_set_handle",
    style: "bve_blocks",
    message0: "%{BKY_BVE_SET_HANDLE}",
    args0: [
      {
        type: "field_dropdown",
        name: "FIELD_SEL",
        options: [
          ["%{BKY_BVE_HND_BRAKE}", "Brake"],
          ["%{BKY_BVE_HND_POWER}", "Power"],
          ["%{BKY_BVE_HND_REVERSER}", "Reverser"],
          ["%{BKY_BVE_HND_CONSTSPD}", "ConstSpeed"],
        ],
      },
      {
        type: "input_value",
        name: "VALUE",
        check: "Number",
      },
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_sound_stop",
    style: "bve_blocks",
    message0: "%{BKY_BVE_SOUND_STOP}",
    args0: [
      {
        type: "input_value",
        name: "ID",
        check: "Number",
      },
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_sound_play_once",
    style: "bve_blocks",
    message0: "%{BKY_BVE_SOUND_PLAY_ONCE}",
    args0: [
      {
        type: "input_value",
        name: "ID",
        check: "Number",
      },
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_sound_play_loop",
    style: "bve_blocks",
    message0: "%{BKY_BVE_SOUND_PLAY_LOOP}",
    args0: [
      {
        type: "input_value",
        name: "ID",
        check: "Number",
      },
      {
        type: "input_value",
        name: "VOLUME",
        check: "Number"
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_get_sound_internal",
    style: "bve_blocks",
    message0: "%{BKY_BVE_GET_SOUND_INTERNAL}",
    args0: [
      {
        type: "input_value",
        name: "ID",
        check: "Number",
      },
    ],
    output: "Number",
  },
  {
    type: "bve_set_sound_internal",
    style: "bve_blocks",
    message0: "%{BKY_BVE_SET_SOUND_INTERNAL}",
    args0: [
      {
        type: "input_value",
        name: "ID",
        check: "Number",
      },
      {
        type: "input_value",
        name: "INTERNAL_VAL",
        check: "Number"
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_set_panel",
    style: "bve_blocks",
    message0: "%{BKY_BVE_SET_PANEL}",
    args0: [
      {
        type: "input_value",
        name: "ID",
        check: "Number",
      },
      {
        type: "input_value",
        name: "VALUE",
        check: "Number"
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_get_panel",
    style: "bve_blocks",
    message0: "%{BKY_BVE_GET_PANEL}",
    args0: [
      {
        type: "input_value",
        name: "ID",
        check: "Number",
      }
    ],
    output: "Number",
  },
  {
    type: "bve_key",
    style: "bve_blocks",
    message0: "%1",
    args0: [
      {
        type: "field_dropdown",
        name: "KEY_TYPE",
        options: [
          ["%{BKY_BVE_ATS_KEY_S}", "S"],
          ["%{BKY_BVE_ATS_KEY_A1}", "A1"],
          ["%{BKY_BVE_ATS_KEY_A2}", "A2"],
          ["%{BKY_BVE_ATS_KEY_B1}", "B1"],
          ["%{BKY_BVE_ATS_KEY_B2}", "B2"],
          ["%{BKY_BVE_ATS_KEY_C1}", "C1"],
          ["%{BKY_BVE_ATS_KEY_C2}", "C2"],
          ["%{BKY_BVE_ATS_KEY_D}", "D"],
          ["%{BKY_BVE_ATS_KEY_E}", "E"],
          ["%{BKY_BVE_ATS_KEY_F}", "F"],
          ["%{BKY_BVE_ATS_KEY_G}", "G"],
          ["%{BKY_BVE_ATS_KEY_H}", "H"],
          ["%{BKY_BVE_ATS_KEY_I}", "I"],
          ["%{BKY_BVE_ATS_KEY_J}", "J"],
          ["%{BKY_BVE_ATS_KEY_K}", "K"],
          ["%{BKY_BVE_ATS_KEY_L}", "L"],
        ],
      },
    ],
    output: "Number",
  },
  {
    type: "bve_get_key",
    style: "bve_blocks",
    message0: "%{BKY_BVE_GET_KEY}",
    args0: [
      {
        type: "input_value",
        name: "KEY_TYPE",
        check: "Number"
      }
    ],
    output: "Boolean",
  },
  {
    type: "bve_get_door",
    style: "bve_blocks",
    message0: "%{BKY_BVE_GET_DOOR}",
    output: "Boolean",
  },
  {
    type: "bve_init_mode",
    style: "bve_blocks",
    message0: "%{BKY_BVE_INIT_MODE}",
    output: "Number",
  },
  {
    type: "bve_updown_key_check",
    style: "bve_blocks",
    message0: "%{BKY_BVE_UPDOWN_KEY_CHECK}",
    args0: [
      {
        type: "input_value",
        name: "KEY_TYPE",
        check: "Number"
      }
    ],
    output: "Boolean",
  },
  {
    type: "bve_horn_blew_check",
    style: "bve_blocks",
    message0: "%{BKY_BVE_HORN_BLEW_CHECK}",
    args0: [
      {
        type: "field_dropdown",
        name: "HORN_TYPE",
        options: [
          ["%{BKY_BVE_HORN_PRIMARY}", "Primary"],
          ["%{BKY_BVE_HORN_SECONDARY}", "Secondary"],
          ["%{BKY_BVE_HORN_MUSIC}", "Music"],
        ],
      },
    ],
    output: "Boolean",
  },
  {
    type: "bve_signal_aspect",
    style: "bve_blocks",
    message0: "%{BKY_BVE_BCN_SIGNAL}",
    output: "Number",
  },
  {
    type: "bve_get_beacon",
    style: "bve_blocks",
    message0: "%{BKY_BVE_GET_BEACON}",
    args0: [
      {
        type: "field_dropdown",
        name: "FIELD_SEL",
        options: [
          ["%{BKY_BVE_BCN_TYPE}", "Type"],
          ["%{BKY_BVE_BCN_SIGNAL}", "Signal"],
          ["%{BKY_BVE_BCN_DISTANCE}", "Distance"],
          ["%{BKY_BVE_BCN_OPTIONAL}", "Optional"],
        ],
      },
    ],
    output: "Number",
  },

  // Category: Utilities
  {
    type: "bve_config_load",
    style: "bve_blocks",
    message0: "%{BKY_BVE_CONFIG_LOAD}",
    args0: [
      {
        type: "field_input",
        name: "PATH",
        text: "%{BKY_BVE_PLACEHOLDER_PATH}"
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_config_save",
    style: "bve_blocks",
    message0: "%{BKY_BVE_CONFIG_SAVE}",
    args0: [
      {
        type: "field_input",
        name: "PATH",
        text: "%{BKY_BVE_PLACEHOLDER_PATH}"
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_get_config",
    style: "bve_blocks",
    message0: "%{BKY_BVE_GET_CONFIG}",
    args0: [
      {
        type: "field_input",
        name: "PART",
        text: "%{BKY_BVE_PLACEHOLDER_PART}"
      },
      {
        type: "field_input",
        name: "KEY",
        text: "%{BKY_BVE_PLACEHOLDER_KEY}"
      }
    ],
    output: [ "String", "Number" ],
  },
  {
    type: "bve_get_config_default",
    style: "bve_blocks",
    message0: "%{BKY_BVE_GET_CONFIG_DEFAULT}",
    args0: [
      {
        type: "field_input",
        name: "PART",
        text: "%{BKY_BVE_PLACEHOLDER_PART}"
      },
      {
        type: "field_input",
        name: "KEY",
        text: "%{BKY_BVE_PLACEHOLDER_KEY}"
      },
      {
        type: "input_value",
        name: "DEFAULT_VAL",
        check: [ "String", "Number" ],
      }
    ],
    output: [ "String", "Number" ],
  },
  {
    type: "bve_set_config",
    style: "bve_blocks",
    message0: "%{BKY_BVE_SET_CONFIG}",
    args0: [
      {
        type: "field_input",
        name: "PART",
        text: "%{BKY_BVE_PLACEHOLDER_PART}"
      },
      {
        type: "field_input",
        name: "KEY",
        text: "%{BKY_BVE_PLACEHOLDER_KEY}"
      },
      {
        type: "input_value",
        name: "VALUE",
        check: [ "String", "Number" ],
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_msgbox",
    style: "bve_blocks",
    message0: "%{BKY_BVE_MSGBOX}",
    args0: [
      {
        type: "input_value",
        name: "MSG",
        check: "String",
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_exception",
    style: "bve_blocks",
    message0: "%{BKY_BVE_EXCEPTION}",
    args0: [
      {
        type: "input_value",
        name: "MSG",
        check: "String",
      }
    ],
    previousStatement: null,
  },
  {
    type: "bve_hat_timer",
    style: "bve_blocks",
    message0: "%{BKY_BVE_TIMER_TRIGGER}",
    args0: [
      {
        type: "field_input",
        name: "NAME",
        text: "%{BKY_BVE_PLACEHOLDER_TIMER}"
      }
    ],
    nextStatement: null,
  },
  {
    type: "bve_timer_set",
    style: "bve_blocks",
    message0: "%{BKY_BVE_TIMER_SET}",
    args0: [
      {
        type: "field_input",
        name: "NAME",
        text: "%{BKY_BVE_PLACEHOLDER_TIMER}"
      },
      {
        type: "input_value",
        name: "INTERVAL",
        check: "Number",
      },
      {
        type: "input_value",
        name: "CYCLE",
        check: "Boolean",
      },
      {
        type: "input_dummy"
      }
    ],
    inputsInline: false,
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_timer_modify",
    style: "bve_blocks",
    message0: "%{BKY_BVE_TIMER_MODIFY}",
    args0: [
      {
        type: "field_input",
        name: "NAME",
        text: "%{BKY_BVE_PLACEHOLDER_TIMER}"
      },
      {
        type: "field_dropdown",
        name: "OPERATION",
        options: [
          ["%{BKY_BVE_TMRMOD_STOP}", "Stop"],
          ["%{BKY_BVE_TMRMOD_TRIGSTOP}", "TrigStop"],
          ["%{BKY_BVE_TMRMOD_RESET}", "Reset"],
          ["%{BKY_BVE_TMRMOD_TRIGRESET}", "TrigReset"],
        ],
      },
    ],
    previousStatement: null,
    nextStatement: null,
  },

  // Category: OpenBVE
  {
    type: "obve_preceding_vehicle",
    style: "openbve_blocks",
    message0: "%{BKY_OBVE_PRECEDING_VEHICLE}",
    args0: [
      {
        type: "field_dropdown",
        name: "FIELD",
        options: [
          ["%{BKY_OBVE_PRETRAIN_EXIST}", "Exists"],
          ["%{BKY_OBVE_PRETRAIN_LOCATION}", "Location"],
          ["%{BKY_OBVE_PRETRAIN_DISTANCE}", "Distance"],
          ["%{BKY_OBVE_PRETRAIN_SPEED}", "Speed"],
        ]
      }
    ],
    output: "Number",
  },
  {
    type: "obve_next_station",
    style: "openbve_blocks",
    message0: "%{BKY_OBVE_NEXT_STATION}",
    args0: [
      {
        type: "field_dropdown",
        name: "STOP",
        options: [
          ["%{BKY_OBVE_NEXTSTA_ANY}", "false"],
          ["%{BKY_OBVE_NEXTSTA_STOP}", "true"],
        ]
      },
      {
        type: "field_dropdown",
        name: "FIELD",
        options: [
          ["%{BKY_OBVE_NEXTSTA_EXIST}", "Exists"],
          ["%{BKY_OBVE_NEXTSTA_TYPE}", "Type"],
          ["%{BKY_OBVE_NEXTSTA_STARTPOS}", "DefaultTrackPosition"],
          ["%{BKY_OBVE_NEXTSTA_STOPPOS}", "StopPosition"],
          ["%{BKY_OBVE_NEXTSTA_ARRTIME}", "ArrivalTime"],
          ["%{BKY_OBVE_NEXTSTA_DEPTIME}", "DepartureTime"],
          ["%{BKY_OBVE_NEXTSTA_STOPTIME}", "StopTime"],
          ["%{BKY_OBVE_NEXTSTA_OPENLEFT}", "OpenLeftDoors"],
          ["%{BKY_OBVE_NEXTSTA_OPENRIGHT}", "OpenRightDoors"],
          ["%{BKY_OBVE_NEXTSTA_FORCESTOP}", "ForceStopSignal"],
        ]
      }
    ],
    output: "Number",
  },
  {
    type: "obve_destination",
    style: "openbve_blocks",
    message0: "%{BKY_OBVE_DESTINATION}",
    output: "Number",
  },
  {
    type: "obve_langcode",
    style: "openbve_blocks",
    message0: "%{BKY_OBVE_LANGCODE}",
    output: "String",
  },
  {
    type: "obve_get_door",
    style: "openbve_blocks",
    message0: "%{BKY_OBVE_GET_DOOR}",
    args0: [
      {
        type: "field_dropdown",
        name: "DOOR",
        options: [
          ["%{BKY_OBVE_DOOR_LEFT}", "Left"],
          ["%{BKY_OBVE_DOOR_RIGHT}", "Right"],
          ["%{BKY_OBVE_DOOR_BOTH}", "Both"],
          ["%{BKY_OBVE_DOOR_ANY}", "Any"],
        ]
      },
    ],
    output: "Boolean",
  },
  {
    type: "obve_set_door_interlock",
    style: "openbve_blocks",
    message0: "%{BKY_OBVE_SET_DOOR_INTERLOCK}",
    args0: [
      {
        type: "field_dropdown",
        name: "DOOR",
        options: [
          ["%{BKY_OBVE_DOOR_LEFT}", "Left"],
          ["%{BKY_OBVE_DOOR_RIGHT}", "Right"],
          ["%{BKY_OBVE_DOOR_BOTH}", "Both"],
        ]
      },
      {
        type: "field_dropdown",
        name: "ACTION",
        options: [
          ["%{BKY_OBVE_DOORIXL_LOCK}", "Lock"],
          ["%{BKY_OBVE_DOORIXL_UNLOCK}", "Unlock"],
        ]
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "obve_sound_play",
    style: "openbve_blocks",
    message0: "%{BKY_OBVE_SOUND_PLAY}",
    args0: [
      {
        type: "input_value",
        name: "ID",
        check: "Number",
      },
      {
        type: "input_value",
        name: "VOLUME",
        check: "Number"
      },
      {
        type: "input_value",
        name: "PITCH",
        check: "Number"
      },
      {
        type: "input_value",
        name: "LOOP",
        check: "Boolean"
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "obve_set_debug_message",
    style: "openbve_blocks",
    message0: "%{BKY_OBVE_SET_DEBUG_MESSAGE}",
    args0: [
      {
        type: "input_value",
        name: "MESSAGE",
        check: "String"
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "obve_show_message",
    style: "openbve_blocks",
    message0: "%{BKY_OBVE_SHOW_MESSAGE}",
    args0: [
      {
        type: "input_value",
        name: "MESSAGE",
        check: "String"
      },
      {
        type: "field_dropdown",
        name: "COLOR",
        options: [
          ["%{BKY_OBVE_COLOR_BLACK}", "Black"],
          ["%{BKY_OBVE_COLOR_GRAY}", "Gray"],
          ["%{BKY_OBVE_COLOR_WHITE}", "White"],
          ["%{BKY_OBVE_COLOR_RED}", "Red"],
          ["%{BKY_OBVE_COLOR_ORANGE}", "Orange"],
          ["%{BKY_OBVE_COLOR_GREEN}", "Green"],
          ["%{BKY_OBVE_COLOR_BLUE}", "Blue"],
          ["%{BKY_OBVE_COLOR_MAGENTA}", "Magenta"],
        ]
      },
      {
        type: "field_number",
        name: "DURATION",
      },
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_hat_door_change_any",
    style: "openbve_blocks",
    message0: "%{BKY_OBVE_DOOR_CHANGE_ANY}",
    nextStatement: null,
  },
  {
    type: "bve_hat_perform_ai",
    style: "openbve_blocks",
    message0: "%{BKY_OBVE_PERFORM_AI}",
    nextStatement: null,
  },

  // Category: Conversions & Misc
  {
    type: "bve_convert_to_double",
    style: "math_blocks",
    message0: "%{BKY_BVE_CONVERT_TO_DOUBLE}",
    args0: [
      {
        type: "input_value",
        name: "SOURCE"
      },
    ],
    output: "Number"
  },
  {
    type: "bve_convert_to_string",
    style: "text_blocks",
    message0: "%{BKY_BVE_CONVERT_TO_STRING}",
    args0: [
      {
        type: "input_value",
        name: "SOURCE"
      },
    ],
    output: "String"
  },
  {
    type: "bve_convert_to_boolean",
    style: "logic_blocks",
    message0: "%{BKY_BVE_CONVERT_TO_BOOLEAN}",
    args0: [
      {
        type: "input_value",
        name: "SOURCE"
      },
    ],
    output: "Boolean"
  },
  {
    type: "bve_can_convert_to",
    style: "logic_blocks",
    message0: "%{BKY_BVE_CAN_CONVERT_TO}",
    args0: [
      {
        type: "input_value",
        name: "SOURCE"
      },
      {
        type: "field_dropdown",
        name: "TYPE",
        options: [
          ["%{BKY_BVE_TYPE_DOUBLE}", "Double"],
          ["%{BKY_BVE_TYPE_BOOLEAN}", "Bool"]
        ]
      }
    ],
    output: "Boolean"
  },
  {
    type: "bve_comment",
    style: "comment_block",
    message0: "%1",
    args0: [
      {
        type: "field_multilinetext",
        name: "COMMENT",
        text: "%{BKY_BVE_PLACEHOLDER_COMMENT}"
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_rawcode_statement",
    style: "openbve_blocks",
    message0: "C# %1",
    args0: [
      {
        type: "field_multilinetext",
        name: "CODE",
        text: "_c.SetHandle(0, 0);"
      }
    ],
    previousStatement: null,
    nextStatement: null,
  },
  {
    type: "bve_rawcode_value",
    style: "openbve_blocks",
    message0: "C# %1",
    args0: [
      {
        type: "field_multilinetext",
        name: "CODE",
        text: "_c.DoorState"
      }
    ],
    output: null
  }
]);
