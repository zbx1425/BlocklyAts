var ieVersion = (function (){
	if (window.ActiveXObject === undefined) return 114514; //Not IE
	if (!window.XMLHttpRequest) return 6;
	if (!document.querySelector) return 7;
	if (!document.addEventListener) return 8;
	if (!window.atob) return 9;
	if (!document.__proto__) return 10;
	return 11;
})();

if (ieVersion < 11) {
	window.onerror = function(message, url, line) {
		// It'll definitely fail, so ignore them
		return true;
	};
}

// User Agent Identifier
// Copyright (C) 2006-2020 Magicant (v1.18 2020-08-01)

function UAIdentifier() {
	if (typeof(navigator) != "object" || !navigator.userAgent) {
		this.unknown = true;
		return;
	}
	
	var ua = navigator.userAgent;
	var match;
	
	if (typeof(RegExp) == "undefined") {
		if (ua.indexOf("Opera") >= 0) {
			this.opera = true;
		} else if (ua.indexOf("Netscape") >= 0) {
			this.netscape = true;
		} else if (ua.indexOf("Mozilla/") == 0) {
			this.mozilla = true;
		} else {
			this.unknown = true;
		}
		
		if (ua.indexOf("Gecko/") >= 0) {
			this.gecko = true;
		} else if (ua.indexOf("Presto/") >= 0) {
			this.presto = true;
		}
		
		if (ua.indexOf("Win") >= 0) {
			this.windows = true;
		} else if (ua.indexOf("Mac") >= 0) {
			this.mac = true;
		} else if (ua.indexOf("Linux") >= 0) {
			this.linux = true;
		} else if (ua.indexOf("BSD") >= 0) {
			this.bsd = true;
		} else if (ua.indexOf("SunOS") >= 0) {
			this.sunos = true;
		}
		return;
	}

	/* for Trident/Tasman */
	/*@cc_on
	@if (@_jscript)
		function jscriptVersion() {
			switch (@_jscript_version) {
				case 3.0:  return "4.0";
				case 5.0:  return "5.0";
				case 5.1:  return "5.01";
				case 5.5:  return "5.5";
				case 5.6:
					if ("XMLHttpRequest" in window) return "7.0";
					return "6.0";
				case 5.7:  return "7.0";
				case 5.8:  return "8.0";
				case 9.0:  return "9.0";
				case 10.0: return "10.0";
				case 11.0: return "11.0";
				default:   return true;
			}
		}
		if (@_win16 || @_win32 || @_win64) {
			this.windows = true;
			this.trident = jscriptVersion();
		} else if (@_mac || navigator.platform.indexOf("Mac") >= 0) {
			// '@_mac' may be 'NaN' even if the platform is Mac,
			// so we check 'navigator.platform', too.
			this.mac = true;
			this.tasman = jscriptVersion();
		}
		if (match = ua.match("MSIE ?(\\d+\\.\\d+)b?;")) {
			this.ie = match[1];
		}
	@else @*/
	if (match = ua.match("Trident/(\\d+(\\.\\d+)*)")) {
		this.trident = match[1];
		if (match = ua.match("\\([^(]*rv:(\\d+(\\.\\d+)*).*?\\)")) {
			this.ie = match[1];
		} else {
			this.ie = true;
		}
	}
	
	/* for old Edge */
	else if (match = ua.match("Edge/(\\d+(\\.\\d+)*)")) {
		this.edge = true;
		this.edgehtml = match[1];
	}
	
	/* for AppleWebKit */
	else if (match = ua.match("AppleWebKit/(\\d+(\\.\\d+)*)")) {
		this.applewebkit = match[1];
	}
	
	/* for Gecko */
	else if (typeof(Components) == "object") {
		if (match = ua.match("Gecko/(\\d{8})")) {
			this.gecko = match[1];
		} else if (navigator.product == "Gecko"
				&& (match = navigator.productSub.match("^(\\d{8})$"))) {
			this.gecko = match[1];
		}
	}
	
	/* for Presto */
	else if (typeof(opera) == "object"
			&& (match = ua.match("Presto/(\\d+(\\.\\d+)*)"))) {
		this.presto = match[1];
	}
	
	/*@end @*/
	
	if (typeof(opera) == "object" && typeof(opera.version) == "function") {
		this.opera = opera.version();
	} else if (typeof(opera) == "object"
			&& (match = ua.match("Opera[/ ](\\d+\\.\\d+)"))) {
		this.opera = match[1];
	} else if (match = ua.match("OPR/(\\d+(\\.\\d+)*)")) {
		this.opera = match[1];
	} else if (this.ie || this.edge) {
	} else if (match = ua.match("Edg/(\\d+(\\.\\d+)*)")) {
		this.edge = match[1];
	} else if (match = ua.match("Vivaldi/(\\d+(\\.\\d+)*)")) {
		this.vivaldi = match[1];
	} else if (match = ua.match("Epiphany/(\\d+(\\.\\d+)*)")) {
		this.epiphany = match[1];
	} else if (match = ua.match("Chrome/(\\d+(\\.\\d+)*)")) {
		this.chrome = match[1];
	} else if (match = ua.match("Safari/(\\d+(\\.\\d+)*)")) {
		this.safari = match[1];
	} else if (match = ua.match("Konqueror/(\\d+(\\.\\d+)*)")) {
		this.konqueror = match[1];
	} else if (ua.indexOf("(compatible;") < 0
			&& (match = ua.match("^Mozilla/(\\d+\\.\\d+)"))) {
		this.mozilla = match[1];
		if (match = ua.match("\\([^(]*rv:(\\d+(\\.\\d+)*).*?\\)"))
			this.mozillarv = match[1];
		if (match = ua.match("Firefox/(\\d+(\\.\\d+)*)")) {
			this.firefox = match[1];
		} else if (match = ua.match("Netscape\\d?/(\\d+(\\.\\d+)*)")) {
			this.netscape = match[1];
		}
	} else {
		this.unknown = true;
	}
	
	if (ua.indexOf("Win 9x 4.90") >= 0) {
		this.windows = "ME";
	} else if (match = ua.match("Win(dows)? ?(NT ?(\\d+\\.\\d+)?|\\d+|XP|ME|Vista)")) {
		this.windows = match[2];
		if (match[3]) {
			this.winnt = match[3];
		} else switch (match[2]) {
			case "2000":   this.winnt = "5.0";  break;
			case "XP":     this.winnt = "5.1";  break;
			case "Vista":  this.winnt = "6.0";  break;
		}
	} else if (ua.indexOf("Mac") >= 0) {
		this.mac = true;
	} else if (ua.indexOf("Linux") >= 0) {
		this.linux = true;
	} else if (match = ua.match("\\w*BSD")) {
		this.bsd = match[0];
	} else if (ua.indexOf("SunOS") >= 0) {
		this.sunos = true;
	}

	if (this.safari && !this.windows && !this.mac) {
		delete this.safari;
		this.unknown = true;
	}
}

UAIdentifier.prototype.toString = function() {
	var r = "";
	
	if (this.opera) {
		r += "Opera";
		if (this.opera !== true)
			r += ":" + this.opera;
	} else if (this.ie) {
		r += "IE";
		if (this.ie !== this)
			r += ":" + this.ie;
	} else if (this.edge) {
		r += "Edge:" + this.edge;
	} else if (this.vivaldi) {
		r += "Vivaldi:" + this.vivaldi;
	} else if (this.chrome) {
		r += "Chrome:" + this.chrome;
	} else if (this.safari) {
		r += "Safari:" + this.safari;
	} else if (this.konqueror) {
		r += "Konqueror:" + this.konqueror;
	} else if (this.mozilla) {
		r += "Mozilla";
		if (this.mozilla !== true) {
			r += ":" + this.mozilla;
			if (this.mozillarv)
				r += ":" + this.mozillarv;
		}
		if (this.firefox)
			r += ",Firefox:" + this.firefox;
		else if (this.netscape)
			r += ",Netscape:" + this.netscape;
	} else {
		r += "Unknown";
	}
	
	return r;
};

var uaString = new UAIdentifier().toString();
var isIE = new UAIdentifier().ie;