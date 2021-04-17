#include "pch.h"
#include "luainterop.h"
#include "globals.h"

void l_printerr() {
	const char * sBuf = lua_tostring(L, -1);
	DWORD dBufSize = MultiByteToWideChar(CP_UTF8, 0, sBuf, -1, NULL, 0);
	std::vector<wchar_t> dBuf(dBufSize);
	int msgBoxResult;
	if (MultiByteToWideChar(CP_UTF8, 0, sBuf, -1, dBuf.data(), dBufSize) > 0) {
		msgBoxResult = MessageBoxW(NULL, dBuf.data(), L"BlocklyAts Lua Script Error", MB_RETRYCANCEL | MB_ICONERROR);
	}
	else {
		msgBoxResult = MessageBoxA(NULL, sBuf, "BlocklyAts Lua Script Error", MB_RETRYCANCEL | MB_ICONERROR);
	}
	if (msgBoxResult == IDCANCEL) {
		lua_close(L);
		L = NULL;
	}
}

static int l_panel_getset(lua_State *L) {
	int id = luaL_checkinteger(L, 1);
	if (id < 0 || id > 255) return luaL_error(L, "_fpanel_getset expects id 0~255");
	if (lua_gettop(L) == 1) {
		lua_pushinteger(L, bvePanel[id]);
		return 1;
	}
	else if (lua_gettop(L) == 2) {
		int val = luaL_checkinteger(L, 2);
		bvePanel[id] = val;
		return 0;
	}
	else {
		return luaL_error(L, "_fpanel_getset expects no more than 2 arguments");
	}
}

static int l_sound_getset(lua_State *L) {
	int id = luaL_checkinteger(L, 1);
	if (id < 0 || id > 255) return luaL_error(L, "_fsound_getset expects id 0~255");
	if (lua_gettop(L) == 1) {
		lua_pushinteger(L, bveSound[id]);
		return 1;
	}
	else if (lua_gettop(L) == 2) {
		int val = luaL_checkinteger(L, 2);
		if (val > 2) {
			bveSound[id] = 2;
		}
		else if (val < -10000) {
			bveSound[id] = -10000;
		}
		else {
			bveSound[id] = val;
		}
		return 0;
	}
	else if (lua_gettop(L) == 3) {
		float vol = luaL_checknumber(L, 3);
		if (vol >= 100) {
			bveSound[id] = 0;
		}
		else if (vol <= 0) {
			bveSound[id] = -10000;
		}
		else {
			bveSound[id] = (int)(vol * 100) - 10000;
		}
		return 0;
	}
	else {
		return luaL_error(L, "_fsound_getset expects no more than 3 arguments");
	}
}

static int l_msgbox(lua_State *L) {
	const char * sBuf = luaL_checkstring(L, 1);
	DWORD dBufSize = MultiByteToWideChar(CP_UTF8, 0, sBuf, -1, NULL, 0);
	std::vector<wchar_t> dBuf(dBufSize);
	if (MultiByteToWideChar(CP_UTF8, 0, sBuf, -1, dBuf.data(), dBufSize) > 0) {
		MessageBoxW(NULL, dBuf.data(), L"BlocklyAts Message", 0);
	}
	else {
		MessageBoxA(NULL, sBuf, "BlocklyAts Message", 0);
	}
	return 0;
}

void open_interop_functions() {
	l_setglobalF("_fpanel", l_panel_getset);
	l_setglobalF("_fsound", l_sound_getset);
	l_setglobalF("_fmsgbox", l_msgbox);
}