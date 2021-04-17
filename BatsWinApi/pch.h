// pch.h: 这是预编译标头文件。
// 下方列出的文件仅编译一次，提高了将来生成的生成性能。
// 这还将影响 IntelliSense 性能，包括代码完成和许多代码浏览功能。
// 但是，如果此处列出的文件中的任何一个在生成之间有更新，它们全部都将被重新编译。
// 请勿在此处添加要频繁更新的文件，这将使得性能优势无效。

#ifndef PCH_H
#define PCH_H

#define ATS_EXPORTS

// 添加要在此处预编译的标头
#include "framework.h"

#include <vector>
#include <queue>

#include "lua54/include/lua.hpp"

#include "atsplugin.h"

#ifdef _WIN64
#pragma comment (lib, "lua54/lua54_x64_static.lib")
#else
#pragma comment (lib, "lua54/lua54_x86_static.lib")
#endif

#define l_setglobalN(name, val) lua_pushnumber(L, val);lua_setglobal(L, name);

#define l_setglobalI(name, val) lua_pushinteger(L, val);lua_setglobal(L, name);

#define l_setglobalF(name, val) lua_pushcfunction(L, val);lua_setglobal(L, name);

#define l_setglobalS(name, val) lua_pushstring(L, val);lua_setglobal(L, name);

#endif //PCH_H
