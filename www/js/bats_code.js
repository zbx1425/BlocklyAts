var Code = {};
var workspace = {};

var themeWithHat = Blockly.Theme.defineTheme('themeWithHat', {
  'base': Blockly.Themes.Classic,
  'blockStyles': {
    "bve_blocks": {
       "colourPrimary": "#37474f",
       "colourSecondary":"#90a4ae",
       "colourTertiary":"#aed581",
       "hat": "cap"
    }
  }
});

function getQueryVariable(variable) {
  var query = window.location.search.substring(1);
  var vars = query.split('&');
  for (var i = 0; i < vars.length; i++) {
      var pair = vars[i].split('=');
      if (decodeURIComponent(pair[0]) == variable) {
          return decodeURIComponent(pair[1]);
      }
  }
  return null;
}

function onWkspChange(e) {
  
}

function batsInit(toolboxNode) {
  Blockly.prompt = function(msg, defaultValue, callback) {
    alertify.prompt(msg, defaultValue, function(evt, value){callback(value)});
  }
  
  var blocklyDiv = document.getElementById('blocklyDiv');
  window.workspace = Blockly.inject(blocklyDiv, {
    toolbox: toolboxNode,
    media: "media/",
    grid: {spacing: 40, length: 3, colour: '#ccc', snap: true},
    maxInstances: {
      "bve_hat_elapse": 1,
      "bve_hat_initialize": 1,
      "bve_hat_keydown_any": 1,
      "bve_hat_keyup_any": 1,
      "bve_hat_horn_blow": 1,
      "bve_hat_door_change": 1,
      "bve_hat_set_signal": 1,
      "bve_hat_set_beacon": 1,
      "bve_hat_load": 1,
      "bve_hat_dispose": 1,
    },
  });
  workspace.setTheme(themeWithHat);
  workspace.addChangeListener(onWkspChange);
  var onresize = function(e) { Blockly.svgResize(workspace); };
  window.addEventListener('resize', onresize, false);
  Blockly.svgResize(workspace);

  var toolboxMap = [11,0,1,2,3,5,6,7,8,9];
  var onkeydown = function(e) {
    if (e.shiftKey) {
      if (e.code && (e.code[5] >= '0' && e.code[5] <= '9')) {
        workspace.getToolbox().selectItemByPosition(toolboxMap[parseInt(e.code[5])]);
      } else if (e.keyCode && (e.keyCode >= 0x30 && e.keyCode <= 0x39)) {
        // Damn Microsoft!
        workspace.getToolbox().selectItemByPosition(toolboxMap[e.keyCode - 0x30]);
      }
    }
  }
  document.addEventListener('keydown', onkeydown, false);
}

window.addEventListener('load', function() {
  batsInit(document.getElementById("toolbox"));
});

function batsWkspReset() {
  workspace.clear();
}

function batsWkspSave() {
  return Blockly.Xml.domToText(Blockly.Xml.workspaceToDom(workspace));
}

function batsWkspLoad(xmlstr) {
  workspace.clear();
  Blockly.Xml.domToWorkspace(Blockly.Xml.textToDom(xmlstr),workspace);
}

function batsWkspExportLua() {
  return batsExportLua(workspace);
}

function batsWkspExportCSharp() {
  return batsExportCSharp(workspace);
}