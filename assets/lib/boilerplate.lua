local LIP={}function LIP.load(b)assert(type(b)=='string','Parameter "fileName" must be a string.')local c=io.open(b,'r');if c==nil then;return;end;local d={}local e;for f in c:lines()do local g=f:match('^%[([^%[%]]+)%]$')if g then e=tonumber(g)and tonumber(g)or g;d[e]=d[e]or{}end;local h,i=f:match('^([%w|_]+)%s-=%s-(.+)$')if h and i~=nil then if tonumber(i)then i=tonumber(i)elseif i=='true'then i=true elseif i=='false'then i=false end;if tonumber(h)then h=tonumber(h)end;d[e][h]=i end end;c:close()return d end;function LIP.save(b,d)assert(type(b)=='string','Parameter "fileName" must be a string.')assert(type(d)=='table','Parameter "data" must be a table.')local c=assert(io.open(b,'w+b'),'Error loading file :'..b)local j=''for e,h in pairs(d)do j=j..('[%s]\n'):format(e)for k,i in pairs(h)do j=j..('%s=%s\n'):format(k,tostring(i))end;j=j..'\n'end;c:write(j)c:close()end

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

function _ftimerschd(name, toend)
  if _vtimer[name] == nil then; return; end
  if toend then
    _vtimer[name][2] = _bEdTime - _vtimer[name][0]
  else
    _vtimer[name][2] = _bEdTime
  end
end

function _ftimerset(name, interval, cycle)
  if interval > 0 then
    _vtimer[name] = { interval, cycle, 0 }
    _ftimerschd(name)
  else
    _vtimer[name] = nil
  end
end

function _ftimerelapse()
  for name, param in pairs(_vtimer) do
    if param[2] > _bEdTime then; _ftimerschd(name); end
    if _bEdTime >= param[2] + param[0] then
      _etimertick(name)
      if param[1] then; _ftimerschd(name); else; _ftimerset(name, 0); end
    end
  end
end