// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "pch.h"

/*FARPROC WINAPI delayHook(unsigned dliNotify, PDelayLoadInfo pdli) {
	if (dliNotify == dliFailLoadLib && strcmp("lua54.dll", pdli->szDll) == 0 && pdli->dwLastError == ERROR_MOD_NOT_FOUND) {
#ifdef _WIN64
		HMODULE lib = LoadLibraryA(".\\lua54_x64.dll");
#else
		HMODULE lib = LoadLibraryA("C:\\Users\\zbx1425\\AppData\\Roaming\\openBVE\\LegacyContent\\Train\\Series-103\\batstest\\lua54_x86.dll");
#endif
		int lastErr = GetLastError();
		return (FARPROC)lib;
	}
	return NULL;
}

ExternC PfnDliHook __pfnDliFailureHook2 = delayHook;

/*

FARPROC WINAPI delayHook(unsigned dliNotify, PDelayLoadInfo pdli) {
	if (dliNotify == dliFailLoadLib && strcmp("lua54.dll", pdli->szDll) == 0 && pdli->dwLastError == ERROR_MOD_NOT_FOUND) {
#if _WIN64
		HMODULE lib = LoadLibraryA("lua54_x64.dll");
#else
		HMODULE lib = LoadLibraryA("lua54_x86.dll");
#endif
		return (FARPROC)lib;
	}

	return NULL;
}*/

BOOL APIENTRY DllMain(HMODULE hModule,  DWORD  ul_reason_for_call, LPVOID lpReserved) {
    switch (ul_reason_for_call) {
		break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}