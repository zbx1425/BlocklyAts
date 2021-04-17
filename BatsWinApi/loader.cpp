#include "pch.h"
#include "globals.h"
#include "winfile.h"
#include "luainterop.h"

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
	for (DWORD i = 0; i < filesize - exesize; i++) luaCode[i] ^= confusion[i % 8];

	L = luaL_newstate();
	luaL_openlibs(L);
	luaL_requiref(L, "winfile", luaopen_winfile, 1);
	open_interop_functions();

	*(wcsrchr(dllPath, '\\') + 1) = 0;
	const char * sBuf = lua_tostring(L, -1);
	DWORD dBufSize = WideCharToMultiByte(CP_UTF8, 0, dllPath, -1, NULL, 0, NULL, FALSE);
	char* dllPathMB = new char[dBufSize];
	WideCharToMultiByte(CP_UTF8, 0, dllPath, -1, dllPathMB, dBufSize, NULL, FALSE);
	l_setglobalS("_vdlldir", dllPathMB);

	int error = luaL_loadbuffer(L, luaCode, filesize - exesize, "bats") || lua_pcall(L, 0, LUA_MULTRET, 0);

	if (error) {
		l_printerr();
	}
	else {
		lua_getglobal(L, "_eload");
		if (lua_pcall(L, 0, 0, 0) != 0) l_printerr();
	}
}

ATS_API int WINAPI GetPluginVersion() {
	return ATS_VERSION;
}

ATS_API void WINAPI Dispose() {
	if (L == NULL) return;
	lua_getglobal(L, "_edispose");
	if (lua_pcall(L, 0, 0, 0) != 0) l_printerr();
	lua_close(L);
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
