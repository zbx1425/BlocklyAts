#pragma once

#include "lua54/include/lauxlib.h"
LUAMOD_API int luaopen_winfile(lua_State *L);

struct dir_data {
	HANDLE findfile;
	int closed;
};