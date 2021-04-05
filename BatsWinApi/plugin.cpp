#include "pch.h"
#include "framework.h"
#include "plugin.h"
#include "winfile.h"

lua_State *L = NULL;

ATS_VEHICLESPEC vSpec;
int phPower, phBrake, phReverser;
int *bvePanel, *bveSound;

void l_printerr() {
	const char * sBuf = lua_tostring(L, -1);
	DWORD dBufSize = MultiByteToWideChar(CP_UTF8, 0, sBuf, -1, NULL, 0);
	std::vector<wchar_t> dBuf(dBufSize);
	int msgBoxResult;
	if (MultiByteToWideChar(CP_UTF8, 0, sBuf, -1, dBuf.data(), dBufSize) > 0) {
		msgBoxResult = MessageBoxW(NULL, dBuf.data(), L"BlocklyAts Lua Script Error", MB_RETRYCANCEL | MB_ICONERROR);
	} else {
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
	} else if (lua_gettop(L) == 2) {
		int val = luaL_checkinteger(L, 2);
		bvePanel[id] = val;
		return 0;
	} else {
		return luaL_error(L, "_fpanel_getset expects no more than 2 arguments");
	}
}

static int l_sound_getset(lua_State *L) {
	int id = luaL_checkinteger(L, 1);
	if (id < 0 || id > 255) return luaL_error(L, "_fsound_getset expects id 0~255");
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
		return luaL_error(L, "_fsound_getset expects no more than 3 arguments");
	}
}

static int l_msgbox(lua_State *L) {
	const char * sBuf = luaL_checkstring(L, 1);
	DWORD dBufSize = MultiByteToWideChar(CP_UTF8, 0, sBuf, -1, NULL, 0);
	std::vector<wchar_t> dBuf(dBufSize);
	if (MultiByteToWideChar(CP_UTF8, 0, sBuf, -1, dBuf.data(), dBufSize) > 0) {
		MessageBoxW(NULL, dBuf.data(), L"BlocklyAts Message", 0);
	} else {
		MessageBoxA(NULL, sBuf, "BlocklyAts Message", 0);
	}
	return 0;
}


wchar_t dllPath[2048];
char buffer[4096];
DWORD read; char* luaCode;

