if (!String.prototype.startsWith) {
  Object.defineProperty(String.prototype, 'startsWith', {
      value: function(search, rawPos) {
          var pos = rawPos > 0 ? rawPos|0 : 0;
          return this.substring(pos, pos + search.length) === search;
      }
  });
}
function removeItemOnce(arr, value) {
  var index = arr.indexOf(value);
  if (index > -1) {
    arr.splice(index, 1);
  }
  return arr;
}

Blockly.Lua.addReservedWords([
  "__atsapi_dispose",
  "__atsapi_doorchange",
  "__atsapi_doorchange",
  "__atsapi_elapse",
  "__atsapi_hornblow",
  "__atsapi_initialize",
  "__atsapi_keydown",
  "__atsapi_keyup",
  "__atsapi_load",
  "__atsapi_setbeacondata",
  "__atsapi_setsignal",
  "__atsarg_brake",
  "__atsarg_constspeed",
  "__atsarg_distance",
  "__atsarg_doorstate",
  "__atsarg_horntype",
  "__atsarg_initindex",
  "__atsarg_key",
  "__atsarg_optional",
  "__atsarg_power",
  "__atsarg_reverser",
  "__atsarg_signal",
  "__atsarg_signal",
  "__atsarg_type",
  "__atsfnc_panel",
  "__atsfnc_sound",
  "__bve_edBcPressure",
  "__bve_edBpPressure",
  "__bve_edCurrent",
  "__bve_edErPressure",
  "__bve_edLocation",
  "__bve_edMrPressure",
  "__bve_edSapPressure",
  "__bve_edSpeed",
  "__bve_edTime",
  "__bve_hBrake",
  "__bve_hPower",
  "__bve_hReverser",
  "__bve_vsAtsNotch",
  "__bve_vsB67Notch",
  "__bve_vsBrakeNotches",
  "__bve_vsCars",
  "__bve_vsPowerNotches",
].join(","));

function batsExportLua(workspace) {
  Blockly.Lua.init(workspace);

  var allHats = ["bve_hat_elapse", "bve_hat_initialize", "bve_hat_keydown_any", "bve_hat_keyup_any", "bve_hat_horn_blow", 
    "bve_hat_door_change", "bve_hat_set_signal", "bve_hat_set_beacon", "bve_hat_load", "bve_hat_dispose"];
  var code = "__bve_keystate={false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false}\n__bve_doorstate=0\n";
  var blocks = workspace.getTopBlocks(false);
  for (var i = 0, block; block = blocks[i]; i++) {
    if (block.type.startsWith("bve_hat")) {
      removeItemOnce(allHats, block.type);
      code += Blockly.Lua.blockToCode(block, true);
      var nextBlock = block.getNextBlock();
      if (nextBlock) code += Blockly.Lua.blockToCode(nextBlock);
      if (block.type == "bve_hat_elapse") code += "return __atsarg_power, __atsarg_brake, __atsarg_reverser, __atsarg_constspeed\n";
      code += "end\n";
    } else if (block.type == "procedures_defnoreturn" || block.type == "procedures_defreturn") {
      code += Blockly.Lua.blockToCode(block);
    }
  }

  for (var i = 0, hatName; hatName = allHats[i]; i++) {
    code += Blockly.Lua[hatName]();
    if (hatName == "bve_hat_elapse") code += "return __atsarg_power, __atsarg_brake, __atsarg_reverser, __atsarg_constspeed\n";
    code += "end\n";
  }

  return Blockly.Lua.finish(code).trim();
}

