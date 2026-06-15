# ClassTracker

ClassTracker is an Android tablet application for tracking children’s activities, levels, and notes.  
It is designed for classroom or daycare-style environments (e.g. kindergarten), but can be adapted for other activity tracking scenarios.

The app focuses on simple, offline usage with all data stored locally in JSON format.

---

## ✨ Features

- Create and manage child profiles
- Log activities per child
- Track levels or progress per activity
- Add notes for each activity
- Tablet-friendly UI
- Offline (no internet connection required)

---

## 🛠 Tech Stack

- Unity 6.3 LTS
- C#
- Local JSON file storage

---

## 📱 Target Platform

- Android tablets  
(Not yet tested on a wide range of devices.)

---

## 🚀 Getting Started

### Requirements

- Unity Hub
- Unity **6.3 LTS**
- Android Build Support (installed via Unity Hub)
- Android SDK & NDK (via Unity Hub)

### Setup

1. Clone this repository
2. Open the project in Unity Hub
3. Load the main scene (if not loaded automatically)

---

## 🏗 Building for Android

1. Open **File → Build Settings**
2. Select **Android** and click **Switch Platform**
3. Set your package name in **Player Settings**
4. Click **Build** and generate an APK

---

## 💾 Data Storage

All data is stored locally on the device using JSON files.  
No cloud services or external servers are used.

Data is saved in Unity’s persistent data path.

---

## 📄 License

This project is licensed under the MIT License – see the LICENSE file for details.
