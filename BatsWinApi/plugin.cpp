#include "pch.h"
#include "globals.h"
#include "luainterop.h"

bool elapseDataReady = false;
struct lafc {
	short type;
	float f1;
	int i1, i2, i3;
	static lafc doorChange(bool b1) { return lafc{ 0, 0.0f, b1, 0, 0 }; }
	static lafc setSignal(int i1) { return lafc{ 1, 0.0f, i1, 0, 0 }; }
	static lafc setBeaconData(float f1, int i1, int i2, int i3) { return lafc{ 2, f1, i1, i2, i3 }; }
	static lafc hornBlow(int i1) { return lafc{ 3, 0.0f, i1, 0, 0 }; }
	static lafc keyDown(int i1) { return lafc{ 4, 0.0f, i1, 0, 0 }; }
	static lafc keyUp(int i1) { return lafc{ 5, 0.0f, i1, 0, 0 }; }
	void invoke();
	void schedule();
};
std::queue<lafc> lateCallQueue;

void lafc::invoke() {
	if (L == NULL) return;
	int paramCount = 1;
	switch (type) {
	case 0:
		lua_getglobal(L, "_edoorchange");
		lua_pushboolean(L, (bool)i1);
		break;
	case 1:
		lua_getglobal(L, "_esetsignal");
		lua_pushinteger(L, i1);
		break;
	case 2:
		lua_getglobal(L, "_esetbeacondata");
		lua_pushnumber(L, f1);
		lua_pushinteger(L, i1);
		lua_pushinteger(L, i2);
		lua_pushinteger(L, i3);
		paramCount = 4;
		break;
	case 3:
		lua_getglobal(L, "_ehornblow");
		lua_pushinteger(L, i1);
		break;
	case 4:
		lua_getglobal(L, "_ekeydown");
		lua_pushinteger(L, i1);
		break;
	case 5:
		lua_getglobal(L, "_ekeyup");
		lua_pushinteger(L, i1);
		break;
	}

	if (lua_pcall(L, paramCount, 0, 0) != 0) l_printerr();
}

void lafc::schedule() {
	if (elapseDataReady) {
		invoke();
	} else {
		lateCallQueue.push(lafc{ type, f1, i1, i2, i3 });
	}
}

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
	if (!elapseDataReady) {
		// Let the internal LastSound inside BVE to be -10000
		// So that new sounds can start from the first plugin Elapse
		for (int i = 0; i < 256; i++) bveSound[i] = -10000;
		elapseDataReady = true;
		return result;
	}
	while (!lateCallQueue.empty()) {
		lateCallQueue.front().invoke();
		lateCallQueue.pop();
		if (L == NULL) return result;
	}
	lua_getglobal(L, "_eelapse");
	lua_pushinteger(L, phPower);
	lua_pushinteger(L, phBrake);
	lua_pushinteger(L, phReverser);
	lua_pushinteger(L, 2);
	if (lua_pcall(L, 4, 4, 0) != 0) {
		l_printerr();
		if (L == NULL) return result;
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
ATS_API void WINAPI KeyDown(int atsKeyCode) {
	lafc::keyDown(atsKeyCode).schedule();
}

ATS_API void WINAPI KeyUp(int atsKeyCode) {
	lafc::keyUp(atsKeyCode).schedule();
}

ATS_API void WINAPI HornBlow(int atsHornBlowIndex) {
	lafc::hornBlow(atsHornBlowIndex).schedule();
}

ATS_API void WINAPI DoorOpen() {
	lafc::doorChange(true).schedule();
}

ATS_API void WINAPI DoorClose() {
	lafc::doorChange(false).schedule();
}

ATS_API void WINAPI SetSignal(int signal) {
	lafc::setSignal(signal).schedule();
}

ATS_API void WINAPI SetBeaconData(ATS_BEACONDATA data) {
	lafc::setBeaconData(data.Distance, data.Optional, data.Signal, data.Type).schedule();
}