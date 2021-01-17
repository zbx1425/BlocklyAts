'use strict';

Blockly.CSharp = new Blockly.Generator('CSharp');

Blockly.CSharp.addReservedWords(
    //http://msdn.microsoft.com/en-us/library/x53a06bb.aspx
    'abstract,as,base,bool,break,byte,case,catch,char,checked,class,const,continue,decimal,default,delegate,do,double,else,enum,event,explicit,extern,false,finally,fixed,float,for,foreach,goto,if,implicit,in,int,interface,internal,is,lock,long,namespace,new,null,object,operator,out,override,params,private,protected,public,readonly,ref,return,sbyte,sealed,short,sizeof,stackalloc,static,string,struct,switch,this,throw,true,try,typeof,uint,ulong,unchecked,unsafe,ushort,using,virtual,void,volatile,while'
    );

Blockly.CSharp.ORDER_ATOMIC = 0;         // 0 ""
Blockly.CSharp.ORDER_MEMBER = 1;         // . []
Blockly.CSharp.ORDER_NEW = 1;            // new
Blockly.CSharp.ORDER_TYPEOF = 1;         // typeof
Blockly.CSharp.ORDER_FUNCTION_CALL = 1;  // ()
Blockly.CSharp.ORDER_INCREMENT = 1;      // ++
Blockly.CSharp.ORDER_DECREMENT = 1;      // --
Blockly.CSharp.ORDER_LOGICAL_NOT = 2;    // !
Blockly.CSharp.ORDER_BITWISE_NOT = 2;    // ~
Blockly.CSharp.ORDER_UNARY_PLUS = 2;     // +
Blockly.CSharp.ORDER_UNARY_NEGATION = 2; // -
Blockly.CSharp.ORDER_MULTIPLICATION = 3; // *
Blockly.CSharp.ORDER_DIVISION = 3;       // /
Blockly.CSharp.ORDER_MODULUS = 3;        // %
Blockly.CSharp.ORDER_ADDITION = 4;       // +
Blockly.CSharp.ORDER_SUBTRACTION = 4;    // -
Blockly.CSharp.ORDER_BITWISE_SHIFT = 5;  // << >>
Blockly.CSharp.ORDER_RELATIONAL = 6;     // < <= > >=
Blockly.CSharp.ORDER_EQUALITY = 7;       // == !=
Blockly.CSharp.ORDER_BITWISE_AND = 8;   // &
Blockly.CSharp.ORDER_BITWISE_XOR = 9;   // ^
Blockly.CSharp.ORDER_BITWISE_OR = 10;    // |
Blockly.CSharp.ORDER_LOGICAL_AND = 11;   // &&
Blockly.CSharp.ORDER_LOGICAL_OR = 12;    // ||
Blockly.CSharp.ORDER_CONDITIONAL = 13;   // ?:
Blockly.CSharp.ORDER_ASSIGNMENT = 14;    // = += -= *= /= %= <<= >>= ...
Blockly.CSharp.ORDER_COMMA = 15;         // ,
Blockly.CSharp.ORDER_NONE = 99;          // (...)

/**
 * Arbitrary code to inject into locations that risk causing infinite loops.
 * Any instances of '%1' will be replaced by the block ID that failed.
 * E.g. '  checkTimeout(%1);\n'
 * @type ?string
 */
Blockly.CSharp.INFINITE_LOOP_TRAP = null;

Blockly.CSharp.init = function() {
  Blockly.CSharp.definitions_ = {};

  if (Blockly.Variables) {
    if (!Blockly.CSharp.variableDB_) {
      Blockly.CSharp.variableDB_ =
          new Blockly.Names(Blockly.CSharp.RESERVED_WORDS_);
    } else {
      Blockly.CSharp.variableDB_.reset();
    }

    var defvars = [];
    var variables = Blockly.Variables.allVariables();
    for (var x = 0; x < variables.length; x++) {
      defvars[x] = 'dynamic ' +
          Blockly.CSharp.variableDB_.getName(variables[x],
          Blockly.Variables.NAME_TYPE) + ';';
    }
    Blockly.CSharp.definitions_['variables'] = defvars.join('\n');
  }
};

/* Prepend the generated code with the variable definitions. */
Blockly.CSharp.finish = function(code) {
  var definitions = [];
  for (var name in Blockly.CSharp.definitions_) {
    definitions.push(Blockly.CSharp.definitions_[name]);
  }
  return definitions.join('\n\n') + '\n\n\n' + code;
};

/**
 * Naked values are top-level blocks with outputs that aren't plugged into
 * anything.  A trailing semicolon is needed to make this legal.
 * @param {string} line Line of generated code.
 * @return {string} Legal line of code.
 */
Blockly.CSharp.scrubNakedValue = function(line) {
  return line + ';\n';
};

Blockly.CSharp.quote_ = function(val) {
  return goog.string.quote(val);
};

/**
 * Common tasks for generating CSharp from blocks.
 * Handles comments for the specified block and any connected value blocks.
 * Calls any statements following this block.
 * @param {!Blockly.Block} block The current block.
 * @param {string} code The CSharp code created for this block.
 * @return {string} CSharp code with comments and subsequent blocks added.
 * @this {Blockly.CodeGenerator}
 * @private
 */
Blockly.CSharp.scrub_ = function(block, code) {
  if (code === null) {
    // Block has handled code generation itself.
    return '';
  }
  var commentCode = '';
  // Only collect comments for blocks that aren't inline.
  if (!block.outputConnection || !block.outputConnection.targetConnection) {
    // Collect comment for this block.
    var comment = block.getCommentText();
    if (comment) {
      commentCode += this.prefixLines(comment, '// ') + '\n';
    }
    // Collect comments for all value arguments.
    // Don't collect comments for nested statements.
    for (var x = 0; x < block.inputList.length; x++) {
      if (block.inputList[x].type == Blockly.INPUT_VALUE) {
        var childBlock = block.inputList[x].connection.targetBlock();
        if (childBlock) {
          var comment = this.allNestedComments(childBlock);
          if (comment) {
            commentCode += this.prefixLines(comment, '// ');
          }
        }
      }
    }
  }
  var nextBlock = block.nextConnection && block.nextConnection.targetBlock();
  var nextCode = this.blockToCode(nextBlock);
  return commentCode + code + nextCode;
};
