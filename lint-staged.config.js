export default {
  "client/**/*.{js,jsx,ts,tsx}": [
    "prettier --write",
    "eslint --fix --config client/eslint.config.js",
  ],
  "server/**/*.cs": [
    "dotnet format server/RoadGuard.Backend.csproj --include",
    () => "dotnet build server/RoadGuard.Backend.csproj /warnaserror",
  ],
};
