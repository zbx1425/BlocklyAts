'use strict';

Blockly.CSharp.control = {};

Blockly.CSharp.controls_repeat = function() {
  // Repeat n times (internal number).
  var repeats = Number(this.getTitleValue('TIMES'));
  var branch = Blockly.CSharp.statementToCode(this, 'DO');
  if (Blockly.CSharp.INFINITE_LOOP_TRAP) {
    branch = Blockly.CSharp.INFINITE_LOOP_TRAP.replace(/%1/g,
        '\'' + this.id + '\'') + branch;
  }
  var loopVar = Blockly.CSharp.variableDB_.getDistinctName(
      'count', Blockly.Variables.NAME_TYPE);
  var code = 'for (var ' + loopVar + ' = 0; ' +
      loopVar + ' < ' + repeats + '; ' +
      loopVar + '++) {\n' +
      branch + '}\n';
  return code;
};

Blockly.CSharp.controls_repeat_ext = function() {
  // Repeat n times (external number).
  var repeats = Blockly.CSharp.valueToCode(this, 'TIMES',
      Blockly.CSharp.ORDER_ASSIGNMENT) || '0';
  var branch = Blockly.CSharp.statementToCode(this, 'DO');
  if (Blockly.CSharp.INFINITE_LOOP_TRAP) {
    branch = Blockly.CSharp.INFINITE_LOOP_TRAP.replace(/%1/g,
        '\'' + this.id + '\'') + branch;
  }
  var code = '';
  var loopVar = Blockly.CSharp.variableDB_.getDistinctName(
      'count', Blockly.Variables.NAME_TYPE);
  var endVar = repeats;
  if (!repeats.match(/^\w+$/) && !Blockly.isNumber(repeats)) {
    var endVar = Blockly.CSharp.variableDB_.getDistinctName(
        'repeat_end', Blockly.Variables.NAME_TYPE);
    code += 'var ' + endVar + ' = ' + repeats + ';\n';
  }
  code += 'for (var ' + loopVar + ' = 0; ' +
      loopVar + ' < ' + endVar + '; ' +
      loopVar + '++) {\n' +
      branch + '}\n';
  return code;
};

Blockly.CSharp.controls_whileUntil = function() {
  // Do while/until loop.
  var until = this.getTitleValue('MODE') == 'UNTIL';
  var argument0 = Blockly.CSharp.valueToCode(this, 'BOOL',
      until ? Blockly.CSharp.ORDER_LOGICAL_NOT :
      Blockly.CSharp.ORDER_NONE) || 'false';
  var branch = Blockly.CSharp.statementToCode(this, 'DO');
  if (Blockly.CSharp.INFINITE_LOOP_TRAP) {
    branch = Blockly.CSharp.INFINITE_LOOP_TRAP.replace(/%1/g,
        '\'' + this.id + '\'') + branch;
  }
  if (until) {
    argument0 = '!' + argument0;
  }
  return 'while (' + argument0 + ') {\n' + branch + '}\n';
};

