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

function batsInterop(cmdName) {
  const xhr = new XMLHttpRequest();
  xhr.open("GET", "/interop/"+cmdName);
  xhr.send();
}

function batsInit(toolboxNode) {
  Blockly.prompt = function(msg, defaultValue, callback) {
    alertify.prompt(msg, defaultValue, function(evt, value){callback(value)});
  }
  
  var blocklyArea = document.getElementById('blocklyArea');
  var blocklyDiv = document.getElementById('blocklyDiv');
  window.workspace = Blockly.inject(blocklyDiv, {
    toolbox: toolboxNode,
    media: "media/",
    grid: {spacing: 40, length: 3, colour: '#ccc', snap: true}
  });
  workspace.setTheme(themeWithHat);
  var onresize = function(e) {
    // Compute the absolute coordinates and dimensions of blocklyArea.
    var element = blocklyArea;
    var x = 0;
    var y = 0;
    do {
      x += element.offsetLeft;
      y += element.offsetTop;
      element = element.offsetParent;
    } while (element);
    // Position blocklyDiv over blocklyArea.
    blocklyDiv.style.left = x + 'px';
    blocklyDiv.style.top = y + 'px';
    blocklyDiv.style.width = blocklyArea.offsetWidth + 'px';
    blocklyDiv.style.height = blocklyArea.offsetHeight + 'px';
    Blockly.svgResize(workspace);
  };
  window.addEventListener('resize', onresize, false);
  onresize();
  Blockly.svgResize(workspace);
  
  if (typeof afterBatsInit === undefined) { afterBatsInit(); }
}

window.addEventListener('load', function() {
  batsInit(document.getElementById("toolbox"));
  return;
  
  const xhr2 = new XMLHttpRequest();
  xhr2.onload = function(){batsInit(xhr2.responseXML.documentElement);};
  xhr2.open("GET", "toolbox.xml");
  xhr2.responseType = "document";
  xhr2.send();
});