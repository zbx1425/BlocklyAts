#include "pch.h"
#include "framework.h"
#include "plugin.h"

#pragma comment(lib, "delayimp")

#if defined(_WIN64)
#pragma comment (lib, "lua64/lua54.lib")
#elif defined(_WIN32)
#pragma comment (lib, "lua32/lua54.lib")
#endif

lua_State *L = NULL;

char selfPath[MAX_PATH];
char programPath[MAX_PATH];

ATS_VEHICLESPEC vSpec;
int phPower, phBrake, phReverser;
int *bvePanel, *bveSound;

void l_printerr() {
	const char * err = lua_tostring(L, -1);
	if (MessageBoxA(NULL, err, "BlocklyATS Lua Script Error", MB_RETRYCANCEL | MB_ICONERROR) == IDCANCEL) {
		lua_close(L);
		L = NULL;
	}
}

static int l_panel_getset(lua_State *L) {
	int id = luaL_checkinteger(L, 1);
	if (id < 0 || id > 255) return luaL_error(L, "__atsfnc_panel_getset expects id 0~255");
	if (lua_gettop(L) == 1) {
		lua_pushinteger(L, bvePanel[id]);
		return 1;
	} else if (lua_gettop(L) == 2) {
		int val = luaL_checkinteger(L, 2);
		bvePanel[id] = val;
		return 0;
	} else {
		return luaL_error(L, "__atsfnc_panel_getset expects no more than 2 arguments");
	}
}

static int l_sound_getset(lua_State *L) {
	int id = luaL_checkinteger(L, 1);
	if (id < 0 || id > 255) return luaL_error(L, "__atsfnc_sound_getset expects id 0~255");
	if (lua_gettop(L) == 1) {
		lua_pushinteger(L, bveSound[id]);
		return 1;
	} else if (lua_gettop(L) == 2) {
		int val = luaL_checkinteger(L, 2);
		if (val > 2) {
			bveSound[id] = 2;
		} else if (val < -10000) {
			bveSound[id] = -10000;
		} else {
			bveSound[id] = val;
		}
		return 0;
	} else if (lua_gettop(L) == 3) {
		float vol = luaL_checknumber(L, 3);
		if (vol >= 100) {
			bveSound[id] = 0;
		} else if (vol <= 0) {
			bveSound[id] = -10000;
		} else {
			bveSound[id] = (int)(vol * 100) - 10000;
		}
		return 0;
	} else {
		return luaL_error(L, "__atsfnc_sound_getset expects no more than 3 arguments");
	}
}

ATS_API void WINAPI Load() {
	HMODULE hm = NULL;

	if (GetModuleHandleExA(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT,
		(LPCSTR)&Dispose, &hm) == 0) {
		MessageBoxA(NULL, "GetModuleHandle failed, ATS plugin cannot load", "BlocklyATS Error", MB_ICONERROR);
		return;
	}
	if (GetModuleFileNameA(hm, selfPath, sizeof(selfPath)) == 0) {
		MessageBoxA(NULL, "GetModuleFileName failed, ATS plugin cannot load", "BlocklyATS Error", MB_ICONERROR);
		return;
	}

	L = luaL_newstate();
	luaL_openlibs(L);

	l_setglobalF("__atsfnc_panel", l_panel_getset);
	l_setglobalF("__atsfnc_sound", l_sound_getset);

	*(strrchr(selfPath, '\\') + 1) = 0;
	sprintf_s(programPath, "%s\\program.lua", selfPath);

	if (luaL_dofile(L, programPath) != 0) {
		l_printerr();
	} else {
		lua_getglobal(L, "__atsapi_load");
		if (lua_pcall(L, 0, 0, 0) != 0) l_printerr();
	}
}

ATS_API void WINAPI Dispose() {
	if (L == NULL) return;
	lua_getglobal(L, "__atsapi_dispose");
	if (lua_pcall(L, 0, 0, 0) != 0) l_printerr();
	lua_close(L);
}

ATS_API int WINAPI GetPluginVersion() {
	return ATS_VERSION;
}

ATS_API void WINAPI SetVehicleSpec(ATS_VEHICLESPEC vehicleSpec) {
	vSpec = vehicleSpec;
	if (L == NULL) return;
	l_setglobalI("__bve_vsAtsNotch", vehicleSpec.AtsNotch);
	l_setglobalI("__bve_vsB67Notch", vehicleSpec.B67Notch);
	l_setglobalI("__bve_vsBrakeNotches", vehicleSpec.BrakeNotches);
	l_setglobalI("__bve_vsCars", vehicleSpec.Cars);
	l_setglobalI("__bve_vsPowerNotches", vehicleSpec.PowerNotches);
}