Blockly.CSharp.controls_for = function() {
  // For loop.
  var variable0 = Blockly.CSharp.variableDB_.getName(
      this.getTitleValue('VAR'), Blockly.Variables.NAME_TYPE);
  var argument0 = Blockly.CSharp.valueToCode(this, 'FROM',
      Blockly.CSharp.ORDER_ASSIGNMENT) || '0';
  var argument1 = Blockly.CSharp.valueToCode(this, 'TO',
      Blockly.CSharp.ORDER_ASSIGNMENT) || '0';
  var increment = Blockly.CSharp.valueToCode(this, 'BY',
      Blockly.CSharp.ORDER_ASSIGNMENT) || '1';
  var branch = Blockly.CSharp.statementToCode(this, 'DO');
  if (Blockly.CSharp.INFINITE_LOOP_TRAP) {
    branch = Blockly.CSharp.INFINITE_LOOP_TRAP.replace(/%1/g,
        '\'' + this.id + '\'') + branch;
  }
  var code;
  if (Blockly.isNumber(argument0) && Blockly.isNumber(argument1) &&
      Blockly.isNumber(increment)) {
    // All arguments are simple numbers.
    var up = parseFloat(argument0) <= parseFloat(argument1);
    code = 'for (' + variable0 + ' = ' + argument0 + '; ' +
        variable0 + (up ? ' <= ' : ' >= ') + argument1 + '; ' +
        variable0;
    var step = Math.abs(parseFloat(increment));
    if (step == 1) {
      code += up ? '++' : '--';
    } else {
      code += (up ? ' += ' : ' -= ') + step;
    }
    code += ') {\n' + branch + '}\n';
  } else {
    code = '';
    // Cache non-trivial values to variables to prevent repeated look-ups.
    var startVar = argument0;
    if (!argument0.match(/^\w+$/) && !Blockly.isNumber(argument0)) {
      var startVar = Blockly.CSharp.variableDB_.getDistinctName(
          variable0 + '_start', Blockly.Variables.NAME_TYPE);
      code += 'var ' + startVar + ' = ' + argument0 + ';\n';
    }
    var endVar = argument1;
    if (!argument1.match(/^\w+$/) && !Blockly.isNumber(argument1)) {
      var endVar = Blockly.CSharp.variableDB_.getDistinctName(
          variable0 + '_end', Blockly.Variables.NAME_TYPE);
      code += 'var ' + endVar + ' = ' + argument1 + ';\n';
    }
    // Determine loop direction at start, in case one of the bounds
    // changes during loop execution.
    var incVar = Blockly.CSharp.variableDB_.getDistinctName(
        variable0 + '_inc', Blockly.Variables.NAME_TYPE);
    code += 'var ' + incVar + ' = ';
    if (Blockly.isNumber(increment)) {
      code += Math.abs(increment) + ';\n';
    } else {
      code += 'Math.Abs(' + increment + ');\n';
    }
    code += 'if (' + startVar + ' > ' + endVar + ') {\n';
    code += '  ' + incVar + ' = -' + incVar +';\n';
    code += '}\n';
    code += 'for (' + variable0 + ' = ' + startVar + ';\n' +
        '     '  + incVar + ' >= 0 ? ' +
        variable0 + ' <= ' + endVar + ' : ' +
        variable0 + ' >= ' + endVar + ';\n' +
        '     ' + variable0 + ' += ' + incVar + ') {\n' +
        branch + '}\n';
  }
  return code;
};

Blockly.CSharp.controls_forEach = function() {
  // For each loop.
  var variable0 = Blockly.CSharp.variableDB_.getName(
      this.getTitleValue('VAR'), Blockly.Variables.NAME_TYPE);
  var argument0 = Blockly.CSharp.valueToCode(this, 'LIST',
      Blockly.CSharp.ORDER_ASSIGNMENT) || '[]';
  var branch = Blockly.CSharp.statementToCode(this, 'DO');
  if (Blockly.CSharp.INFINITE_LOOP_TRAP) {
    branch = Blockly.CSharp.INFINITE_LOOP_TRAP.replace(/%1/g,
        '\'' + this.id + '\'') + branch;
  }
  var code;
  var indexVar = Blockly.CSharp.variableDB_.getDistinctName(
      variable0 + '_index', Blockly.Variables.NAME_TYPE);
  if (argument0.match(/^\w+$/)) {
    code = 'foreach (var ' + variable0 + ' in  ' + argument0 + ') {\n' + branch + '}\n';
  } else {
    // The list appears to be more complicated than a simple variable.
    // Cache it to a variable to prevent repeated look-ups.
    var listVar = Blockly.CSharp.variableDB_.getDistinctName(
        variable0 + '_list', Blockly.Variables.NAME_TYPE);
    branch = '  ' + variable0 + ' = ' + listVar + '[' + indexVar + '];\n' +
        branch;
    code = 'var ' + listVar + ' = ' + argument0 + ';\n' +
        'foreach (var ' + indexVar + ' in ' + listVar + ') {\n' +
        branch + '}\n';
  }
  return code;
};

Blockly.CSharp.controls_flow_statements = function() {
  // Flow statements: continue, break.
  switch (this.getTitleValue('FLOW')) {
    case 'BREAK':
      return 'break;\n';
    case 'CONTINUE':
      return 'continue;\n';
  }
  throw 'Unknown flow statement.';
};
