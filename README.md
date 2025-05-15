Potion Collector Game

"Potion Collector" is a casual mobile game I developed using Unity. In this game, players collect potions to accumulate scores. I integrated Firebase services for user authentication, session tracking, and leaderboard management. The architecture is event-driven to maintain a clean and decoupled game flow.

#Game Overview and Interaction Flow

Here’s how the game functions:

- On launching the game, the player is authenticated using either Google Sign-In through Firebase.
- When the game starts:
  - Session start time is saved to firestore.
  - Potions begin spawning, and the player can collect them to earn points.
  - The score, session start time, and session end time are saved to Firestore under the player's session history.
- Players can view the top scores globally through the leaderboard UI.


#Firebase Setup Guide

To set up Firebase in this project, I followed these steps:

Firebase Services Used:
- Authentication: Google Sign-In.
- Cloud Firestore: To manage player profiles, session history, and leaderboard scores.

#Configuration Steps I Followed:

1. I created a new project in the [Firebase Console](https://console.firebase.google.com/).
2. Under Authentication > Sign-in Method, I enabled:
   - Google Sign-In
3. I enabled Cloud Firestore, starting in test mode for development purposes.
4. In Unity:
   - I imported the Firebase Unity SDK.
   - Placed the `google-services.json` file inside the `Assets/StreamingAssets` folder.
5. I implemented data saving for:
   - Player profile data in a `players` collection.
   - Game session data in a `players/{userId}/sessions` subcollection.
   - Leaderboard entries in a centralized `leaderboard` collection.

- This allowed me to keep the game lightweight and modular.

#Event System Description

I created a centralized event system in `GameEvents.cs` to decouple logic between gameplay, UI, and Firebase operations. This makes the system scalable and maintainable.

#Here are some Events I Implemented:
- OnGameStarted(float timestamp, string sessionId)
- OnGameEnded(float timestamp, int totalScore)
- OnScoreUpdated(int newScore, int scoreDelta)
- OnLeaderboardLoaded(PlayerData[] topScores)
- OnFirebaseSyncStarted(string operationType)
- OnFirebaseSyncCompleted(string operationType, bool success)
