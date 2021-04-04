require "winfile"

-- Start of BlocklyAts boilerplate code.
local LIP={}function LIP.load(b)assert(type(b)=='string','Parameter "fileName" must be a string.')local c=winfile.open(b,'r');if c==nil then;return;end;local d={}local e;for f in c:lines()do local g=f:match('^%[([^%[%]]+)%]$')if g then e=tonumber(g)and tonumber(g)or g;d[e]=d[e]or{}end;local h,i=f:match('^([%w|_]+)%s-=%s-(.+)$')if h and i~=nil then if tonumber(i)then i=tonumber(i)elseif i=='true'then i=true elseif i=='false'then i=false end;if tonumber(h)then h=tonumber(h)end;d[e][h]=i end end;c:close()return d end;function LIP.save(b,d)assert(type(b)=='string','Parameter "fileName" must be a string.')assert(type(d)=='table','Parameter "data" must be a table.')local c=assert(winfile.open(b,'w+b'),'Error loading file :'..b)local j=''for e,h in pairs(d)do j=j..('[%s]\n'):format(e)for k,i in pairs(h)do j=j..('%s=%s\n'):format(k,tostring(i))end;j=j..'\n'end;c:write(j)c:close()end

_bkeystate = {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false}
_bdoorstate = false
_vtimer = {}

function _fcfgload(path)
  _vconfig = LIP.load(_vdlldir..path)
end

function _fcfgsave(path)
  LIP.save(_vdlldir..path, _vconfig)
end

function _fcfgget(part, key, default)
  if _vconfig == nil then; return default; end
  if _vconfig[part] == nil then; return default; end
  if _vconfig[part][key] == nil then; return default; end
  return _vconfig[part][key]
end

function _fcfgset(part, key, value)
  if _vconfig == nil then; _vconfig = {}; end
  if _vconfig[part] == nil then; _vconfig[part] = {}; end
  _vconfig[part][key] = value
end

-- Timer functions
-- _vtimer[<NAME>][1]:  Interval
-- _vtimer[<NAME>][2]:  Should timer repeat?
-- _vtimer[<NAME>][3]:  The time of last trigger
-- _vtimer[<NAME>][4]:  Is enabled?

function _ftimercallhandler(name)
  if _G["_etimertick_"..name] ~= nil then; _G["_etimertick_"..name](); end
end

function _ftimerreset(name, callhandler)
  if _vtimer[name] == nil then; return; end
  _vtimer[name][3] = _bEdTime
  _vtimer[name][4] = true
  if callhandler then; _ftimercallhandler(name); end
end

function _ftimercancel(name, callhandler)
  if _vtimer[name] == nil then; return; end
  _vtimer[name][4] = false;
  if callhandler then; _ftimercallhandler(name); end
end

function _ftimerset(name, interval, cycle)
  if interval > 0 then
    _vtimer[name] = { interval, cycle, 0, false }
    _ftimerreset(name, false)
  else
    _ftimercancel(name, false)
  end
end

function _ftimerupdate()
  for name, param in pairs(_vtimer) do
    if param[4] then
      if param[3] == nil or param[3] > _bEdTime then
        -- Nil means the timer was initialized when ElapseData was not ready
        -- Or from the future? Maybe caused by a station jump, just reset it
        _ftimerreset(name, false)
      elseif _bEdTime >= param[3] + param[1] then
          _ftimercallhandler(name)
          if param[2] then; _ftimerreset(name, false); else; _ftimercancel(name, false); end
      end
    end
  end
end