ATS_API void WINAPI Initialize(int initIndex) {
	if (L == NULL) return;
	lua_getglobal(L, "__atsapi_initialize");
	lua_pushinteger(L, initIndex);
	if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
}

ATS_API ATS_HANDLES WINAPI Elapse(ATS_VEHICLESTATE vehicleState, int *panel, int *sound) {
	if (L == NULL) return { phBrake, phPower, phReverser, 2 };
	bvePanel = panel;
	bveSound = sound;
	l_setglobalN("__bve_edLocation", vehicleState.Location);
	l_setglobalN("__bve_edSpeed", vehicleState.Speed);
	l_setglobalI("__bve_edTime", vehicleState.Time);
	l_setglobalN("__bve_edBcPressure", vehicleState.BcPressure);
	l_setglobalN("__bve_edMrPressure", vehicleState.MrPressure);
	l_setglobalN("__bve_edErPressure", vehicleState.ErPressure);
	l_setglobalN("__bve_edBpPressure", vehicleState.BpPressure);
	l_setglobalN("__bve_edSapPressure", vehicleState.SapPressure);
	l_setglobalN("__bve_edCurrent", vehicleState.Current);
	lua_getglobal(L, "__atsapi_elapse");
	lua_pushinteger(L, phPower);
	lua_pushinteger(L, phBrake);
	lua_pushinteger(L, phReverser);
	lua_pushinteger(L, 2);
	if (lua_pcall(L, 4, 4, 0) != 0) {
		l_printerr();
		return { phBrake, phPower, phReverser, 2 };
	} else {
		ATS_HANDLES result;
		result.Power = lua_tointeger(L, -4);
		result.Brake = lua_tointeger(L, -3);
		result.Reverser = lua_tointeger(L, -2);
		result.ConstantSpeed = lua_tointeger(L, -1);
		lua_pop(L, 4);
		return result;
	}
}

ATS_API void WINAPI SetPower(int notch) {
	phPower = notch;
	if (L == NULL) return;
	l_setglobalI("__bve_hPower", notch);
}

ATS_API void WINAPI SetBrake(int notch) {
	phBrake = notch;
	if (L == NULL) return;
	l_setglobalI("__bve_hBrake", notch);
}

ATS_API void WINAPI SetReverser(int pos) {
	phReverser = pos;
	if (L == NULL) return;
	l_setglobalI("__bve_hReverser", pos);
}

ATS_API void WINAPI KeyDown(int atsKeyCode) {
	if (L == NULL) return;
	lua_getglobal(L, "__atsapi_keydown");
	lua_pushinteger(L, atsKeyCode);
	if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
}

ATS_API void WINAPI KeyUp(int atsKeyCode) {
	if (L == NULL) return;
	lua_getglobal(L, "__atsapi_keyup");
	lua_pushinteger(L, atsKeyCode);
	if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
}

ATS_API void WINAPI HornBlow(int atsHornBlowIndex) {
	if (L == NULL) return;
	lua_getglobal(L, "__atsapi_hornblow");
	lua_pushinteger(L, atsHornBlowIndex);
	if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
}

ATS_API void WINAPI DoorOpen() {
	if (L == NULL) return;
	lua_getglobal(L, "__atsapi_doorchange");
	lua_pushinteger(L, 1);
	if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
}

ATS_API void WINAPI DoorClose() {
	if (L == NULL) return;
	lua_getglobal(L, "__atsapi_doorchange");
	lua_pushinteger(L, 0);
	if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
}

ATS_API void WINAPI SetSignal(int signal) {
	if (L == NULL) return;
	lua_getglobal(L, "__atsapi_setsignal");
	lua_pushinteger(L, signal);
	if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
}

ATS_API void WINAPI SetBeaconData(ATS_BEACONDATA beaconData) {
	if (L == NULL) return;
	lua_getglobal(L, "__atsapi_setbeacondata");
	lua_pushnumber(L, beaconData.Distance);
	lua_pushinteger(L, beaconData.Optional);
	lua_pushinteger(L, beaconData.Signal);
	lua_pushinteger(L, beaconData.Type);
	if (lua_pcall(L, 4, 0, 0) != 0) l_printerr();
}