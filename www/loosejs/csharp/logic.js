'use strict';

Blockly.CSharp.logic = {};

Blockly.CSharp.controls_if = function() {
  // If/elseif/else condition.
  var n = 0;
  var argument = Blockly.CSharp.valueToCode(this, 'IF' + n,
      Blockly.CSharp.ORDER_NONE) || 'false';
  var branch = Blockly.CSharp.statementToCode(this, 'DO' + n);
  var code = 'if (' + argument + ') {\n' + branch + '}';
  for (n = 1; n <= this.elseifCount_; n++) {
    argument = Blockly.CSharp.valueToCode(this, 'IF' + n,
        Blockly.CSharp.ORDER_NONE) || 'false';
    branch = Blockly.CSharp.statementToCode(this, 'DO' + n);
    code += ' else if (' + argument + ') {\n' + branch + '}\n';
  }
  if (this.elseCount_) {
    branch = Blockly.CSharp.statementToCode(this, 'ELSE');
    code += ' else {\n' + branch + '}\n';
  }
  return code + '\n';
};

Blockly.CSharp.logic_compare = function() {
  // Comparison operator.
  var mode = this.getTitleValue('OP');
  var operator = Blockly.CSharp.logic_compare.OPERATORS[mode];
  var order = (operator == '==' || operator == '!=') ?
      Blockly.CSharp.ORDER_EQUALITY : Blockly.CSharp.ORDER_RELATIONAL;
  var argument0 = Blockly.CSharp.valueToCode(this, 'A', order) || 'null';
  var argument1 = Blockly.CSharp.valueToCode(this, 'B', order) || 'null';
  var code = argument0 + ' ' + operator + ' ' + argument1;
  return [code, order];
};

Blockly.CSharp.logic_compare.OPERATORS = {
  EQ: '==',
  NEQ: '!=',
  LT: '<',
  LTE: '<=',
  GT: '>',
  GTE: '>='
};

Blockly.CSharp.logic_operation = function() {
  // Operations 'and', 'or'.
  var operator = (this.getTitleValue('OP') == 'AND') ? '&&' : '||';
  var order = (operator == '&&') ? Blockly.CSharp.ORDER_LOGICAL_AND :
      Blockly.CSharp.ORDER_LOGICAL_OR;
  var argument0 = Blockly.CSharp.valueToCode(this, 'A', order) || 'false';
  var argument1 = Blockly.CSharp.valueToCode(this, 'B', order) || 'false';
  var code = argument0 + ' ' + operator + ' ' + argument1;
  return [code, order];
};

Blockly.CSharp.logic_negate = function() {
  // Negation.
  var order = Blockly.CSharp.ORDER_LOGICAL_NOT;
  var argument0 = Blockly.CSharp.valueToCode(this, 'BOOL', order) ||
      'false';
  var code = '!' + argument0;
  return [code, order];
};

Blockly.CSharp.logic_boolean = function() {
  // Boolean values true and false.
  var code = (this.getTitleValue('BOOL') == 'TRUE') ? 'true' : 'false';
  return [code, Blockly.CSharp.ORDER_ATOMIC];
};

Blockly.CSharp.logic_null = function() {
  // Null data type.
  return ['null', Blockly.CSharp.ORDER_ATOMIC];
};

Blockly.CSharp.logic_ternary = function() {
  // Ternary operator.
  var value_if = Blockly.CSharp.valueToCode(this, 'IF',
      Blockly.CSharp.ORDER_CONDITIONAL) || 'false';
  var value_then = Blockly.CSharp.valueToCode(this, 'THEN',
      Blockly.CSharp.ORDER_CONDITIONAL) || 'null';
  var value_else = Blockly.CSharp.valueToCode(this, 'ELSE',
      Blockly.CSharp.ORDER_CONDITIONAL) || 'null';
  var code = value_if + ' ? ' + value_then + ' : ' + value_else
  return [code, Blockly.CSharp.ORDER_CONDITIONAL];
};
