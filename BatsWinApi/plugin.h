#pragma once

#ifdef BATS_LUA_DYNAMIC
	#ifdef _WIN64
		#pragma comment (lib, "lua54/lua54_x64.lib")
	#else
		#pragma comment (lib, "lua54/lua54_x86.lib")
	#endif
#else
	#ifdef _WIN64
		#pragma comment (lib, "lua54/lua54_x64_static.lib")
	#else
		#pragma comment (lib, "lua54/lua54_x86_static.lib")
	#endif
#endif

#define l_setglobalN(name, val) lua_pushnumber(L, val);lua_setglobal(L, name);

#define l_setglobalI(name, val) lua_pushinteger(L, val);lua_setglobal(L, name);

#define l_setglobalF(name, val) lua_pushcfunction(L, val);lua_setglobal(L, name);

#define l_setglobalS(name, val) lua_pushstring(L, val);lua_setglobal(L, name);