
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

🎬 [View Screencast on YouTube](#)

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

- Unity Manual: https://docs.unity3d.com/Manual/index.html
- Shader Graph: https://unity.com/shader-graph
- GOAP Framework by CrashKonijn: https://goap.crashkonijn.com/
- VFX Graph Tutorial: [Link](#)
- Unity HDRP Documentation: https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@17.1/manual/index.html

---

## 📌 License

