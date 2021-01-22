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

Blockly.CSharp.addReservedWords(["BlocklyAtsPlugin",
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
  "__atsfnc_iniload",
  "__atsfnc_inisave",
  "__atsfnc_sound",
  "__atsval_config",
  "__atsval_dlldir",
  "__atsval_sound_emulated",
  "__bve_doorstate",
  "__bve_ed",
  "__bve_keystate",
  "__bve_panel",
  "__bve_playsound",
  "__bve_soundhandle",
  "__bve_vs",
  "__zbx_1",
  "__zbx_2",
].join(","));

function batsExportCSharp(workspace) {
  Blockly.CSharp.init(workspace);

  var allHats = ["bve_hat_elapse", "bve_hat_initialize", "bve_hat_keydown_any", "bve_hat_keyup_any", "bve_hat_horn_blow", 
    "bve_hat_door_change", "bve_hat_set_signal", "bve_hat_set_beacon", "bve_hat_load", "bve_hat_dispose"];
  var code = "private VehicleSpecs __bve_vs;\n"
  + "private ElapseData __bve_ed;\n"
  + "private int[] __bve_panel = new int[256];\n"
  + "private PlaySoundDelegate __bve_playsound;\n"
  + "private bool __bve_doorstate;\n"
  + "private bool[] __bve_keystate = new bool[16];\n"
  + "private Dictionary<string,Dictionary<string,dynamic>> __atsval_config;\n"
  + "private int[] __atsval_sound_emulated = new int[256];\n"
  + "private SoundHandle[] __bve_soundhandle = new SoundHandle[256];\n"
  + "private string __atsval_dlldir;\n"
  + "private int __atsfnc_sound(int id,int state=int.MaxValue,double volume=double.NaN){if(state==0&&double.IsNaN(volume)){if(volume<=0){state=-10000;}else if(volume>=100){state=0;}else{state=(int)(volume*100)-10000;}}if(state==-10000){if(__bve_soundhandle[id]!=null&&__bve_soundhandle[id].Playing){__bve_soundhandle[id].Stop();__bve_soundhandle[id]=null;}__atsval_sound_emulated[id]=state;}else if(state>-10000&&state<=0){if(__bve_soundhandle[id]!=null&&__bve_soundhandle[id].Playing){__bve_soundhandle[id].Volume=(state+10000)/10000;}else{__bve_soundhandle[id]=__bve_playsound(id,(state+10000)/10000,1,true);}__atsval_sound_emulated[id]=state;}else if(state==1){if(__bve_soundhandle[id]!=null&&__bve_soundhandle[id].Playing){__bve_soundhandle[id].Stop();}__bve_soundhandle[id]=__bve_playsound(id,1,1,false);__atsval_sound_emulated[id]=2;}else if(state==2){__atsval_sound_emulated[id]=state;}else if(state>10000){return __atsval_sound_emulated[id];}else{throw new System.ArgumentOutOfRangeException();}return 0;}\n"
  + "private Dictionary<string,Dictionary<string,dynamic>>__atsfnc_iniload(string path){var data=new Dictionary<string,Dictionary<string,dynamic>>();var section=\"\";foreach(var line in System.IO.File.ReadAllLines(path)){var tline=line.Trim();if(tline.StartsWith(\"[\")&&tline.EndsWith(\"]\")){section=tline.Substring(1,tline.Length-2);if(!data.ContainsKey(section))data[section]=new Dictionary<string,dynamic>();}if(!tline.StartsWith(\"#\")&&tline.Contains(\"=\")){var parts=tline.Split(new []{'='},2);double td;if(double.TryParse(parts[1],out td))data[section][parts[0]]=td;else if(parts[1].ToLowerInvariant()==\"true\")data[section][parts[0]]=true;else if(parts[1].ToLowerInvariant()==\"false\")data[section][parts[0]]=false;else data[section][parts[0]]=parts[1];}}	return data;}\n"
  + "private void __atsfnc_inisave(string path,Dictionary<string,Dictionary<string,dynamic>>data){var sb=new System.Text.StringBuilder();foreach(var section in data){sb.AppendFormat(\"[{0}]\",section.Key);foreach(var pair in section.Value){sb.AppendFormat(\"{0}={1}\\n\",pair.Key,pair.Value);}sb.AppendLine();}System.IO.File.WriteAllText(path,sb.ToString());}\n"
  + "public void SetVehicleSpecs(VehicleSpecs __zbx_1) { __bve_vs = __zbx_1; }\n"
  + "public void SetReverser(int __zbx_1) { if (__bve_ed != null) __bve_ed.Handles.Reverser = __zbx_1; }\n"
  + "public void SetPower(int __zbx_1) { if (__bve_ed != null) __bve_ed.Handles.PowerNotch = __zbx_1; }\n"
  + "public void SetBrake(int __zbx_1) { if (__bve_ed != null) __bve_ed.Handles.BrakeNotch = __zbx_1; }\n"
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
  return "using OpenBveApi.Runtime;\nusing System;\nusing System.Windows.Forms;\nusing System.Collections.Generic;\n"
  + "public class BlocklyAtsPlugin : IRuntime {\n" + code + "}";
}

