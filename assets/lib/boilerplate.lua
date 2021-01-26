local LIP={}function LIP.load(b)assert(type(b)=='string','Parameter "fileName" must be a string.')local c=io.open(b,'r');if c==nil then;return;end;local d={}local e;for f in c:lines()do local g=f:match('^%[([^%[%]]+)%]$')if g then e=tonumber(g)and tonumber(g)or g;d[e]=d[e]or{}end;local h,i=f:match('^([%w|_]+)%s-=%s-(.+)$')if h and i~=nil then if tonumber(i)then i=tonumber(i)elseif i=='true'then i=true elseif i=='false'then i=false end;if tonumber(h)then h=tonumber(h)end;d[e][h]=i end end;c:close()return d end;function LIP.save(b,d)assert(type(b)=='string','Parameter "fileName" must be a string.')assert(type(d)=='table','Parameter "data" must be a table.')local c=assert(io.open(b,'w+b'),'Error loading file :'..b)local j=''for e,h in pairs(d)do j=j..('[%s]\n'):format(e)for k,i in pairs(h)do j=j..('%s=%s\n'):format(k,tostring(i))end;j=j..'\n'end;c:write(j)c:close()end
function __atsfnc_cfgload(path)
  __atsval_config=LIP.load(__atsval_dlldir..path)
end
function __atsfnc_cfgsave(path)
  LIP.save(__atsval_dlldir..path,__atsval_config)
end
function __atsfnc_cfgget(part,key,default)
  if __atsval_config==nil then;return default;end
  if __atsval_config[part]==nil then;return default;end
  if __atsval_config[part][key]==nil then;return default;end
  return __atsval_config[part][key]
end
function __atsfnc_cfgset(part,key,value)
  if __atsval_config==nil then;__atsval_config={};end
  if __atsval_config[part]==nil then;__atsval_config[part]={};end
  __atsval_config[part][key]=value
end
__bve_keystate = {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false}
__bve_doorstate = false

