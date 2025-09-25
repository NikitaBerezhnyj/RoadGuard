import React, { useEffect, useState, useRef } from "react";
import {
  StyleSheet,
  View,
  ActivityIndicator,
  useColorScheme,
  Dimensions,
  Pressable
} from "react-native";
import MapView, { Marker, Circle } from "react-native-maps";
import * as Location from "expo-location";
import { Ionicons } from "@expo/vector-icons";
import { useRouter } from "expo-router";
import { Colors, Sizes, Spaces } from "@/constants/theme";

const { width, height } = Dimensions.get("window");

export default function HomeScreen() {
  const router = useRouter();
  const colorScheme = useColorScheme();
  const theme = colorScheme === "dark" ? Colors.dark : Colors.light;
  const [location, setLocation] = useState<{ latitude: number; longitude: number } | null>(null);
  const mapRef = useRef<MapView>(null);

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

  const centerOnUser = () => {
    if (location && mapRef.current) {
      mapRef.current.animateToRegion(
        {
          latitude: location.latitude,
          longitude: location.longitude,
          latitudeDelta: 0.01,
          longitudeDelta: 0.01
        },
        500
      );
    }
  };

  return (
    <View style={{ flex: 1 }}>
      <MapView
        ref={mapRef}
        style={styles.map}
        showsUserLocation
        initialRegion={{
          latitude: location.latitude,
          longitude: location.longitude,
          latitudeDelta: 0.01,
          longitudeDelta: 0.01
        }}
        customMapStyle={colorScheme === "dark" ? darkMapStyle : undefined}
        showsMyLocationButton={false}
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

      <View style={styles.topRightContainer}>
        <Pressable
          style={[styles.fabButton, { backgroundColor: theme.tint }]}
          onPress={centerOnUser}
        >
          <Ionicons name="locate-outline" size={28} color="#fff" />
        </Pressable>
      </View>

      <View style={styles.fabContainer}>
        <Pressable
          style={[styles.fabButton, { backgroundColor: theme.tint }]}
          onPress={() => console.log("Alert")}
        >
          <Ionicons name="alert" size={28} color="#fff" />
        </Pressable>

        <Pressable
          style={[styles.fabButton, { backgroundColor: theme.tint }]}
          onPress={() => router.push("/(tabs)/settings")}
        >
          <Ionicons name="settings-outline" size={28} color="#fff" />
        </Pressable>

        <Pressable
          style={[styles.fabButton, { backgroundColor: theme.tint }]}
          onPress={() => router.push("/(tabs)/profile-card")}
        >
          <Ionicons name="person-outline" size={28} color="#fff" />
        </Pressable>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  map: {
    flex: 1,
    width: width,
    height: height,
    margin: 0,
    padding: 0
  },
  loader: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    width: width,
    height: height,
    margin: 0,
    padding: 0
  },
  userCircle: {
    width: 20,
    height: 20,
    borderRadius: 10,
    borderWidth: 2,
    borderColor: "#fff"
  },
  fabContainer: {
    position: "absolute",
    right: Spaces.medium,
    bottom: Spaces.large,
    gap: Spaces.medium,
    alignItems: "center"
  },
  topRightContainer: {
    position: "absolute",
    top: Spaces.large,
    right: Spaces.medium
  },
  fabButton: {
    width: Sizes.xlarge,
    height: Sizes.xlarge,
    borderRadius: Sizes.xlarge / 2,
    justifyContent: "center",
    alignItems: "center",
    shadowColor: "#000",
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.3,
    shadowRadius: 3,
    elevation: 5
  }
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
