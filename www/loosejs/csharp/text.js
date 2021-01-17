'use strict';

Blockly.CSharp.text = {};

Blockly.CSharp.text = function() {
  // Text value.
  var code = Blockly.CSharp.quote_(this.getTitleValue('TEXT'));
  return [code, Blockly.CSharp.ORDER_ATOMIC];
};

Blockly.CSharp.text_join = function() {
  // Create a string made up of any number of elements of any type.
  var code;
  if (this.itemCount_ == 0) {
    return ['""', Blockly.CSharp.ORDER_ATOMIC];
  } else if (this.itemCount_ == 1) {
    var argument0 = Blockly.CSharp.valueToCode(this, 'ADD0', Blockly.CSharp.ORDER_NONE) || '""';
    code = argument0 + '.ToString()';
    return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
  } else if (this.itemCount_ == 2) {
    var argument0 = Blockly.CSharp.valueToCode(this, 'ADD0', Blockly.CSharp.ORDER_NONE) || '""';
    var argument1 = Blockly.CSharp.valueToCode(this, 'ADD1', Blockly.CSharp.ORDER_NONE) || '""';
    code = 'String.Concat(' + argument0 + ', ' + argument1 + ')';
    return [code, Blockly.CSharp.ORDER_ADDITION];
  } else {
    code = new Array(this.itemCount_);
    for (var n = 0; n < this.itemCount_; n++) {
      code[n] = Blockly.CSharp.valueToCode(this, 'ADD' + n, Blockly.CSharp.ORDER_COMMA) || '""';
    }
    code = 'String.Concat(' + code.join(', ') + ')';
    return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
  }
};

Blockly.CSharp.text_append = function() {
  // Append to a variable in place.
  var varName = Blockly.CSharp.variableDB_.getName(this.getTitleValue('VAR'), Blockly.Variables.NAME_TYPE);
  var argument0 = Blockly.CSharp.valueToCode(this, 'TEXT',
      Blockly.CSharp.ORDER_NONE) || '""';
  return varName + ' = String.Concat(' + varName + ', ' + argument0 + ');\n';
};

Blockly.CSharp.text_length = function() {
  // String length.
  var argument0 = Blockly.CSharp.valueToCode(this, 'VALUE', Blockly.CSharp.ORDER_FUNCTION_CALL) || '""';
  return [argument0 + '.Length', Blockly.CSharp.ORDER_MEMBER];
};

Blockly.CSharp.text_isEmpty = function() {
  // Is the string null?
  var argument0 = Blockly.CSharp.valueToCode(this, 'VALUE', Blockly.CSharp.ORDER_MEMBER) || '""';
  return [argument0 + '.Length == 0', Blockly.CSharp.ORDER_EQUALITY];
};

Blockly.CSharp.text_indexOf = function() {
  // Search the text for a substring.
  var operator = this.getTitleValue('END') == 'FIRST' ?
      'IndexOf' : 'LastIndexOf';
  var argument0 = Blockly.CSharp.valueToCode(this, 'FIND', Blockly.CSharp.ORDER_NONE) || '""';
  var argument1 = Blockly.CSharp.valueToCode(this, 'VALUE', Blockly.CSharp.ORDER_MEMBER) || '""';
  var code = argument1 + '.' + operator + '(' + argument0 + ') + 1';
  return [code, Blockly.CSharp.ORDER_MEMBER];
};

Blockly.CSharp.text_charAt = function() {
  var where = this.getTitleValue('WHERE') || 'FROM_START';
  var at = Blockly.CSharp.valueToCode(this, 'AT',
      Blockly.CSharp.ORDER_UNARY_NEGATION) || '1';
  var text = Blockly.CSharp.valueToCode(this, 'VALUE',
      Blockly.CSharp.ORDER_MEMBER) || '""';

  // Blockly uses one-based indicies.

  switch (where) {
    case 'FIRST':
      var code = text + '.First()';
      return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
    case 'LAST':
      var code = text + '.Last()';
      return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
    case 'FROM_START':
      var code = text + '[' + at + ' - 1]';
      return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
    case 'FROM_END':
        var code = text + '[text.Length - ' + at + ']';
      return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
    case 'RANDOM':
      if (!Blockly.CSharp.definitions_['text_random_letter']) {
        var functionName = Blockly.CSharp.variableDB_.getDistinctName(
            'text_random_letter', Blockly.Generator.NAME_TYPE);
        Blockly.CSharp.text_charAt.text_random_letter = functionName;
        var func = [];
        func.push('var ' + functionName + ' = new Func<string, char>((text) => {');
        func.push('  var x = (new Random()).Next(text.length);');
        func.push('  return text[x];');
        func.push('});');
        Blockly.CSharp.definitions_['text_random_letter'] = func.join('\n');
      }
      code = Blockly.CSharp.text_charAt.text_random_letter +
          '(' + text + ')';
      return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
  }
  throw 'Unhandled option (text_charAt).';
};