Blockly.Lua.bve_hat_elapse=function(block){
  return "function __atsapi_elapse(__atsarg_power, __atsarg_brake, __atsarg_reverser, __atsarg_constspeed)\n";
}
Blockly.Lua.bve_hat_initialize=function(block){
  return "function __atsapi_initialize(__atsarg_initindex)\n";
}
Blockly.Lua.bve_hat_keydown_any=function(block){
  return "function __atsapi_keydown(__atsarg_key)\n__bve_keystate[__atsarg_key+1]=true\n";
}
Blockly.Lua.bve_hat_keyup_any=function(block){
  return "function __atsapi_keyup(__atsarg_key)\n__bve_keystate[__atsarg_key+1]=false\n";
}
Blockly.Lua.bve_hat_horn_blow=function(block){
  return "function __atsapi_hornblow(__atsarg_horntype)\n";
}
Blockly.Lua.bve_hat_door_change=function(block){
  return "function __atsapi_doorchange(__atsarg_doorstate)\n__bve_doorstate=__atsarg_doorstate\n";
}
Blockly.Lua.bve_hat_set_signal=function(block){
  return "function __atsapi_setsignal(__atsarg_signal)\n";
}
Blockly.Lua.bve_hat_set_beacon=function(block){
  return "function __atsapi_setbeacondata(__atsarg_type, __atsarg_signal, __atsarg_distance, __atsarg_optional)\n";
}
Blockly.Lua.bve_hat_load=function(block){
  return "function __atsapi_load()\n";
}
Blockly.Lua.bve_hat_dispose=function(block){
  return "function __atsapi_dispose()\n";
}
Blockly.Lua.bve_vehicle_spec=function(block){
  return ["__bve_vs"+block.getFieldValue("FIELD_SEL"), Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_location=function(block){
  return ["__bve_edLocation", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_speed=function(block){
  return ["__bve_edSpeed", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_time=function(block){
  return ["__bve_edTime", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_vehicle_state=function(block){
  return ["__bve_ed" + block.getFieldValue("FIELD_SEL"), Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_get_handle=function(block){
  return ["__bve_h" + block.getFieldValue("FIELD_SEL"), Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_set_handle=function(block){
  return "__atsarg_" + block.getFieldValue("FIELD_SEL").toLowerCase() + " = "
    + (Blockly.Lua.valueToCode(block, "VALUE", Blockly.Lua.ORDER_NONE) || "0") + "\n";
}
Blockly.Lua.bve_sound_stop=function(block){
  return "__atsfnc_sound(" + block.getFieldValue("ID") + ", -10000)\n";
}
Blockly.Lua.bve_sound_play_once=function(block){
  return "__atsfnc_sound(" + block.getFieldValue("ID") + ", 1)\n";
}
Blockly.Lua.bve_sound_play_loop=function(block){
  return "__atsfnc_sound(" + block.getFieldValue("ID") + ", 0, " + 
    Blockly.Lua.valueToCode(block, "VOLUME", Blockly.Lua.ORDER_NONE) + ")\n";
}
Blockly.Lua.bve_get_sound_internal=function(block){
  return ["__atsfnc_sound(" + block.getFieldValue("ID") + ")", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_set_sound_internal=function(block){
  return "__atsfnc_sound(" + block.getFieldValue("ID") + ", " + 
    Blockly.Lua.valueToCode(block, "INTERNAL_VAL", Blockly.Lua.ORDER_NONE) + ")\n";
}
Blockly.Lua.bve_set_panel=function(block){
  return "__atsfnc_sound(" + block.getFieldValue("ID") + ", " + 
    Blockly.Lua.valueToCode(block, "VALUE", Blockly.Lua.ORDER_NONE) + ")\n";
}
Blockly.Lua.bve_get_panel=function(block){
  return ["__atsfnc_panel(" + block.getFieldValue("ID") + ")", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_key=function(block){
  return [["S","A1","A2","B1","B2","C1","C2","D","E","F","G","H","I","J","K","L"].indexOf(block.getFieldValue("KEY_TYPE")),
    Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_get_key=function(block){
  return ["__bve_keystate[(" + Blockly.Lua.valueToCode(block, "KEY_TYPE", Blockly.Lua.ORDER_NONE) + ")+1]",
    Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_horn=function(block){
  return [["Primary", "Secondary", "Music"].indexOf(block.getFieldValue("KEY_TYPE")), Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_get_door=function(block){
  return ["__bve_doorstate", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_init_mode=function(block){
  return ["__atsarg_initindex", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_updown_key=function(block){
  return ["__atsarg_key", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_horn_blew=function(block){
  return ["__atsarg_horntype", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_signal_aspect=function(block){
  return ["__atsarg_signal", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_get_beacon=function(block){
  return ["__atsarg_" + block.getFieldValue("FIELD_SEL").toLowerCase(), Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_config_load=function(block){

}
Blockly.Lua.bve_config_save=function(block){

}
Blockly.Lua.bve_get_config=function(block){

}
Blockly.Lua.bve_get_config_default=function(block){

}
Blockly.Lua.bve_set_config=function(block){

}