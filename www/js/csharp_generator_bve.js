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
  "__atsarg_beacondata",
  "__atsarg_horntype",
  "__atsarg_initindex",
  "__atsarg_key",
  "__atsarg_signal",
  "__c",
  "__zbx_1",
  "__zbx_2",
].join(","));

function batsExportCSharp(workspace) {
  Blockly.CSharp.init(workspace);

  var allHats = ["bve_hat_elapse", "bve_hat_initialize", "bve_hat_keydown_any", "bve_hat_keyup_any", "bve_hat_horn_blow", 
    "bve_hat_door_change", "bve_hat_set_signal", "bve_hat_set_beacon", "bve_hat_load", "bve_hat_dispose"];
  var code = "private AtsCompanion __c;\n"
  + "public void SetVehicleSpecs(VehicleSpecs __zbx_1) { __c.VSpec = __zbx_1; }\n"
  + "public void SetReverser(int __zbx_1) { if (__c.EData != null) __c.EData.Handles.Reverser = __zbx_1; }\n"
  + "public void SetPower(int __zbx_1) { if (__c.EData != null) __c.EData.Handles.PowerNotch = __zbx_1; }\n"
  + "public void SetBrake(int __zbx_1) { if (__c.EData != null) __c.EData.Handles.BrakeNotch = __zbx_1; }\n"
  + "public void PerformAI(AIData __zbx_1) { }\n";
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
  return "public void Elapse(ElapseData __zbx_1) { __c.EData = __zbx_1;\n";
}
Blockly.CSharp.bve_hat_initialize=function(block){
  return "public void Initialize(InitializationModes __zbx_1) { int __atsarg_initindex = (int)__zbx_1;\n";
}
Blockly.CSharp.bve_hat_keydown_any=function(block){
  return "public void KeyDown(VirtualKeys __zbx_1) { int __atsarg_key = (int)__zbx_1; if (__atsarg_key > 15) return; __c.KeyState[__atsarg_key] = true;\n";
}
Blockly.CSharp.bve_hat_keyup_any=function(block){
  return "public void KeyUp(VirtualKeys __zbx_1) { int __atsarg_key = (int)__zbx_1; if (__atsarg_key > 15) return; __c.KeyState[__atsarg_key] = false;\n";
}
Blockly.CSharp.bve_hat_horn_blow=function(block){
  return "public void HornBlow(HornTypes __zbx_1) { int __atsarg_horntype = (int)__zbx_1;\n";
}
Blockly.CSharp.bve_hat_door_change=function(block){
  return "public void DoorChange(DoorStates __zbx_1, DoorStates __zbx_2) { __c.DoorState = __zbx_2; if ((__zbx_1 == DoorStates.None) == (__zbx_2 == DoorStates.None)) return;\n";
}
Blockly.CSharp.bve_hat_set_signal=function(block){
  return "public void SetSignal(SignalData[] __zbx_1) { int __atsarg_signal = __zbx_1[0].Aspect;\n";
}
Blockly.CSharp.bve_hat_set_beacon=function(block){
  return "public void SetBeacon(BeaconData __atsarg_beacondata) {\n";
}
Blockly.CSharp.bve_hat_load=function(block){
  return "public bool Load(LoadProperties __zbx_1) { __c = new AtsCompanion(__zbx_1);\n";
}
Blockly.CSharp.bve_hat_dispose=function(block){
  return "public void Unload() {\n";
}
Blockly.CSharp.bve_vehicle_spec=function(block){
  return ["__c.VSpec."+block.getFieldValue("FIELD_SEL"), Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_location=function(block){
  return ["__c.EData.Vehicle.Location", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_speed=function(block){
  return ["__c.EData.Vehicle.Speed.KilometersPerHour", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_time=function(block){
  return ["__c.EData.TotalTime.Milliseconds", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_vehicle_state=function(block){
  if (block.getFieldValue("FIELD_SEL") == "Current") {
    return ["0", Blockly.CSharp.ORDER_ATOMIC];
  } else {
    return ["__c.EData.Vehicle." + block.getFieldValue("FIELD_SEL"), Blockly.CSharp.ORDER_MEMBER];
  }
}
Blockly.CSharp.bve_get_handle=function(block){
  var handleName = block.getFieldValue("FIELD_SEL"); if (handleName == "Power" || handleName == "Brake") handleName += "Notch";
  return ["__c.EData.Handles." + handleName, Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_set_handle=function(block){
  var handleName = block.getFieldValue("FIELD_SEL"); if (handleName == "Power" || handleName == "Brake") handleName += "Notch";
  if (handleName == "ConstSpeed") {
    return "__c.EData.Handles.ConstSpeed = new bool[] {false, true, __c.EData.Handles.ConstSpeed}["
    + (Blockly.CSharp.valueToCode(block, "VALUE", Blockly.CSharp.ORDER_NONE) || "0") + "];\n";
  } else {
    return "__c.EData.Handles." + handleName + " = "
      + (Blockly.CSharp.valueToCode(block, "VALUE", Blockly.CSharp.ORDER_NONE) || "0") + ";\n";
  }
}
Blockly.CSharp.bve_sound_stop=function(block){
  return "__c.AccessLegacySound(" + block.getFieldValue("ID") + ", -10000);\n";
}
Blockly.CSharp.bve_sound_play_once=function(block){
  return "__c.AccessLegacySound(" + block.getFieldValue("ID") + ", 1);\n";
}
Blockly.CSharp.bve_sound_play_loop=function(block){
  return "__c.AccessLegacySound(" + block.getFieldValue("ID") + ", 0, " + 
    Blockly.CSharp.valueToCode(block, "VOLUME", Blockly.CSharp.ORDER_NONE) + ");\n";
}
Blockly.CSharp.bve_get_sound_internal=function(block){
  return ["__c.AccessLegacySound(" + block.getFieldValue("ID") + ")", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_set_sound_internal=function(block){
  return "__c.AccessLegacySound(" + block.getFieldValue("ID") + ", " + 
    Blockly.CSharp.valueToCode(block, "INTERNAL_VAL", Blockly.CSharp.ORDER_NONE) + ");\n";
}
Blockly.CSharp.bve_set_panel=function(block){
  return "__c.Panel[" + block.getFieldValue("ID") + "] = " + 
    Blockly.CSharp.valueToCode(block, "VALUE", Blockly.CSharp.ORDER_NONE) + ";\n";
}
Blockly.CSharp.bve_get_panel=function(block){
  return ["__c.Panel[" + block.getFieldValue("ID") + "]", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_key=function(block){
  return [["S","A1","A2","B1","B2","C1","C2","D","E","F","G","H","I","J","K","L"].indexOf(block.getFieldValue("KEY_TYPE")),
    Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_get_key=function(block){
  return ["__c.KeyState[" + Blockly.CSharp.valueToCode(block, "KEY_TYPE", Blockly.CSharp.ORDER_NONE) + "]",
    Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_horn=function(block){
  return [["Primary", "Secondary", "Music"].indexOf(block.getFieldValue("KEY_TYPE")), Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_get_door=function(block){
  return ["__c.LegacyDoorState", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_init_mode=function(block){
  return ["__atsarg_initindex", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_updown_key=function(block){
  return ["__atsarg_key", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_horn_blew=function(block){
  return ["__atsarg_horntype", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_signal_aspect=function(block){
  return ["__atsarg_signal", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_get_beacon=function(block){
  var propName = block.getFieldValue("FIELD_SEL");
  if (propName == "Signal") propName = "Signal.Aspect"; else if (propName == "Distance") propName = "Signal.Distance";
  return ["__atsarg_beacondata." + propName, Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_config_load=function(block){
  return "__c.LoadConfig(" + Blockly.CSharp.quote_(block.getFieldValue("PATH")) + ");\n";
}
Blockly.CSharp.bve_config_save=function(block){
  return "__c.SaveConfig(" + Blockly.CSharp.quote_(block.getFieldValue("PATH")) + ");\n";
}
Blockly.CSharp.bve_get_config=function(block){
  return ["__c.GetConfig(" + Blockly.CSharp.quote_(block.getFieldValue("PART")) + ", " 
    + Blockly.CSharp.quote_(block.getFieldValue("KEY")) + ")", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_get_config_default=function(block){ // TODO
  return [
    "__c.GetConfig(" + Blockly.CSharp.quote_(block.getFieldValue("PART")) + ", " 
    + Blockly.CSharp.quote_(block.getFieldValue("KEY")) + ", ("
    + Blockly.Lua.valueToCode(block, "DEFAULT_VAL", Blockly.CSharp.ORDER_NONE) +"))",
    Blockly.CSharp.ORDER_ATOMIC
  ];
}
Blockly.CSharp.bve_set_config=function(block){
  return "__c.SetConfig(" + Blockly.CSharp.quote_(block.getFieldValue("PART")) + ", " 
    + Blockly.CSharp.quote_(block.getFieldValue("KEY")) + ", "
    + (Blockly.CSharp.valueToCode(block, "VALUE", Blockly.CSharp.ORDER_NONE) || "\"\"") + ");\n";;
}
Blockly.CSharp.bve_msgbox=function(block){
  return "MessageBox.Show(" + (Blockly.CSharp.valueToCode(block, "MSG", Blockly.CSharp.ORDER_NONE) || "\"\"") 
    + ", \"BlocklyATS Message\");\n";;
}