ATS_API void WINAPI Load() {
	HMODULE hm = NULL;

	if (GetModuleHandleEx(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT,
		(LPCWSTR)&Dispose, &hm) == 0) {
		MessageBox(NULL, L"GetModuleHandleEx failed, ATS plugin cannot load", L"BlocklyAts Error", MB_ICONERROR);
		return;
	}
	if (GetModuleFileName(hm, dllPath, sizeof(dllPath)) == 0) {
		MessageBox(NULL, L"GetModuleFileName failed, ATS plugin cannot load", L"BlocklyAts Error", MB_ICONERROR);
		return;
	}

	HANDLE hFile = CreateFile(dllPath, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	if (INVALID_HANDLE_VALUE == hFile) {
		MessageBox(NULL, L"CreateFile failed, ATS plugin cannot load", L"BlocklyAts Error", MB_ICONERROR);
		return;
	}

	ReadFile(hFile, buffer, sizeof(buffer), &read, NULL);
	// The previous solution of calculating PE section size doesn't seem to work on 64-bit DLL
	// I should've tried to find out why, but I cannot find how to solve this problem
	DWORD exesize = *(int*)(buffer + 0x6C); // the word "DOS " in DOS stub will be replaced to describe the size
	DWORD filesize = GetFileSize(hFile, NULL);
	if (filesize - exesize <= 0) {
		CloseHandle(hFile);
		MessageBox(NULL, L"No extra code, ATS plugin cannot load", L"BlocklyAts Error", MB_ICONERROR);
		return;
	}

	SetFilePointer(hFile, exesize, NULL, FILE_BEGIN);
	luaCode = new char[filesize - exesize + 1];
	ReadFile(hFile, luaCode, filesize - exesize, &read, NULL);
	CloseHandle(hFile);

	// I knew I should've used bytecode, but then
	// it'll be more difficult to compile on Mono, because you'll need to ship luac with different arch
	// and somehow I could not get it to work, and I could not find out why
	// So there is a quite easy xor obfuscation, to prevent the code from being stolen too easily
	char confusion[] = { 0x11, 0x45, 0x14, 0x19, 0x19, 0x81, 0x14, 0x25 };
	for (DWORD i = 0; i < filesize - exesize; i++) luaCode[i] ^= confusion[i%8];

	L = luaL_newstate();
	luaL_openlibs(L);
	luaL_requiref(L, "winfile", luaopen_winfile, 1);

	l_setglobalF("_fpanel", l_panel_getset);
	l_setglobalF("_fsound", l_sound_getset);
	l_setglobalF("_fmsgbox", l_msgbox);

	*(wcsrchr(dllPath, '\\') + 1) = 0;
	const char * sBuf = lua_tostring(L, -1);
	DWORD dBufSize = WideCharToMultiByte(CP_UTF8, 0, dllPath, -1, NULL, 0, NULL, FALSE);
	char* dllPathMB = new char[dBufSize];
	WideCharToMultiByte(CP_UTF8, 0, dllPath, -1, dllPathMB, dBufSize, NULL, FALSE);
	l_setglobalS("_vdlldir", dllPathMB);

	int error = luaL_loadbuffer(L, luaCode, filesize - exesize, "bats") || lua_pcall(L, 0, LUA_MULTRET, 0);

	if (error) {
		l_printerr();
	} else {
		lua_getglobal(L, "_eload");
		if (lua_pcall(L, 0, 0, 0) != 0) l_printerr();
	}
}

ATS_API void WINAPI Dispose() {
	if (L == NULL) return;
	lua_getglobal(L, "_edispose");
	if (lua_pcall(L, 0, 0, 0) != 0) l_printerr();
	lua_close(L);
}

ATS_API int WINAPI GetPluginVersion() {
	return ATS_VERSION;
}

ATS_API void WINAPI SetVehicleSpec(ATS_VEHICLESPEC vehicleSpec) {
	vSpec = vehicleSpec;
	if (L == NULL) return;
	l_setglobalI("_bVsAtsNotch", vehicleSpec.AtsNotch);
	l_setglobalI("_bVsB67Notch", vehicleSpec.B67Notch);
	l_setglobalI("_bVsBrakeNotches", vehicleSpec.BrakeNotches);
	l_setglobalI("_bVsCars", vehicleSpec.Cars);
	l_setglobalI("_bVsPowerNotches", vehicleSpec.PowerNotches);
}

ATS_API void WINAPI Initialize(int initIndex) {
	if (L == NULL) return;
	lua_getglobal(L, "_einitialize");
	lua_pushinteger(L, initIndex);
	if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
}

bool firstElapse = true;
bool schdSetSignal = false, schdSetBeacon = false, schdDoorChange = false;
int sdatSignal; bool sdatDoor; ATS_BEACONDATA sdatBeacon;

ATS_API ATS_HANDLES WINAPI Elapse(ATS_VEHICLESTATE vehicleState, int *panel, int *sound) {
	ATS_HANDLES result = { phBrake, phPower, phReverser, 2 };
	if (L == NULL) return result;
	bvePanel = panel;
	bveSound = sound;
	l_setglobalN("_bEdLocation", vehicleState.Location);
	l_setglobalN("_bEdSpeed", vehicleState.Speed);
	l_setglobalI("_bEdTime", vehicleState.Time);
	l_setglobalN("_bEdBcPressure", vehicleState.BcPressure);
	l_setglobalN("_bEdMrPressure", vehicleState.MrPressure);
	l_setglobalN("_bEdErPressure", vehicleState.ErPressure);
	l_setglobalN("_bEdBpPressure", vehicleState.BpPressure);
	l_setglobalN("_bEdSapPressure", vehicleState.SapPressure);
	l_setglobalN("_bEdCurrent", vehicleState.Current);
	if (firstElapse) {
		// Let the internal LastSound inside BVE to be -10000
		// So that new sounds can start from the first plugin Elapse
		for (int i = 0; i < 256; i++) bveSound[i] = -10000;
		firstElapse = false;
		return result;
	}
	if (schdDoorChange) {
		schdDoorChange = false;
		lua_getglobal(L, "_edoorchange");
		lua_pushboolean(L, sdatDoor);
		if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
		if (L == NULL) return result;
	}
	if (schdSetSignal) {
		schdSetSignal = false;
		lua_getglobal(L, "_esetsignal");
		lua_pushinteger(L, sdatSignal);
		if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
		if (L == NULL) return result;
	}
	if (schdSetBeacon) {
		schdSetBeacon = false;
		lua_getglobal(L, "_esetbeacondata");
		lua_pushnumber(L, sdatBeacon.Distance);
		lua_pushinteger(L, sdatBeacon.Optional);
		lua_pushinteger(L, sdatBeacon.Signal);
		lua_pushinteger(L, sdatBeacon.Type);
		if (lua_pcall(L, 4, 0, 0) != 0) l_printerr();
		if (L == NULL) return result;
	}
	lua_getglobal(L, "_eelapse");
	lua_pushinteger(L, phPower);
	lua_pushinteger(L, phBrake);
	lua_pushinteger(L, phReverser);
	lua_pushinteger(L, 2);
	if (lua_pcall(L, 4, 4, 0) != 0) {
		l_printerr();
		// Apply emergency brake
		// Lt's hope it'll get the plugin out of the error state by chance
		if (L != NULL) return { vSpec.BrakeNotches + 1, 0, 1, 2 }; else return result;
	} else {
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
	l_setglobalI("_bHPower", notch);
}

ATS_API void WINAPI SetBrake(int notch) {
	phBrake = notch;
	if (L == NULL) return;
	l_setglobalI("_bHBrake", notch);
}

ATS_API void WINAPI SetReverser(int pos) {
	phReverser = pos;
	if (L == NULL) return;
	l_setglobalI("_bHReverser", pos);
}

ATS_API void WINAPI KeyDown(int atsKeyCode) {
	if (L == NULL) return;
	lua_getglobal(L, "_ekeydown");
	lua_pushinteger(L, atsKeyCode);
	if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
}

ATS_API void WINAPI KeyUp(int atsKeyCode) {
	if (L == NULL) return;
	lua_getglobal(L, "_ekeyup");
	lua_pushinteger(L, atsKeyCode);
	if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
}

ATS_API void WINAPI HornBlow(int atsHornBlowIndex) {
	if (L == NULL) return;
	lua_getglobal(L, "_ehornblow");
	lua_pushinteger(L, atsHornBlowIndex);
	if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
}

ATS_API void WINAPI DoorOpen() {
	if (L == NULL) return;
	schdDoorChange = true;
	sdatDoor = true;
}

ATS_API void WINAPI DoorClose() {
	if (L == NULL) return;
	schdDoorChange = true;
	sdatDoor = false;
}

ATS_API void WINAPI SetSignal(int signal) {
	if (L == NULL) return;
	schdSetSignal = true;
	sdatSignal = signal;
}

ATS_API void WINAPI SetBeaconData(ATS_BEACONDATA beaconData) {
	if (L == NULL) return;
	schdSetBeacon = true;
	sdatBeacon = beaconData;
}