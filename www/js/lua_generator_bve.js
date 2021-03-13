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
  "_edispose",
  "_edoorchange",
  "_edoorchange",
  "_eelapse",
  "_ehornblow",
  "_einitialize",
  "_ekeydown",
  "_ekeyup",
  "_eload",
  "_esetbeacondata",
  "_esetsignal",
  "_pbrake",
  "_pconstspeed",
  "_pdistance",
  "_pdoorstate",
  "_phorntype",
  "_pinitindex",
  "_pkey",
  "_poptional",
  "_ppower",
  "_preverser",
  "_psignal",
  "_ptype",
  "_fcfgload",
  "_fcfgsave",
  "_fcfgget",
  "_fcfgset",
  "_fpanel",
  "_fsound",
  "_fmsgbox",
  "_vconfig",
  "_vdlldir",
  "_bEdBcPressure",
  "_bEdBpPressure",
  "_bEdCurrent",
  "_bEdErPressure",
  "_bEdLocation",
  "_bEdMrPressure",
  "_bEdSapPressure",
  "_bEdSpeed",
  "_bEdTime",
  "_bHBrake",
  "_bHPower",
  "_bHReverser",
  "_bVsAtsNotch",
  "_bVsB67Notch",
  "_bVsBrakeNotches",
  "_bVsCars",
  "_bVsPowerNotches",
  "LIP",
].join(","));

function batsExportLua(workspace) {
  Blockly.Lua.init(workspace);

  var allHats = ["bve_hat_elapse", "bve_hat_initialize", "bve_hat_keydown_any", "bve_hat_keyup_any", "bve_hat_horn_blow", 
    "bve_hat_door_change", "bve_hat_set_signal", "bve_hat_set_beacon", "bve_hat_load", "bve_hat_dispose"];
  var code = "";
  var blocks = workspace.getTopBlocks(false);
  for (var i = 0, block; block = blocks[i]; i++) {
    if (block.type.startsWith("bve_hat")) {
      removeItemOnce(allHats, block.type);
      code += Blockly.Lua.blockToCode(block, true);
      var nextBlock = block.getNextBlock();
      if (nextBlock) code += Blockly.Lua.blockToCode(nextBlock);
      if (block.type == "bve_hat_elapse") code += "return _ppower, _pbrake, _preverser, _pconstspeed\n";
      code += "end\n";
    } else if (block.type == "procedures_defnoreturn" || block.type == "procedures_defreturn") {
      code += Blockly.Lua.blockToCode(block);
    }
  }

  for (var i = 0, hatName; hatName = allHats[i]; i++) {
    code += Blockly.Lua[hatName]();
    if (hatName == "bve_hat_elapse") code += "return _ppower, _pbrake, _preverser, _pconstspeed\n";
    code += "end\n";
  }

  return Blockly.Lua.finish(code).trim();
}

