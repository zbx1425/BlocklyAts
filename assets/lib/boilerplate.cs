using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBveApi.Runtime;

public class LU : List<U> {
    public override string ToString() {
        return "{" + string.Join(",", this.Select(t => t.ToString())) + "}";
    }
}

public class U {
    private object e;
    public U(object d) { e = d; }

    public dynamic Cast(Type type) {
        if (e.GetType() == type) return e;
        if (type == typeof(bool)) {
            if (e is string) {
                return (e as string).ToLowerInvariant() == "true" || (e as string) == "1";
            } else if (e is int || e is double) {
                return (int)e != 0;
            } else {
                throw new InvalidCastException();
            }
        } else if (type == typeof(int)) {
            if (e is string) {
                int result;
                if (!int.TryParse(e as string, out result)) throw new InvalidCastException();
                return result;
            } else if (e is int || e is double) {
                return (int)e;
            } else {
                throw new InvalidCastException();
            }
        } else if (type == typeof(double)) {
            if (e is string) {
                double result;
                if (!double.TryParse(e as string, out result)) throw new InvalidCastException();
                return result;
            } else if (e is int || e is double) {
                return (double)e;
            } else {
                throw new InvalidCastException();
            }
        } else if (type == typeof(string)) {
            return e.ToString();
        } else if (type == typeof(LU)) {
            throw new InvalidCastException();
        } else {
            throw new InvalidCastException();
        }
    }

    public static implicit operator U(bool d) => new U(d);
    public static implicit operator U(int d) => new U(d);
    public static implicit operator U(double d) => new U(d);
    public static implicit operator U(string d) => new U(d);
    public static implicit operator U(LU d) => new U(d);

    public static implicit operator bool(U d) => d.Cast(typeof(bool));
    public static implicit operator int(U d) => d.Cast(typeof(int));
    public static implicit operator double(U d) => d.Cast(typeof(double));
    public static implicit operator string(U d) => d.Cast(typeof(string));
    public static implicit operator LU(U d) => d.Cast(typeof(LU));
}