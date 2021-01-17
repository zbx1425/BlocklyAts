'use strict';

Blockly.CSharp.procedures = {};

Blockly.CSharp.procedures_defreturn = function() {
  // Define a procedure with a return value.
  var funcName = Blockly.CSharp.variableDB_.getName(
      this.getTitleValue('NAME'), Blockly.Procedures.NAME_TYPE);
  var branch = Blockly.CSharp.statementToCode(this, 'STACK');

  if (Blockly.CSharp.INFINITE_LOOP_TRAP) {
    branch = Blockly.CSharp.INFINITE_LOOP_TRAP.replace(/%1/g,
        '\'' + this.id + '\'') + branch;
  }

  var returnValue = Blockly.CSharp.valueToCode(this, 'RETURN', Blockly.CSharp.ORDER_NONE) || '';
  if (returnValue) {
    returnValue = '  return ' + returnValue + ';\n';
  }

  var args = [];
  for (var x = 0; x < this.arguments_.length; x++) {
    args[x] = Blockly.CSharp.variableDB_.getName(this.arguments_[x],
        Blockly.Variables.NAME_TYPE);
  }

  var append_to_list = function (res, val) {
      if (res.length == 0)
          argTypes = val;
      else
          argTypes += ', ' + val;
  };

  var argTypes = '';
  for (var x = 0; x < args.length; x++) {
      append_to_list(argTypes, 'dynamic');
  }

  if (returnValue.length != 0) {
      append_to_list(argTypes, 'dynamic');
  }

  var delegateType = (returnValue.length == 0) ? 'Action' : ('Func<' + argTypes + '>');

  var code = 'var ' + funcName + ' = new ' + delegateType + '((' + args.join(', ') + ') => {\n' + branch + returnValue + '});';
  code = Blockly.CSharp.scrub_(this, code);
  Blockly.CSharp.definitions_[funcName] = code;
  return null;
};

// Defining a procedure without a return value uses the same generator as
// a procedure with a return value.
Blockly.CSharp.procedures_defnoreturn =
    Blockly.CSharp.procedures_defreturn;

Blockly.CSharp.procedures_callreturn = function() {
  // Call a procedure with a return value.
  var funcName = Blockly.CSharp.variableDB_.getName(
      this.getTitleValue('NAME'), Blockly.Procedures.NAME_TYPE);
  var args = [];
  for (var x = 0; x < this.arguments_.length; x++) {
    args[x] = Blockly.CSharp.valueToCode(this, 'ARG' + x,
        Blockly.CSharp.ORDER_COMMA) || 'null';
  }
  var code = funcName + '(' + args.join(', ') + ')';
  return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
};

Blockly.CSharp.procedures_callnoreturn = function() {
  // Call a procedure with no return value.
  var funcName = Blockly.CSharp.variableDB_.getName(
      this.getTitleValue('NAME'), Blockly.Procedures.NAME_TYPE);
  var args = [];
  for (var x = 0; x < this.arguments_.length; x++) {
    args[x] = Blockly.CSharp.valueToCode(this, 'ARG' + x, Blockly.CSharp.ORDER_COMMA) || 'null';
  }
  var code = funcName + '(' + args.join(', ') + ');\n';
  return code;
};

Blockly.CSharp.procedures_ifreturn = function() {
  // Conditionally return value from a procedure.
  var condition = Blockly.CSharp.valueToCode(this, 'CONDITION', Blockly.CSharp.ORDER_NONE) || 'false';
  var code = 'if (' + condition + ') {\n';
  if (this.hasReturnValue_) {
    var value = Blockly.CSharp.valueToCode(this, 'VALUE', Blockly.CSharp.ORDER_NONE) || 'null';
    code += '  return ' + value + ';\n';
  } else {
    code += '  return;\n';
  }
  code += '}\n';
  return code;
};
