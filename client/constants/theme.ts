import { Platform } from "react-native";

const tintColorLight = "#0B79A3";
const tintColorDark = "#00D1FF";

export const Colors = {
  light: {
    text: "#11181C",
    background: "#F7F9FA",
    tint: tintColorLight,
    icon: "#6B7C85",
    tabIconDefault: "#6B7C85",
    tabIconSelected: tintColorLight,

    driverGood: "#28C76F",
    driverNeutral: "#FBC02D",
    driverDanger: "#F44336",
    dangerZoneStroke: "#F44336",
    dangerZoneFill: "rgba(244, 67, 54, 0.2)"
  },
  dark: {
    text: "#ECEDEE",
    background: "#151718",
    tint: tintColorDark,
    icon: "#9BA1A6",
    tabIconDefault: "#9BA1A6",
    tabIconSelected: tintColorDark,

    driverGood: "#00E676",
    driverNeutral: "#FFD600",
    driverDanger: "#FF3D00",
    dangerZoneStroke: "#FF3D00",
    dangerZoneFill: "rgba(255, 61, 0, 0.2)"
  }
};

export const Fonts = Platform.select({
  ios: {
    sans: "system-ui",
    serif: "ui-serif",
    rounded: "ui-rounded",
    mono: "ui-monospace"
  },
  default: {
    sans: "normal",
    serif: "serif",
    rounded: "normal",
    mono: "monospace"
  },
  web: {
    sans: "system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif",
    serif: "Georgia, 'Times New Roman', serif",
    rounded: "'SF Pro Rounded', 'Hiragino Maru Gothic ProN', Meiryo, 'MS PGothic', sans-serif",
    mono: "SFMono-Regular, Menlo, Monaco, Consolas, 'Liberation Mono', 'Courier New', monospace"
  }
});
