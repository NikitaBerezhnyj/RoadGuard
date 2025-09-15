# RoadGuard

RoadGuard is a mobile application for drivers that allows users to see other drivers on the road, their behavior ratings, and receive real-time warnings about dangerous drivers. The goal is to improve road safety and promote social interaction among drivers.

## Features

- **Live Driver Map**: See yourself and other users on the map with color-coded icons representing driver ratings:
  - 🟢 Calm / Safe
  - 🟡 Neutral / Average
  - 🔴 Aggressive / Dangerous
- **Driver Ratings**: Like/dislike other drivers and calculate average ratings. Reputation system prevents abuse.
- **Anonymous Reports**: Report unsafe drivers, including non-registered users, showing temporary danger zones on the map.
- **Vehicle Info**: Optional display of car make and color in user profiles; supports anonymous mode.
- **Privacy & Security**: No license plates, VINs, or personal data stored. Real-time location only; history is not saved.
- **Gamification**: User levels and achievements based on ratings and reputation.

## Technology Stack

### Mobile App

- **React Native + Expo** – cross-platform for iOS and Android
- **Maps**: react-native-maps or Mapbox
- **UI Library**: NativeBase / React Native Paper

### Backend

- **C# + .NET Core + SignalR** – real-time updates for driver positions, ratings, and temporary reports
- **REST API** – user profiles, ratings, locations, and reports
- **SignalR Hub** – live updates to clients

### Database

- **PostgreSQL + PostGIS** – stores users, locations, ratings, and reports
- **Redis** – caches live data for faster updates

### Additional

- JWT authentication
- Push notifications / voice alerts (future expansion)

## Getting Started

### Backend

```bash
docker compose up --build
```

### Client

Run on an Android emulator or device with Expo:

```bash
npm run android
```

or:

```bash
npm start
```
