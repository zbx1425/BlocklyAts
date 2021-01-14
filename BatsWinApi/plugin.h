#pragma once

#define l_setglobalN(name, val) lua_pushnumber(L, val);lua_setglobal(L, name);

#define l_setglobalI(name, val) lua_pushinteger(L, val);lua_setglobal(L, name);

#define l_setglobalF(name, val) lua_pushcfunction(L, val);lua_setglobal(L, name);