using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocklyAts.WebView {
    public static class JsStringEscaper {
        /// <summary>
        /// 将字符串转义为JavaScript字符串字面量（包含引号）
        /// </summary>
        /// <param name="input">要转义的字符串</param>
        /// <returns>转义后的JavaScript字符串字面量</returns>
        public static string EscapeJsString(string input) {
            if (input == null)
                return "null";

            var sb = new StringBuilder(input.Length + 2);
            sb.Append('"');

            foreach (char c in input) {
                switch (c) {
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\v':
                        sb.Append("\\v");
                        break;
                    case '\0':
                        sb.Append("\\0");
                        break;
                    default:
                        if (c < ' ' || c > '~') {
                            // 对于控制字符和非ASCII字符，使用Unicode转义
                            sb.Append("\\u");
                            sb.Append(((int)c).ToString("X4"));
                        } else {
                            sb.Append(c);
                        }
                        break;
                }
            }

            sb.Append('"');
            return sb.ToString();
        }

        /// <summary>
        /// 将字符串转义为JavaScript字符串内容（不包含引号）
        /// </summary>
        /// <param name="input">要转义的字符串</param>
        /// <returns>转义后的JavaScript字符串内容</returns>
        public static string EscapeJsStringContent(string input) {
            if (input == null)
                return string.Empty;

            var sb = new StringBuilder(input.Length);

            foreach (char c in input) {
                switch (c) {
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\v':
                        sb.Append("\\v");
                        break;
                    case '\0':
                        sb.Append("\\0");
                        break;
                    default:
                        if (c < ' ' || c > '~') {
                            // 对于控制字符和非ASCII字符，使用Unicode转义
                            sb.Append("\\u");
                            sb.Append(((int)c).ToString("X4"));
                        } else {
                            sb.Append(c);
                        }
                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将JavaScript转义字符串反转义为原始字符串
        /// </summary>
        /// <param name="input">JavaScript转义字符串</param>
        /// <returns>反转义后的原始字符串</returns>
        public static string UnescapeJsString(string input) {
            if (string.IsNullOrEmpty(input))
                return input;

            // 如果是带引号的字符串字面量，去掉首尾引号
            if (input.Length >= 2 && input[0] == '"' && input[input.Length - 1] == '"') {
                input = input.Substring(1, input.Length - 2);
            }

            var sb = new StringBuilder(input.Length);

            for (int i = 0; i < input.Length; i++) {
                if (input[i] == '\\' && i + 1 < input.Length) {
                    char nextChar = input[i + 1];
                    switch (nextChar) {
                        case '"':
                            sb.Append('"');
                            i++;
                            break;
                        case '\\':
                            sb.Append('\\');
                            i++;
                            break;
                        case 'b':
                            sb.Append('\b');
                            i++;
                            break;
                        case 'f':
                            sb.Append('\f');
                            i++;
                            break;
                        case 'n':
                            sb.Append('\n');
                            i++;
                            break;
                        case 'r':
                            sb.Append('\r');
                            i++;
                            break;
                        case 't':
                            sb.Append('\t');
                            i++;
                            break;
                        case 'v':
                            sb.Append('\v');
                            i++;
                            break;
                        case '0':
                            sb.Append('\0');
                            i++;
                            break;
                        case 'u':
                            // Unicode转义序列：\uXXXX
                            if (i + 5 < input.Length) {
                                string hex = input.Substring(i + 2, 4);
                                if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int unicode)) {
                                    sb.Append((char)unicode);
                                    i += 5;
                                } else {
                                    // 无效的Unicode转义，按原样保留
                                    sb.Append(input[i]);
                                }
                            } else {
                                // 不完整的Unicode转义，按原样保留
                                sb.Append(input[i]);
                            }
                            break;
                        default:
                            // 其他转义字符，按原样保留反斜杠
                            sb.Append(input[i]);
                            break;
                    }
                } else {
                    sb.Append(input[i]);
                }
            }

            return sb.ToString();
        }
    }
}
