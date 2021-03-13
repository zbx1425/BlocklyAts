var workspace = null;

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

var hIntervalInit;

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
  //workspace.addChangeListener(onWkspChange);
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

  if (getQueryVariable("ver") == null) hIntervalInit = setInterval(batsRemoteInit, 500);
  if (typeof onBlocklyLoad != 'undefined' && onBlocklyLoad != null) {
    onBlocklyLoad();
  }
}

var clientID = 0;

function batsRemoteInit() {
  if (window.location.protocol == 'file:') {
    document.getElementById("page-overlay").innerHTML = "This program cannot be started in this way. Please run BlocklyAts.exe!";
    document.getElementById("page-overlay").style.display = "block";
    clearInterval(hIntervalInit);
    return;
  }
  var xhr1 = new XMLHttpRequest();
  xhr1.onreadystatechange = function() {
    if (xhr1.readyState != 4) return;
    if (xhr1.status == 200) {
      var cIDElement = xhr1.responseXML.documentElement.getElementsByTagName("id")[0];
      var cID = (cIDElement.innerText || cIDElement.textContent || "");
      var versionElement = xhr1.responseXML.documentElement.getElementsByTagName("version")[0];
      var version = (versionElement.innerText || versionElement.textContent || "");
      clientID = parseInt(cID);
      document.getElementById("vertext").innerHTML = "v" + version + ", ";
      clearInterval(hIntervalInit);
      setInterval(batsRemoteHeartbeat, 500);
    } else {
      document.getElementById("page-overlay").innerHTML = "Please keep the BlocklyAts main program running!";
      document.getElementById("page-overlay").style.display = "block";
    }
  };
  xhr1.open("GET", "interop/meta");
  xhr1.responseType = "document";
  xhr1.send();
}

function batsRemoteHeartbeat() {
  var xhr1 = new XMLHttpRequest();
  xhr1.onreadystatechange = function() {
    if (xhr1.readyState != 4) return;
    if (xhr1.status == 200) {
      if (xhr1.responseXML.documentElement.getElementsByTagName("toomany").length > 0) {
        document.getElementById("page-overlay").innerHTML = "Too many clients! Only one tab page of one browser may be connected!";
        document.getElementById("page-overlay").style.display = "block";
      } else {
        document.getElementById("page-overlay").style.display = "none";
        if (xhr1.responseXML.documentElement.getElementsByTagName("id").length > 0) {
          var jsIDElement = xhr1.responseXML.documentElement.getElementsByTagName("id")[0];
          var jsID = (jsIDElement.innerText || jsIDElement.textContent || "");
          var scriptElement = xhr1.responseXML.documentElement.getElementsByTagName("script")[0];
          var script = (scriptElement.innerText || scriptElement.textContent || "");
          var xhr2 = new XMLHttpRequest();
          xhr2.open("POST", "interop/" + jsID);
          xhr2.responseType = "document";
          xhr2.send(eval(script));
        }
      }
    } else {
      document.getElementById("page-overlay").innerHTML = "Please keep the BlocklyAts main program running!";
      document.getElementById("page-overlay").style.display = "block";
    }
  };
  xhr1.open("POST", "interop/heartbeat");
  xhr1.responseType = "document";
  xhr1.send(clientID.toString());
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