Blockly.Lua.bve_hat_elapse=function(block){
  return "function _eelapse(_ppower, _pbrake, _preverser, _pconstspeed)\n";
}
Blockly.Lua.bve_hat_initialize=function(block){
  return "function _einitialize(_pinitindex)\n";
}
Blockly.Lua.bve_hat_keydown_any=function(block){
  return "function _ekeydown(_pkey)\n_bkeystate[_pkey+1]=true\n";
}
Blockly.Lua.bve_hat_keyup_any=function(block){
  return "function _ekeyup(_pkey)\n_bkeystate[_pkey+1]=false\n";
}
Blockly.Lua.bve_hat_horn_blow=function(block){
  return "function _ehornblow(_phorntype)\n";
}
Blockly.Lua.bve_hat_door_change=function(block){
  return "function _edoorchange(_pdoorstate)\n_bdoorstate=_pdoorstate\n";
}
Blockly.Lua.bve_hat_set_signal=function(block){
  return "function _esetsignal(_psignal)\n";
}
Blockly.Lua.bve_hat_set_beacon=function(block){
  return "function _esetbeacondata(_pdistance, _poptional, _psignal, _ptype)\n";
}
Blockly.Lua.bve_hat_load=function(block){
  return "function _eload()\n";
}
Blockly.Lua.bve_hat_dispose=function(block){
  return "function _edispose()\n";
}
Blockly.Lua.bve_vehicle_spec=function(block){
  return ["_bVs"+block.getFieldValue("FIELD_SEL"), Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_location=function(block){
  return ["_bEdLocation", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_speed=function(block){
  return ["_bEdSpeed", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_time=function(block){
  return ["_bEdTime", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_vehicle_state=function(block){
  return ["_bEd" + block.getFieldValue("FIELD_SEL"), Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_get_handle=function(block){
  return ["_bH" + block.getFieldValue("FIELD_SEL"), Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_set_handle=function(block){
  return "_p" + block.getFieldValue("FIELD_SEL").toLowerCase() + " = "
    + (Blockly.Lua.valueToCode(block, "VALUE", Blockly.Lua.ORDER_NONE) || "0") + "\n";
}
Blockly.Lua.bve_sound_stop=function(block){
  return "_fsound(" + block.getFieldValue("ID") + ", -10000)\n";
}
Blockly.Lua.bve_sound_play_once=function(block){
  return "_fsound(" + block.getFieldValue("ID") + ", 1)\n";
}
Blockly.Lua.bve_sound_play_loop=function(block){
  return "_fsound(" + block.getFieldValue("ID") + ", 0, " + 
    Blockly.Lua.valueToCode(block, "VOLUME", Blockly.Lua.ORDER_NONE) + ")\n";
}
Blockly.Lua.bve_get_sound_internal=function(block){
  return ["_fsound(" + block.getFieldValue("ID") + ")", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_set_sound_internal=function(block){
  return "_fsound(" + block.getFieldValue("ID") + ", " + 
    Blockly.Lua.valueToCode(block, "INTERNAL_VAL", Blockly.Lua.ORDER_NONE) + ")\n";
}
Blockly.Lua.bve_set_panel=function(block){
  return "_fpanel(" + block.getFieldValue("ID") + ", " + 
    Blockly.Lua.valueToCode(block, "VALUE", Blockly.Lua.ORDER_NONE) + ")\n";
}
Blockly.Lua.bve_get_panel=function(block){
  return ["_fpanel(" + block.getFieldValue("ID") + ")", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_key=function(block){
  return [["S","A1","A2","B1","B2","C1","C2","D","E","F","G","H","I","J","K","L"].indexOf(block.getFieldValue("KEY_TYPE")),
    Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_get_key=function(block){
  return ["_bkeystate[(" + Blockly.Lua.valueToCode(block, "KEY_TYPE", Blockly.Lua.ORDER_NONE) + ")+1]",
    Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_horn=function(block){
  return [["Primary", "Secondary", "Music"].indexOf(block.getFieldValue("KEY_TYPE")), Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_get_door=function(block){
  return ["_bdoorstate", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_init_mode=function(block){
  return ["_pinitindex", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_updown_key=function(block){
  return ["_pkey", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_horn_blew=function(block){
  return ["_phorntype", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_signal_aspect=function(block){
  return ["_psignal", Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_get_beacon=function(block){
  return ["_p" + block.getFieldValue("FIELD_SEL").toLowerCase(), Blockly.Lua.ORDER_ATOMIC];
}
Blockly.Lua.bve_config_load=function(block){
  return "_fcfgload(" + Blockly.Lua.quote_(block.getFieldValue("PATH")) + ")\n";
}
Blockly.Lua.bve_config_save=function(block){
  return "_fcfgsave(" + Blockly.Lua.quote_(block.getFieldValue("PATH")) + ")\n";
}
Blockly.Lua.bve_get_config=function(block){
  return [
    "_fcfgget(" + Blockly.Lua.quote_(block.getFieldValue("PART")) + "," 
    + Blockly.Lua.quote_(block.getFieldValue("KEY")) + ")",
    Blockly.Lua.ORDER_ATOMIC
  ];
}
Blockly.Lua.bve_get_config_default=function(block){
  return [
    "_fcfgget(" + Blockly.Lua.quote_(block.getFieldValue("PART")) + "," 
    + Blockly.Lua.quote_(block.getFieldValue("KEY")) + ",("
    + (Blockly.Lua.valueToCode(block, "DEFAULT_VAL", Blockly.Lua.ORDER_NONE) || "\"\"") + "))",
    Blockly.Lua.ORDER_ATOMIC
  ];
}
Blockly.Lua.bve_set_config=function(block){
  return "_fcfgset(" + Blockly.Lua.quote_(block.getFieldValue("PART")) + "," 
    + Blockly.Lua.quote_(block.getFieldValue("KEY")) + ",("
    + (Blockly.Lua.valueToCode(block, "VALUE", Blockly.Lua.ORDER_NONE) || "\"\"") + "))\n";;
}
Blockly.Lua.bve_msgbox=function(block){
  return "_fmsgbox(" + (Blockly.Lua.valueToCode(block, "MSG", Blockly.Lua.ORDER_NONE) || "\"\"") + ")\n";;
}