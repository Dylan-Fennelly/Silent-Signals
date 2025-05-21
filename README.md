
# 🧊 Silent Signals – Unity HDRP Citizen Science Game

**Silent Signals** is a narrative-driven, first-person exploration prototype developed in Unity HDRP. Set in a remote Arctic research base during a time of global environmental collapse, players must operate degraded scientific machinery, gather climate data, and decide whether to uphold scientific integrity or align with profit-driven corporate agendas.

This project was developed as part of the **BSc in Computing in Games Development** course at DKIT. It explores critical real-world issues including environmental degradation, misinformation, and the ethics of research under pressure.

---

## 🎮 Features

- 🚶‍♂️ First-person movement and environment interaction
- ⚙️ Machine-based minigames for gathering scientific data
- 📝 Dynamic report submission system with funding consequences
- 🧠 AI system using GOAP or Behavior Graph to simulate environmental or systemic changes
- ❄️ Atmospheric HDRP visuals including frost/rust shaders and snowstorm VFX
- 🔋 Resource management for generator power and machine status
- 📓 Dual-stage UI: analog-style notepad transforms into digital tablet based on player decisions
- 🧭 Clean HUD showing tasks, fuel levels, and environmental state

---

## 📁 Project Structure

```plaintext
Assets/
├── Scenes/                   // All Unity scenes
├── Prefabs/                  // Prefabricated GameObjects
│   ├── Characters/
│   ├── Environment/
│   ├── UI/
│   └── Gameplay/
├── Scripts/                  // All game logic and scripts
│   ├── Core/                 // Managers and global systems
│   ├── Gameplay/             // Gameplay mechanics scripts
│   ├── Characters/           // Player/NPC scripts
│   ├── AI/                   // AI-related scripts
│   ├── UI/                   // UI interaction scripts
│   ├── Utilities/            // General-purpose helpers
│   └── Editor/               // Custom editor tools
├── ScriptableObjects/        // Data-driven assets
│   ├── GameplayData/
│   ├── CharacterStats/
│   └── Settings/
├── Art/                      // Visual assets
│   ├── Animations/
│   ├── Materials/
│   ├── Models/
│   │   ├── Characters/
│   │   ├── Environment/
│   │   └── Props/
│   ├── Textures/
│   ├── VFX/
│   └── UI/
│       ├── Sprites/
│       └── Icons/
├── Audio/                    // Audio assets
│   ├── Music/
│   ├── SFX/
│   └── Ambience/
├── UI/                       // UI-specific Prefabs and components
│   ├── Prefabs/
│   ├── Fonts/
│   └── Animations/
├── Plugins/                  // Third-party libraries and plugins
├── Resources/                // Assets loaded dynamically at runtime
├── StreamingAssets/          // Files included directly with builds
└── Tests/                    // Unit tests and integration tests
```

---

## 📽️ Screencast

🎬 [View Screencast on YouTube](https://youtu.be/Wt_ey6ONmz8)

The screencast demonstrates:
- Core mechanics and gameplay loop
- Interaction and reporting system
- ShaderGraph and VFX Graph integrations
- AI behavior overview
- Design pattern usage (e.g., Singleton, Strategy)
- Project organisation and SOLID principles

---

## 🛠️ Technologies Used

- Unity HDRP
- ShaderGraph & VFX Graph
- Cinemachine
- C# & ScriptableObjects
- GOAP AI Framework
- GitHub for version control & issue tracking

---

## 📚 References
(Unless otherwise stated Materials and prefabs were obtained from Quixel Library)
- Unity Manual: https://docs.unity3d.com/Manual/index.html
- Shader Graph: https://unity.com/shader-graph
- GOAP Framework by CrashKonijn: https://goap.crashkonijn.com/
- Road Architect - https://github.com/FritzsHero/RoadArchitect
- VFX Graph Tutorial: [Link](#)
- Unity HDRP Documentation: https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@17.1/manual/index.html
- Smoke VFX: https://www.youtube.com/watch?v=OCzGXcdyqnQ
- Triplaner Shader: https://www.youtube.com/watch?v=mzZMlq3UAMQ&t=125s
- FootPrint Shader: https://chatgpt.com/share/682a691e-8438-8001-97fb-666e0c2fe294
- Snow Flakes - https://assetstore.unity.com/packages/2d/textures-materials/snowflakes-sprites-75874
- Player Controller: https://chatgpt.com/share/682c98ac-0b88-8001-a957-128afffd1c0a
- Buoy - "Ocean Buoy v.2 (TAO Buoy)" (https://skfb.ly/oPLJT) by TSB3DMODELS is licensed under Creative Commons Attribution-NonCommercial (http://creativecommons.org/licenses/by-nc/4.0/).
- Snow manager: https://chatgpt.com/share/682cc294-ac70-8001-a9b3-1937c0cc95f9
- Mini map icons: https://assetstore.unity.com/packages/2d/gui/icons/modern-rpg-free-icons-pack-264706
- SeismoGraph: https://sketchfab.com/3d-models/comms-room-assets-566a5b112384401aad622de6832b1c5c
- Anemometer: https://grabcad.com/library/anemometer-25
- Crates: https://assetstore.unity.com/packages/3d/props/wooden-crates-16599
- Hanging light fixtures: https://assetstore.unity.com/packages/3d/props/interior/casual-light-pack-303168
- Server Racks - https://sketchfab.com/3d-models/server-racking-system-6fe2cacf836b4aed96c650b286db5486
- Table: https://assetstore.unity.com/packages/3d/environments/training-table-136070
- Radio: https://assetstore.unity.com/packages/3d/props/hq-pbr-old-retro-radio-free-180303
- Chairs: https://sketchfab.com/3d-models/rusty-folding-chairs-636b1ae89d9449b8bcd670adbb7eb173 https://sketchfab.com/3d-models/chairs-33b684e6ae634c36ab57960ffeca296b
- Table and chair: https://sketchfab.com/3d-models/table-metallic-canteen-the-chair-ad9bb7337ba543aa9da267277bfd3940
- Sky: https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633?srsltid=AfmBOorE1HysaDu9XLLwPbDs6xaypTbnZ3JoOsap4f6V8HorgVUywPPt
- ZIL 130 Military truck - https://assetstore.unity.com/packages/3d/vehicles/land/zil-130-military-truck-208991
- Furnished camera: https://assetstore.unity.com/packages/3d/environments/urban/furnished-cabin-71426
- Icons Pack - https://assetstore.unity.com/packages/2d/gui/icons/modern-rpg-free-icons-pack-264706
- Door - https://assetstore.unity.com/packages/3d/props/metal-door-5397
- Warehouse: https://assetstore.unity.com/packages/3d/props/industrial/old-warehouse-116767
---

## 📌 License

