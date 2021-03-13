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

Blockly.CSharp.addReservedWords([
  "BlocklyAtsPlugin",
  "DoorChange",
  "Elapse",
  "HornBlow",
  "IRuntime",
  "Initialize",
  "KeyDown",
  "KeyUp",
  "Load",
  "OpenBveApi",
  "PerformAI",
  "SetBeacon",
  "SetBrake",
  "SetPower",
  "SetReverser",
  "SetSignal",
  "SetVehicleSpecs",
  "System",
  "Unload",
  "_pbeacondata",
  "_phorntype",
  "_pinitindex",
  "_pkey",
  "_psignal",
  "_c",
  "_p1",
  "_p2",
].join(","));

function batsExportCSharp(workspace) {
  Blockly.CSharp.init(workspace);

  var allHats = ["bve_hat_elapse", "bve_hat_initialize", "bve_hat_keydown_any", "bve_hat_keyup_any", "bve_hat_horn_blow", 
    "bve_hat_door_change", "bve_hat_set_signal", "bve_hat_set_beacon", "bve_hat_load", "bve_hat_dispose"];
  var code = "private AtsCompanion _c;\n"
  + "public void SetVehicleSpecs(VehicleSpecs _p1) { _c.VSpec = _p1; }\n"
  + "public void SetReverser(int _p1) { if (_c.EData != null) _c.EData.Handles.Reverser = _p1; }\n"
  + "public void SetPower(int _p1) { if (_c.EData != null) _c.EData.Handles.PowerNotch = _p1; }\n"
  + "public void SetBrake(int _p1) { if (_c.EData != null) _c.EData.Handles.BrakeNotch = _p1; }\n"
  + "public void PerformAI(AIData _p1) { }\n";
  var blocks = workspace.getTopBlocks(false);
  for (var i = 0, block; block = blocks[i]; i++) {
    if (block.type.startsWith("bve_hat")) {
      removeItemOnce(allHats, block.type);
      code += Blockly.CSharp.blockToCode(block, true);
      //var nextBlock = block.getNextBlock();
      //if (nextBlock) code += Blockly.CSharp.blockToCode(nextBlock);
      if (block.type == "bve_hat_load") code += "return true;\n";
      code += "}\n";
    } else if (block.type == "procedures_defnoreturn" || block.type == "procedures_defreturn") {
      code += Blockly.CSharp.blockToCode(block);
    }
  }

  for (var i = 0, hatName; hatName = allHats[i]; i++) {
    code += Blockly.CSharp[hatName]();
    if (hatName == "bve_hat_load") code += "return true;\n";
    code += "}\n";
  }

  code = Blockly.CSharp.finish(code).trim();
  return "public class BlocklyAtsPlugin : IRuntime {\n" + code + "}";
}