Blockly.CSharp.text_getSubstring = function() {
  // Get substring.
  var text = Blockly.CSharp.valueToCode(this, 'STRING', Blockly.CSharp.ORDER_MEMBER) || 'null';
  var where1 = this.getTitleValue('WHERE1');
  var where2 = this.getTitleValue('WHERE2');
  var at1 = Blockly.CSharp.valueToCode(this, 'AT1', Blockly.CSharp.ORDER_NONE) || '1';
  var at2 = Blockly.CSharp.valueToCode(this, 'AT2', Blockly.CSharp.ORDER_NONE) || '1';
  if (where1 == 'FIRST' && where2 == 'LAST') {
    var code = text;
  } else {
    if (!Blockly.CSharp.definitions_['text_get_substring']) {
      var functionName = Blockly.CSharp.variableDB_.getDistinctName(
          'text_get_substring', Blockly.Generator.NAME_TYPE);
      Blockly.CSharp.text_getSubstring.func = functionName;
      var func = [];
      func.push('var ' + functionName + ' = new Func<string, dynamic, int, dynamic, int, string>((text, where1, at1, where2, at2) => {');
      func.push('  var getAt =new Func<dynamic, int, int>((where, at) => {');
      func.push('    if (where == "FROM_START") {');
      func.push('      at--;');
      func.push('    } else if (where == "FROM_END") {');
      func.push('      at = text.Length - at;');
      func.push('    } else if (where == "FIRST") {');
      func.push('      at = 0;');
      func.push('    } else if (where == "LAST") {');
      func.push('      at = text.Length - 1;');
      func.push('    } else {');
      func.push('      throw new ApplicationException("Unhandled option (text_getSubstring).");');
      func.push('    }');
      func.push('    return at;');
      func.push('  });');
      func.push('  at1 = getAt(where1, at1);');
      func.push('  at2 = getAt(where2, at2) + 1;');
      func.push('  return text.Substring(at1, at2 - at1);');
      func.push('});');
      Blockly.CSharp.definitions_['text_get_substring'] =
          func.join('\n');
    }
    var code = Blockly.CSharp.text_getSubstring.func + '(' + text + ', "' + where1 + '", ' + at1 + ', "' + where2 + '", ' + at2 + ')';
  }
  return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
};

Blockly.CSharp.text_changeCase = function() {
  // Change capitalization.
  var mode = this.getTitleValue('CASE');
  var operator = Blockly.CSharp.text_changeCase.OPERATORS[mode];
  var code;
  if (operator) {
    // Upper and lower case are functions built into CSharp.
    var argument0 = Blockly.CSharp.valueToCode(this, 'TEXT', Blockly.CSharp.ORDER_MEMBER) || '""';
    code = argument0 + operator;
  } else {
    if (!Blockly.CSharp.definitions_['text_toTitleCase']) {
      // Title case is not a native CSharp function.  Define one.
      var functionName = Blockly.CSharp.variableDB_.getDistinctName('text_toTitleCase', Blockly.Generator.NAME_TYPE);
      Blockly.CSharp.text_changeCase.toTitleCase = functionName;
      var func = [];
      func.push('var ' + functionName + ' = new Func<string, string>((str) => {');
      func.push('  var buf = new StringBuilder(str.Length);');
      func.push('  var toUpper = true;');
      func.push('  foreach (var ch in str) {');
      func.push('    buf.Append(toUpper ? Char.ToUpper(ch) : ch);');
      func.push('    toUpper = Char.IsWhiteSpace(ch);');
      func.push('  }');
      func.push('  return buf.ToString();');
      func.push('});');
      Blockly.CSharp.definitions_['text_toTitleCase'] = func.join('\n');
    }
    var argument0 = Blockly.CSharp.valueToCode(this, 'TEXT',
        Blockly.CSharp.ORDER_NONE) || '""';
    code = Blockly.CSharp.text_changeCase.toTitleCase + '(' + argument0 + ')';
  }
  return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
};

Blockly.CSharp.text_changeCase.OPERATORS = {
  UPPERCASE: '.ToUpper()',
  LOWERCASE: '.ToLower()',
  TITLECASE: null
};

Blockly.CSharp.text_trim = function() {
  // Trim spaces.
  var mode = this.getTitleValue('MODE');
  var operator = Blockly.CSharp.text_trim.OPERATORS[mode];
  var argument0 = Blockly.CSharp.valueToCode(this, 'TEXT', Blockly.CSharp.ORDER_MEMBER) || '""';
  return [argument0 + operator, Blockly.CSharp.ORDER_FUNCTION_CALL];
};

Blockly.CSharp.text_trim.OPERATORS = {
  LEFT: '.TrimStart()',
  RIGHT: '.TrimEnd()',
  BOTH: '.Trim()'
};

Blockly.CSharp.text_print = function() {
  // Print statement.
  var argument0 = Blockly.CSharp.valueToCode(this, 'TEXT', Blockly.CSharp.ORDER_NONE) || '""';
  return 'Console.WriteLine(' + argument0 + ');\n';
};

Blockly.CSharp.text_prompt = function () {
    var msg = Blockly.CSharp.quote_(this.getTitleValue('TEXT'));
    var toNumber = this.getTitleValue('TYPE') == 'NUMBER';

    var functionName = Blockly.CSharp.variableDB_.getDistinctName('text_promptInput', Blockly.Generator.NAME_TYPE);
    Blockly.CSharp.text_prompt.promptInput = functionName;
    var func = [];
    func.push('var ' + functionName + ' = new Func<string, bool, dynamic>((msg, toNumber) => {');
    func.push('  Console.WriteLine(msg);');
    func.push('  var res = Console.ReadLine();');
    func.push('  if (toNumber)');
    func.push('    return Double.Parse(res);');
    func.push('  return res;');
    func.push('});');
    Blockly.CSharp.definitions_['text_promptInput'] = func.join('\n');

    var code = Blockly.CSharp.text_prompt.promptInput + '(' + msg + ', ' + toNumber + ')';
    return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
};
