Blockly.defineBlocksWithJsonArray([
  {
    "type": "bve_elapse",
    "style": "bve_blocks",
    "message0": "%{BKY_BVE_ELAPSE}",
    "nextStatement": null
  },
  {
    "type": "bve_vehicle_spec",
    "style": "bve_blocks",
    "message0": "%{BKY_BVE_VEHICLE_SPEC}",
    "args0": [
      {
        "type": "field_dropdown",
        "name": "spec_type",
        "options": [
          ["%{BKY_BVE_VSPEC_BRAKE}", "brake_notches"],
          ["%{BKY_BVE_VSPEC_POWER}", "power_notches"],
          ["%{BKY_BVE_VSPEC_ATS}", "ats_notch"],
          ["%{BKY_BVE_VSPEC_B67}", "b67_notch"],
          ["%{BKY_BVE_VSPEC_CAR}", "cars"]
        ]
      }
    ],
    "output": "Number"
  },
  {
    "type": "bve_location",
    "style": "bve_blocks",
    "message0": "%{BKY_BVE_LOCATION}",
    "output": "Number"
  },
  {
    "type": "bve_speed",
    "style": "bve_blocks",
    "message0": "%{BKY_BVE_SPEED}",
    "output": "Number"
  },
  {
    "type": "bve_time",
    "style": "bve_blocks",
    "message0": "%{BKY_BVE_TIME}",
    "output": "Number"
  },
  {
    "type": "bve_vehicle_state",
    "style": "bve_blocks",
    "message0": "%{BKY_BVE_VEHICLE_STATE}",
    "args0": [
      {
        "type": "field_dropdown",
        "name": "spec_type",
        "options": [
          ["%{BKY_BVE_VSTATE_BC}", "bc_pressure"],
          ["%{BKY_BVE_VSTATE_MR}", "mr_pressure"],
          ["%{BKY_BVE_VSTATE_ER}", "er_pressure"],
          ["%{BKY_BVE_VSTATE_BP}", "bp_pressure"],
          ["%{BKY_BVE_VSTATE_SAP}", "sap_pressure"],
          ["%{BKY_BVE_VSTATE_AMP}", "current"]
        ]
      }
    ],
    "output": "Number"
  },
  {
    "type": "bve_get_handle",
    "style": "bve_blocks",
    "message0": "%{BKY_BVE_GET_HANDLE}",
    "args0": [
      {
        "type": "field_dropdown",
        "name": "handle_type",
        "options": [
          ["%{BKY_BVE_HND_BRAKE}", "brake"],
          ["%{BKY_BVE_HND_POWER}", "power"],
          ["%{BKY_BVE_HND_REVERSER}", "reverser"],
          ["%{BKY_BVE_HND_CONSTSPD}", "const_speed"]
        ]
      }
    ],
    "output": "Number"
  },
  {
    "type": "bve_set_handle",
    "style": "bve_blocks",
    "message0": "%{BKY_BVE_SET_HANDLE}",
    "args0": [
      {
        "type": "field_dropdown",
        "name": "handle_type",
        "options": [
          ["%{BKY_BVE_HND_BRAKE}", "brake"],
          ["%{BKY_BVE_HND_POWER}", "power"],
          ["%{BKY_BVE_HND_REVERSER}", "reverser"],
          ["%{BKY_BVE_HND_CONSTSPD}", "const_speed"]
        ]
      },
      {
        "type": "input_value",
        "name": "NAME",
        "check": "Number"
      }
    ],
    "previousStatement": null,
    "nextStatement": null
  },
]);