Blockly.CSharp.bve_hat_elapse=function(block){
  return "public void Elapse(ElapseData _p1) { _c.EData = _p1;\n";
}
Blockly.CSharp.bve_hat_initialize=function(block){
  return "public void Initialize(InitializationModes _p1) { int _pinitindex = (int)_p1;\n";
}
Blockly.CSharp.bve_hat_keydown_any=function(block){
  return "public void KeyDown(VirtualKeys _p1) { int _pkey = (int)_p1; if (_pkey > 15) return; _c.KeyState[_pkey] = true;\n";
}
Blockly.CSharp.bve_hat_keyup_any=function(block){
  return "public void KeyUp(VirtualKeys _p1) { int _pkey = (int)_p1; if (_pkey > 15) return; _c.KeyState[_pkey] = false;\n";
}
Blockly.CSharp.bve_hat_horn_blow=function(block){
  return "public void HornBlow(HornTypes _p1) { int _phorntype = (int)_p1;\n";
}
Blockly.CSharp.bve_hat_door_change=function(block){
  return "public void DoorChange(DoorStates _p1, DoorStates _p2) { _c.DoorState = _p2; if ((_p1 == DoorStates.None) == (_p2 == DoorStates.None)) return;\n";
}
Blockly.CSharp.bve_hat_set_signal=function(block){
  return "public void SetSignal(SignalData[] _p1) { int _psignal = _p1[0].Aspect;\n";
}
Blockly.CSharp.bve_hat_set_beacon=function(block){
  return "public void SetBeacon(BeaconData _pbeacondata) {\n";
}
Blockly.CSharp.bve_hat_load=function(block){
  return "public bool Load(LoadProperties _p1) { _c = new AtsCompanion(_p1);\n";
}
Blockly.CSharp.bve_hat_dispose=function(block){
  return "public void Unload() {\n";
}
Blockly.CSharp.bve_vehicle_spec=function(block){
  return ["_c.VSpec."+block.getFieldValue("FIELD_SEL"), Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_location=function(block){
  return ["_c.EData.Vehicle.Location", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_speed=function(block){
  return ["_c.EData.Vehicle.Speed.KilometersPerHour", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_time=function(block){
  return ["_c.EData.TotalTime.Milliseconds", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_vehicle_state=function(block){
  if (block.getFieldValue("FIELD_SEL") == "Current") {
    return ["0", Blockly.CSharp.ORDER_ATOMIC];
  } else {
    return ["_c.EData.Vehicle." + block.getFieldValue("FIELD_SEL"), Blockly.CSharp.ORDER_MEMBER];
  }
}
Blockly.CSharp.bve_get_handle=function(block){
  var handleName = block.getFieldValue("FIELD_SEL"); if (handleName == "Power" || handleName == "Brake") handleName += "Notch";
  return ["_c.EData.Handles." + handleName, Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_set_handle=function(block){
  var handleName = block.getFieldValue("FIELD_SEL"); if (handleName == "Power" || handleName == "Brake") handleName += "Notch";
  if (handleName == "ConstSpeed") {
    return "_c.EData.Handles.ConstSpeed = new bool[] {false, true, _c.EData.Handles.ConstSpeed}["
    + (Blockly.CSharp.valueToCode(block, "VALUE", Blockly.CSharp.ORDER_NONE) || "0") + "];\n";
  } else {
    return "_c.EData.Handles." + handleName + " = "
      + (Blockly.CSharp.valueToCode(block, "VALUE", Blockly.CSharp.ORDER_NONE) || "0") + ";\n";
  }
}
Blockly.CSharp.bve_sound_stop=function(block){
  return "_c.AccessLegacySound(" + block.getFieldValue("ID") + ", -10000);\n";
}
Blockly.CSharp.bve_sound_play_once=function(block){
  return "_c.AccessLegacySound(" + block.getFieldValue("ID") + ", 1);\n";
}
Blockly.CSharp.bve_sound_play_loop=function(block){
  return "_c.AccessLegacySound(" + block.getFieldValue("ID") + ", 0, " + 
    Blockly.CSharp.valueToCode(block, "VOLUME", Blockly.CSharp.ORDER_NONE) + ");\n";
}
Blockly.CSharp.bve_get_sound_internal=function(block){
  return ["_c.AccessLegacySound(" + block.getFieldValue("ID") + ")", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_set_sound_internal=function(block){
  return "_c.AccessLegacySound(" + block.getFieldValue("ID") + ", " + 
    Blockly.CSharp.valueToCode(block, "INTERNAL_VAL", Blockly.CSharp.ORDER_NONE) + ");\n";
}
Blockly.CSharp.bve_set_panel=function(block){
  return "_c.Panel[" + block.getFieldValue("ID") + "] = " + 
    Blockly.CSharp.valueToCode(block, "VALUE", Blockly.CSharp.ORDER_NONE) + ";\n";
}
Blockly.CSharp.bve_get_panel=function(block){
  return ["_c.Panel[" + block.getFieldValue("ID") + "]", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_key=function(block){
  return [["S","A1","A2","B1","B2","C1","C2","D","E","F","G","H","I","J","K","L"].indexOf(block.getFieldValue("KEY_TYPE")),
    Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_get_key=function(block){
  return ["_c.KeyState[" + Blockly.CSharp.valueToCode(block, "KEY_TYPE", Blockly.CSharp.ORDER_NONE) + "]",
    Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_horn=function(block){
  return [["Primary", "Secondary", "Music"].indexOf(block.getFieldValue("KEY_TYPE")), Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_get_door=function(block){
  return ["_c.LegacyDoorState", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_init_mode=function(block){
  return ["_pinitindex", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_updown_key=function(block){
  return ["_pkey", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_horn_blew=function(block){
  return ["_phorntype", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_signal_aspect=function(block){
  return ["_psignal", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_get_beacon=function(block){
  var propName = block.getFieldValue("FIELD_SEL");
  if (propName == "Signal") propName = "Signal.Aspect"; else if (propName == "Distance") propName = "Signal.Distance";
  return ["_pbeacondata." + propName, Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_config_load=function(block){
  return "_c.LoadConfig(" + Blockly.CSharp.quote_(block.getFieldValue("PATH")) + ");\n";
}
Blockly.CSharp.bve_config_save=function(block){
  return "_c.SaveConfig(" + Blockly.CSharp.quote_(block.getFieldValue("PATH")) + ");\n";
}
Blockly.CSharp.bve_get_config=function(block){
  return ["_c.GetConfig(" + Blockly.CSharp.quote_(block.getFieldValue("PART")) + ", " 
    + Blockly.CSharp.quote_(block.getFieldValue("KEY")) + ")", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_get_config_default=function(block){ // TODO
  return [
    "_c.GetConfig(" + Blockly.CSharp.quote_(block.getFieldValue("PART")) + ", " 
    + Blockly.CSharp.quote_(block.getFieldValue("KEY")) + ", ("
    + Blockly.Lua.valueToCode(block, "DEFAULT_VAL", Blockly.CSharp.ORDER_NONE) +"))",
    Blockly.CSharp.ORDER_ATOMIC
  ];
}
Blockly.CSharp.bve_set_config=function(block){
  return "_c.SetConfig(" + Blockly.CSharp.quote_(block.getFieldValue("PART")) + ", " 
    + Blockly.CSharp.quote_(block.getFieldValue("KEY")) + ", "
    + (Blockly.CSharp.valueToCode(block, "VALUE", Blockly.CSharp.ORDER_NONE) || "\"\"") + ");\n";;
}
Blockly.CSharp.bve_msgbox=function(block){
  return "MessageBox.Show(" + (Blockly.CSharp.valueToCode(block, "MSG", Blockly.CSharp.ORDER_NONE) || "\"\"") 
    + ", \"BlocklyATS Message\");\n";;
}