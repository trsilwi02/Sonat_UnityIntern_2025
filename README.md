# Sonat_UnityIntern_2025

Tran Duy Anh – Unity Intern Test

# 🧪 Water Sort Puzzle – Unity Prototype

A demo of a classic color-sorting puzzle game (Water Sort), built in Unity with a strong focus on data structures, algorithmic thinking, and user experience optimization.

# 📖 Overview

This project recreates the familiar gameplay of the Water Sort Puzzle genre. The player’s task is to sort mixed color layers inside test tubes so that each tube contains only one color.

This project was developed as part of a Technical Test, focusing on:

+ Stack logic to manage color layers

+ Deadlock detection algorithms

+ Flexible level system using prefabs

+ Optimized input for both PC and Mobile

# 🎮 Gameplay Mechanics
Basic Rules

+ Select: Tap on a tube to select it (the tube will lift up)

+ Pour: Tap on another tube to pour water into it

Pour Conditions:

+ The target tube must have available space (Capacity > Current)

+ The target tube must be empty, OR its top color must match the color being poured

Win Condition:

+ You win when all tubes are either:

+ Completely filled with only one color, or Empty

# 🌟 Features

🖱️ Multi-platform Input – Smooth interaction on both Mouse (PC) and Touch (Mobile)
🌊 Fluid Animation – Smooth pouring animations using DOTween
🎨 Dynamic Colors – Colors managed through ScriptableObjects for easy theming
🧩 Level System – Automatically loads levels from prefabs with Replay and Next Level support
🔊 Audio System – Interactive sound effects (Click, Pour, Win, Lose)
🤖 Smart Deadlock Detection – Automatically checks when no more valid moves exist

🛠️ Tech Stack & Architecture

+ Engine: Unity 2022.3 (or Unity 6)

+ Language: C#

+ Library: DOTween (Animation)

+ Design Patterns: Singleton (Managers), MVC (separated Tube Logic and Visuals)

📁 Project Structure
Scripts:

+ GameManager.cs

+ Tube.cs

+ AudioManager.cs

+ UIManager.cs

+ ColorPalette (ScriptableObject)

# 🚀 Installation
Clone the Repository
git clone https://github.com/trsilwi02/Sonat_UnityIntern_2025.git

Open with Unity Hub

Add the project in Unity Hub and open it using Unity 2022.3 or newer.

Install DOTween

If the project reports missing DOTween:

Open Window > Package Manager or reinstall DOTween from the Asset Store

Go to Tools > Demigiant > DOTween Utility Panel

Click Setup DOTween

Play the Game

Open SampleScene (or GameScene) and press Play.

# ▶️ Playable Build (Download)

Drive: https://drive.google.com/drive/folders/1YxfpRbQl5XljpYVeVWKRCRKC30HxTTgi?usp=sharing
