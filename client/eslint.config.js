const { defineConfig } = require("eslint/config");
const expoConfig = require("eslint-config-expo/flat");

module.exports = defineConfig([
  expoConfig,
  {
    ignores: ["dist/*"],
    plugins: {
      sonarjs: require("eslint-plugin-sonarjs")
    },
    settings: {
      "import/resolver": {
        typescript: {
          project: "./client/tsconfig.json",
          alwaysTryTypes: true
        }
      }
    },
    rules: {
      "import/no-unresolved": "error",
      ...require("eslint-plugin-sonarjs").configs.recommended.rules,
      "no-unused-vars": ["warn", { vars: "all", args: "after-used", ignoreRestSiblings: true }],
      "no-console": "off",
      "sonarjs/no-commented-code": "off",
      "sonarjs/pseudo-random": "off",
      "sonarjs/cognitive-complexity": ["warn", 15]
    }
  }
]);
