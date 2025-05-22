# Tethered Climb

> Plataforma cooperativa 2D • Unity 2022.3 • C#

![Unity 2022](https://img.shields.io/badge/Unity-2022.3-blue?logo=unity)

---

## 🎮 Gameplay

Dos aventurers units per una **corda elàstica** han de col·laborar per recollir **10 monedes** i tornar-les al *Senyor Seta*.

* Físicament connectats: si un cau, l’altre pot estirar‑lo.
* Doble salt, *wall‑jump* i retractar corda per arribar més amunt.
* Fletxa guia que apunta sempre a la moneda més propera.

<p align="center">
  <img src="Docs/preview.gif" width="600" alt="Gameplay preview"/>
</p>

---

## 📂 Estructura del projecte

```
TetheredClimb/
├─ Assets/
│  ├─ Art/               # Sprites i tileset (Aseprite)
│  ├─ Audio/             # BGM i SFX
│  ├─ Scenes/            # MainMenu, LoadingScene, Level01, FinalScene...
│  └─ Scripts/
│      ├─ TetheredClimb/
│      │   ├─ FollowCamera.cs
│      │   ├─ GameManager.cs
│      │   └─ AudioManager.cs
│      ├─ Players/
│      │   ├─ Player1Controller.cs
│      │   └─ Player2Controller.cs
│      ├─ Rope/
│      │   ├─ RopeGenerator.cs
│      │   └─ RopeBetweenPlayers.cs
│      └─ UI/
│          ├─ FlechaGuiaUI.cs
│          └─ NPCDialogue.cs
└─ README.md
```

### Scripts destacats

| Nom               | Descripció                                                                | Fitxer                                                                                  |
| ----------------- | ------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- |
| **FollowCamera**  | Càmera ortogràfica que centra el punt mitjà dels jugadors i aplica límits | [`Assets/Scripts/TetheredClimb/FollowCamera.cs`](Assets/Scripts/Core/FollowCamera.cs)   |
| **RopeGenerator** | Crea la corda amb `HingeJoint2D` i LineRenderer                           | [`Assets/Scripts/TetheredClimb/RopeGenerator.cs`](Assets/Scripts/Rope/RopeGenerator.cs) |
| **GameManager**   | Estat global de la missió, compta monedes i canvia d’escena               | [`Assets/Scripts/TetheredClimb/GameManager.cs`](Assets/Scripts/Core/GameManager.cs)     |
| **AudioManager**  | Música en loop + SFX amb *ducking* durant diàlegs                         | [`Assets/Scripts/TetheredClimb/AudioManager.cs`](Assets/Scripts/Core/AudioManager.cs)   |
| **NPCDialogue**   | Diàleg lletra‑a‑lletra i inici de la missió                               | [`Assets/Scripts/TetheredClimb/NPCDialogue.cs`](Assets/Scripts/UI/NPCDialogue.cs)       |

---

## ⌨️ Controls

| Acció          | Jugador 1                 | Jugador 2 |
| -------------- | ------------------------- | --------- |
| Moure          | **A / D**                 | **← / →** |
| Salt           | **W**                     | **↑**     |
| Escurçar corda | **L**                     | **P**     |
| Parlar amb NPC | **E** (qualsevol jugador) |           |

---

## 🚀 Instal·lació

### Dependències

* Unity 2022.3 o superior.

---

## 🗺 Full de ruta

* [x] Mecànica de corda estable
* [x] Fletxa guia de monedes
* [x] Menú + pantalla de càrrega asíncrona
* [ ] Parallax background (polish)
* [ ] Suport *gamepad* (Input System)
* [ ] Nous nivells i enemics

---

## 📜 Llicència

Aquest projecte està publicat sota llicència **MIT** — consulta `LICENSE` per a més detalls.