Blockly.CSharp.bve_hat_elapse=function(block){
  return "public void Elapse(ElapseData __zbx_1) { __bve_ed = __zbx_1;\n";
}
Blockly.CSharp.bve_hat_initialize=function(block){
  return "public void Initialize(InitializationModes __zbx_1) { int __atsarg_initindex = (int)__zbx_1;\n";
}
Blockly.CSharp.bve_hat_keydown_any=function(block){
  return "public void KeyDown(VirtualKeys __zbx_1) { int __atsarg_key = (int)__zbx_1; if (__atsarg_key > 15) return; __bve_keystate[__atsarg_key] = true;\n";
}
Blockly.CSharp.bve_hat_keyup_any=function(block){
  return "public void KeyUp(VirtualKeys __zbx_1) { int __atsarg_key = (int)__zbx_1; if (__atsarg_key > 15) return; __bve_keystate[__atsarg_key] = false;\n";
}
Blockly.CSharp.bve_hat_horn_blow=function(block){
  return "public void HornBlow(HornTypes __zbx_1) { int __atsarg_horntype = (int)__zbx_1;\n";
}
Blockly.CSharp.bve_hat_door_change=function(block){
  return "public void DoorChange(DoorStates __zbx_1, DoorStates __zbx_2) { if ((__zbx_1 == DoorStates.None) == (__zbx_2 == DoorStates.None)) return; __bve_doorstate = __zbx_2 != DoorStates.None;\n";
}
Blockly.CSharp.bve_hat_set_signal=function(block){
  return "public void SetSignal(SignalData[] __zbx_1) { int __atsarg_signal = __zbx_1[0].Aspect;\n";
}
Blockly.CSharp.bve_hat_set_beacon=function(block){
  return "public void SetBeacon(BeaconData __atsarg_beacondata) {\n";
}
Blockly.CSharp.bve_hat_load=function(block){
  return "public bool Load(LoadProperties __zbx_1) { __bve_playsound = __zbx_1.PlaySound; __zbx_1.Panel = __bve_panel; __atsval_dlldir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).TrimEnd('\\\\', '/');\n";
}
Blockly.CSharp.bve_hat_dispose=function(block){
  return "public void Unload() {\n";
}
Blockly.CSharp.bve_vehicle_spec=function(block){
  return ["__bve_vs."+block.getFieldValue("FIELD_SEL"), Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_location=function(block){
  return ["__bve_ed.Vehicle.Location", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_speed=function(block){
  return ["__bve_ed.Vehicle.Speed.KilometersPerHour", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_time=function(block){
  return ["__bve_ed.TotalTime.Milliseconds", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_vehicle_state=function(block){
  if (block.getFieldValue("FIELD_SEL") == "Current") {
    return ["0", Blockly.CSharp.ORDER_ATOMIC];
  } else {
    return ["__bve_ed.Vehicle." + block.getFieldValue("FIELD_SEL"), Blockly.CSharp.ORDER_MEMBER];
  }
}
Blockly.CSharp.bve_get_handle=function(block){
  var handleName = block.getFieldValue("FIELD_SEL"); if (handleName == "Power" || handleName == "Brake") handleName += "Notch";
  return ["__bve_ed.Handles." + handleName, Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_set_handle=function(block){
  var handleName = block.getFieldValue("FIELD_SEL"); if (handleName == "Power" || handleName == "Brake") handleName += "Notch";
  return "__bve_ed.Handles." + handleName + " = "
    + (Blockly.CSharp.valueToCode(block, "VALUE", Blockly.CSharp.ORDER_NONE) || "0") + ";\n";
}
Blockly.CSharp.bve_sound_stop=function(block){
  return "__atsfnc_sound(" + block.getFieldValue("ID") + ", -10000);\n";
}
Blockly.CSharp.bve_sound_play_once=function(block){
  return "__atsfnc_sound(" + block.getFieldValue("ID") + ", 1);\n";
}
Blockly.CSharp.bve_sound_play_loop=function(block){
  return "__atsfnc_sound(" + block.getFieldValue("ID") + ", 0, " + 
    Blockly.CSharp.valueToCode(block, "VOLUME", Blockly.CSharp.ORDER_NONE) + ");\n";
}
Blockly.CSharp.bve_get_sound_internal=function(block){
  return ["__atsfnc_sound(" + block.getFieldValue("ID") + ")", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_set_sound_internal=function(block){
  return "__atsfnc_sound(" + block.getFieldValue("ID") + ", " + 
    Blockly.CSharp.valueToCode(block, "INTERNAL_VAL", Blockly.CSharp.ORDER_NONE) + ");\n";
}
Blockly.CSharp.bve_set_panel=function(block){
  return "__bve_panel[" + block.getFieldValue("ID") + "] = " + 
    Blockly.CSharp.valueToCode(block, "VALUE", Blockly.CSharp.ORDER_NONE) + ";\n";
}
Blockly.CSharp.bve_get_panel=function(block){
  return ["__bve_panel[" + block.getFieldValue("ID") + "]", Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_key=function(block){
  return [["S","A1","A2","B1","B2","C1","C2","D","E","F","G","H","I","J","K","L"].indexOf(block.getFieldValue("KEY_TYPE")),
    Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_get_key=function(block){
  return ["__bve_keystate[" + Blockly.CSharp.valueToCode(block, "KEY_TYPE", Blockly.CSharp.ORDER_NONE) + "]",
    Blockly.CSharp.ORDER_MEMBER];
}
Blockly.CSharp.bve_horn=function(block){
  return [["Primary", "Secondary", "Music"].indexOf(block.getFieldValue("KEY_TYPE")), Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_get_door=function(block){
  return ["__bve_doorstate", Blockly.CSharp.ORDER_ATOMIC];
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
  return "__atsval_config = __atsfnc_iniload(__atsval_dlldir + " + Blockly.CSharp.quote_("\\" + block.getFieldValue("PATH")) + ");\n";
}
Blockly.CSharp.bve_config_save=function(block){
  return "__atsfnc_inisave(__atsval_dlldir + " + Blockly.CSharp.quote_("\\" + block.getFieldValue("PATH")) + ", __atsval_config);\n";
}
Blockly.CSharp.bve_get_config=function(block){
  return ["__atsval_config[" + Blockly.CSharp.quote_(block.getFieldValue("PART")) + "][" 
    + Blockly.CSharp.quote_(block.getFieldValue("KEY")) + "]", Blockly.CSharp.ORDER_ATOMIC];
}
Blockly.CSharp.bve_get_config_default_num=function(block){ // TODO
  return [
    "((__atsval_config." + block.getFieldValue("PART") + "." + block.getFieldValue("KEY") + " ~= nil) and {"
    + "__atsval_config." + block.getFieldValue("PART") + "." + block.getFieldValue("KEY") + "} or {"
    + block.getFieldValue("DEFAULT_VAL") + "})[1]",
    Blockly.CSharp.ORDER_ATOMIC
  ];
}
Blockly.CSharp.bve_get_config_default_text=function(block){ // TODO
  return [
    "((__atsval_config." + block.getFieldValue("PART") + "." + block.getFieldValue("KEY") + " ~= nil) and {"
    + "__atsval_config." + block.getFieldValue("PART") + "." + block.getFieldValue("KEY") + "} or {"
    + Blockly.CSharp.quote_("\\" + block.getFieldValue("PATH")) + "})[1]",
    Blockly.CSharp.ORDER_ATOMIC
  ];
}
Blockly.CSharp.bve_set_config=function(block){
  return "__atsval_config[" + Blockly.CSharp.quote_(block.getFieldValue("PART")) + "][" 
    + Blockly.CSharp.quote_(block.getFieldValue("KEY")) + "] = "
    + (Blockly.CSharp.valueToCode(block, "VALUE", Blockly.CSharp.ORDER_NONE) || "\"\"") + ";\n";;
}
Blockly.CSharp.bve_msgbox=function(block){
  return "MessageBox.Show(" + (Blockly.CSharp.valueToCode(block, "MSG", Blockly.CSharp.ORDER_NONE) || "\"\"") 
    + ", \"BlocklyATS Message\");\n";;
}