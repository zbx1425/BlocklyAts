'use strict';

Blockly.CSharp.variables = {};

Blockly.CSharp.variables_get = function() {
  // Variable getter.
  var code = Blockly.CSharp.variableDB_.getName(this.getTitleValue('VAR'),
      Blockly.Variables.NAME_TYPE);
  return [code, Blockly.CSharp.ORDER_ATOMIC];
};

Blockly.CSharp.variables_set = function() {
  // Variable setter.
  var argument0 = Blockly.CSharp.valueToCode(this, 'VALUE',
      Blockly.CSharp.ORDER_ASSIGNMENT) || 'null';
  var varName = Blockly.CSharp.variableDB_.getName(
      this.getTitleValue('VAR'), Blockly.Variables.NAME_TYPE);
  return varName + ' = ' + argument0 + ';\n';
};
