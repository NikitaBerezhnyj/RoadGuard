export default {
  "client/**/*.{js,jsx,ts,tsx}": [
    "prettier --write",
    "eslint --fix --config client/eslint.config.js",
  ],
  "server/**/*.cs": [
    // форматування тільки stage-нутих .cs
    "dotnet format server/RoadGuard.Backend.csproj --include",
    // повний build без підстановки stage-нутих файлів
    () => "dotnet build server/RoadGuard.Backend.csproj /warnaserror",
  ],
};
