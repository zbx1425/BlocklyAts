#include "pch.h"
#include "framework.h"
#include "plugin.h"

lua_State *L = NULL;

ATS_VEHICLESPEC vSpec;
int phPower, phBrake, phReverser;
int *bvePanel, *bveSound;

void l_printerr() {
	const char * err = lua_tostring(L, -1);
	if (MessageBoxA(NULL, err, "BlocklyAts Lua Script Error", MB_RETRYCANCEL | MB_ICONERROR) == IDCANCEL) {
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

static int l_msgbox(lua_State *L) {
	const char* msg = luaL_checkstring(L, 1);
	MessageBoxA(NULL, msg, "BlocklyAts Message", 0);
	return 0;
}


char dllPath[2048], programPath[2048], buffer[4096];
DWORD read; char* luaCode;

ATS_API void WINAPI Load() {
	HMODULE hm = NULL;

	if (GetModuleHandleExA(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT,
		(LPCSTR)&Dispose, &hm) == 0) {
		MessageBoxA(NULL, "GetModuleHandle failed, ATS plugin cannot load", "BlocklyAts Error", MB_ICONERROR);
		return;
	}
	if (GetModuleFileNameA(hm, dllPath, sizeof(dllPath)) == 0) {
		MessageBoxA(NULL, "GetModuleFileName failed, ATS plugin cannot load", "BlocklyAts Error", MB_ICONERROR);
		return;
	}

	HANDLE hFile = CreateFileA(dllPath, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	if (INVALID_HANDLE_VALUE == hFile) {
		MessageBoxA(NULL, "CreateFileA failed, ATS plugin cannot load", "BlocklyAts Error", MB_ICONERROR);
		return;
	}
	ReadFile(hFile, buffer, sizeof(buffer), &read, NULL);
	IMAGE_DOS_HEADER* dosheader = (IMAGE_DOS_HEADER*)buffer;
	IMAGE_NT_HEADERS32* header = (IMAGE_NT_HEADERS32*)(buffer + dosheader->e_lfanew);
	if (dosheader->e_magic != IMAGE_DOS_SIGNATURE || header->Signature != IMAGE_NT_SIGNATURE) {
		CloseHandle(hFile);
		MessageBoxA(NULL, "PE header malformed, ATS plugin cannot load", "BlocklyAts Error", MB_ICONERROR);
		return;
	}

	IMAGE_SECTION_HEADER* sectiontable = (IMAGE_SECTION_HEADER*)((BYTE*)header + sizeof(IMAGE_NT_HEADERS32));
	DWORD maxpointer = 0, exesize = 0;
	for (int i = 0; i < header->FileHeader.NumberOfSections; i++) {
		if (sectiontable->PointerToRawData > maxpointer) {
			maxpointer = sectiontable->PointerToRawData;
			exesize = sectiontable->PointerToRawData + sectiontable->SizeOfRawData;
		}
		sectiontable++;
	}

	DWORD filesize = GetFileSize(hFile, NULL);
	if (filesize - exesize <= 0) {
		CloseHandle(hFile);
		MessageBoxA(NULL, "Cannot locate PE terminal offset, ATS plugin cannot load", "BlocklyAts Error", MB_ICONERROR);
		return;
	}
	SetFilePointer(hFile, exesize, NULL, FILE_BEGIN);
	luaCode = new char[filesize - exesize + 1];
	ReadFile(hFile, luaCode, filesize - exesize, &read, NULL);
	CloseHandle(hFile);

	// I knew I should've used bytecode, but then
	// 1. it'll be more difficult to compile on Mono, because you'll need to ship luac with different arch
	// 2. I couldn't get it to work, don't know exactly why
	// So there is an very easy xor obfuscation, to prevent retarded people from stealing code
	char confusion[] = { 0x11, 0x45, 0x14, 0x19, 0x19, 0x81, 0x14, 0x25 };
	for (int i = 0; i < filesize - exesize; i++) luaCode[i] ^= confusion[i%8];

	L = luaL_newstate();
	luaL_openlibs(L);

	l_setglobalF("__atsfnc_panel", l_panel_getset);
	l_setglobalF("__atsfnc_sound", l_sound_getset);
	l_setglobalF("__atsfnc_msgbox", l_msgbox);

	*(strrchr(dllPath, '\\') + 1) = 0;
	l_setglobalS("__atsval_dlldir", dllPath);
	//sprintf_s(programPath, "%s\\program.lua", dllPath);

	int error = luaL_loadbuffer(L, luaCode, filesize - exesize, "bats") || lua_pcall(L, 0, LUA_MULTRET, 0);

	if (error) {
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

bool firstElapse = true;
bool schdSetSignal = false, schdSetBeacon = false, schdDoorChange = false;
int sdatSignal; bool sdatDoor; ATS_BEACONDATA sdatBeacon;

ATS_API ATS_HANDLES WINAPI Elapse(ATS_VEHICLESTATE vehicleState, int *panel, int *sound) {
	ATS_HANDLES result = { phBrake, phPower, phReverser, 2 };
	if (L == NULL) return result;
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
	if (firstElapse) {
		// Let the internal LastSound inside BVE to be -10000
		// So that new sounds can start from the first plugin Elapse
		for (int i = 0; i < 256; i++) bveSound[i] = -10000;
		firstElapse = false;
		return result;
	}
	if (schdDoorChange) {
		schdDoorChange = false;
		lua_getglobal(L, "__atsapi_doorchange");
		lua_pushboolean(L, sdatDoor);
		if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
		if (L == NULL) return result;
	}
	if (schdSetSignal) {
		schdSetSignal = false;
		lua_getglobal(L, "__atsapi_setsignal");
		lua_pushinteger(L, sdatSignal);
		if (lua_pcall(L, 1, 0, 0) != 0) l_printerr();
		if (L == NULL) return result;
	}
	if (schdSetBeacon) {
		schdSetBeacon = false;
		lua_getglobal(L, "__atsapi_setbeacondata");
		lua_pushnumber(L, sdatBeacon.Distance);
		lua_pushinteger(L, sdatBeacon.Optional);
		lua_pushinteger(L, sdatBeacon.Signal);
		lua_pushinteger(L, sdatBeacon.Type);
		if (lua_pcall(L, 4, 0, 0) != 0) l_printerr();
		if (L == NULL) return result;
	}
	lua_getglobal(L, "__atsapi_elapse");
	lua_pushinteger(L, phPower);
	lua_pushinteger(L, phBrake);
	lua_pushinteger(L, phReverser);
	lua_pushinteger(L, 2);
	if (lua_pcall(L, 4, 4, 0) != 0) {
		l_printerr();
		// Apply emergency brake
		// Lt's hope it'll get the plugin out of the error state by chance
		return { vSpec.BrakeNotches + 1, 0, 1, 2 };
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