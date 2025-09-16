import React, { useEffect, useState } from "react";
import { StyleSheet, View, ActivityIndicator, useColorScheme } from "react-native";
import MapView, { Marker, Circle } from "react-native-maps";
import * as Location from "expo-location";
import { Colors } from "@/constants/theme";

export default function HomeScreen() {
  const colorScheme = useColorScheme();
  const theme = colorScheme === "dark" ? Colors.dark : Colors.light;

  const [location, setLocation] = useState<{ latitude: number; longitude: number } | null>(null);

  const otherUsers = [
    { id: 1, name: "Андрій", lat: 50.451, lng: 30.523, rating: "great" },
    { id: 2, name: "Марія", lat: 50.449, lng: 30.525, rating: "normal" },
    { id: 3, name: "Олег", lat: 50.452, lng: 30.521, rating: "bad" }
  ];

  const zones = [
    { id: "zone1", lat: 50.4505, lng: 30.5235, radius: 300 },
    { id: "zone2", lat: 50.4515, lng: 30.522, radius: 200 }
  ];

  useEffect(() => {
    (async () => {
      const { status } = await Location.requestForegroundPermissionsAsync();
      if (status !== "granted") {
        console.log("Permission to access location was denied");
        return;
      }
      const loc = await Location.getCurrentPositionAsync({});
      setLocation(loc.coords);
    })();
  }, []);

  // console.log

  if (!location) {
    return (
      <View style={[styles.loader, { backgroundColor: theme.background }]}>
        <ActivityIndicator size="large" color={theme.tint} />
      </View>
    );
  }

  const getColorByRating = (rating: string) => {
    switch (rating) {
      case "great":
        return theme.driverGood;
      case "normal":
        return theme.driverNeutral;
      case "bad":
        return theme.driverDanger;
      default:
        return theme.icon;
    }
  };

  return (
    <MapView
      style={styles.map}
      showsUserLocation
      initialRegion={{
        latitude: location.latitude,
        longitude: location.longitude,
        latitudeDelta: 0.01,
        longitudeDelta: 0.01
      }}
      customMapStyle={colorScheme === "dark" ? darkMapStyle : undefined}
    >
      {otherUsers.map(user => (
        <Marker
          key={user.id}
          coordinate={{ latitude: user.lat, longitude: user.lng }}
          onPress={() => console.log(`Press on ${user.name}`)}
        >
          <View style={[styles.userCircle, { backgroundColor: getColorByRating(user.rating) }]} />
        </Marker>
      ))}

      {zones.map(zone => (
        <Circle
          key={zone.id}
          center={{ latitude: zone.lat, longitude: zone.lng }}
          radius={zone.radius}
          strokeColor={theme.dangerZoneStroke}
          fillColor={theme.dangerZoneFill}
        />
      ))}
    </MapView>
  );
}

const styles = StyleSheet.create({
  map: { flex: 1 },
  loader: { flex: 1, justifyContent: "center", alignItems: "center" },
  userCircle: { width: 20, height: 20, borderRadius: 10, borderWidth: 2, borderColor: "#fff" }
});

const darkMapStyle = [
  {
    elementType: "geometry",
    stylers: [{ color: "#151718" }]
  },
  {
    elementType: "labels.text.fill",
    stylers: [{ color: "#ECEDEE" }]
  },
  {
    elementType: "labels.text.stroke",
    stylers: [{ color: "#151718" }]
  },
  {
    featureType: "road",
    elementType: "geometry",
    stylers: [{ color: "#1F2426" }]
  },
  {
    featureType: "water",
    elementType: "geometry",
    stylers: [{ color: "#0A7EA4" }]
  }
];
