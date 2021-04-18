var workspace = null;

var themeWithHat = Blockly.Theme.defineTheme('themeWithHat', {
  'base': Blockly.Themes.Classic,
  'blockStyles': {
    "bve_blocks": {
       "colourPrimary": "#37474f",
       "colourSecondary":"#90a4ae",
       "colourTertiary":"#aed581",
       "hat": "cap"
    }, 
    "comment_block": {
      "colourPrimary": "#cccccc",
      "colourSecondary":"#cccccc",
      "colourTertiary":"#cccccc"
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

var shortcutKeyMap = ["Q", "W", "E", "R", "", "A", "S", "D", "F", "Z", "", "C", "V"];

function batsInit(toolboxNode) {
  Blockly.prompt = function(msg, defaultValue, callback) {
    alertify.prompt(msg, defaultValue, function(evt, value){callback(value)});
  }
  
  var blocklyDiv = document.getElementById('blocklyDiv');
  window.workspace = Blockly.inject(blocklyDiv, {
    toolbox: toolboxNode,
    media: "media/",
    grid: { spacing: 40, length: 3, colour: "#ccc", snap: true },
    zoom: {
      controls: true,
      wheel: false,
      startScale: 1.0,
      maxScale: 3,
      minScale: 0.3,
      scaleSpeed: 1.2,
      pinch: false,
    },
    move:{
      scrollbars: {
        horizontal: true,
        vertical: true
      },
      drag: true,
      wheel: true
    },
    maxInstances: {
      bve_hat_elapse: 1,
      bve_hat_initialize: 1,
      bve_hat_keydown_any: 1,
      bve_hat_keyup_any: 1,
      bve_hat_horn_blow: 1,
      bve_hat_door_change: 1,
      bve_hat_set_signal: 1,
      bve_hat_set_beacon: 1,
      bve_hat_load: 1,
      bve_hat_dispose: 1,
    },
  });
  workspace.setTheme(themeWithHat);
  var onresize = function(e) { Blockly.svgResize(workspace); };
  window.addEventListener('resize', onresize, false);
  Blockly.svgResize(workspace);

  var onkeydown = function(e) {
    if (e.shiftKey && e.key && document.activeElement.tagName.toUpperCase() != "INPUT") {
      var selIndex = shortcutKeyMap.indexOf(e.key);
      if (selIndex >= 0) workspace.getToolbox().selectItemByPosition(selIndex);
    }
    if (e.key == "Shift") document.getElementById("hotkey-hint-overlay").style.display = "block";
  }
  var onkeyup = function(e) {
    if (e.key == "Shift") document.getElementById("hotkey-hint-overlay").style.display = "none";
  }
  document.addEventListener('keydown', onkeydown, false);
  document.addEventListener('keyup', onkeyup, false);
  batsWkspReset();

  if (typeof WorkspaceSearch != 'undefined' && WorkspaceSearch != null) {
    var workspaceSearch = new WorkspaceSearch(workspace);
    workspaceSearch.init();
  }

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

var pageSeq, pageMap, pageCurrent, shareVariables;

function batsPageSwitchTo(newPage) {
  var dom = Blockly.Xml.workspaceToDom(workspace);
  var domVariables = dom.getElementsByTagName("variables");
  if (domVariables.length > 0) dom.removeChild(domVariables[0]);
  shareVariables = workspace.variableMap_.getAllVariables();
  pageMap[pageCurrent] = dom;

  pageCurrent = newPage;
  workspace.clear();
  workspace.getToolbox().clearSelection(); // To update the variable category
  if (pageMap[pageCurrent] != null) {
    var newDom = pageMap[pageCurrent];
    Blockly.Xml.domToWorkspace(newDom, workspace);
  } else {
    pageSeq.push(pageCurrent);
  }
  workspace.variableMap_.clear();
  for (var i = 0; i < shareVariables.length; i++) {
    workspace.variableMap_.createVariable(shareVariables[i].name, shareVariables[i].type, shareVariables[i].id);
  }
}

function batsWkspReset() {
  pageSeq = ["Main"];
  pageMap = {"Main": null};
  pageCurrent = "Main";
  shareVariables = [];
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