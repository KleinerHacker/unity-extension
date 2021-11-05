# unity-extension
Extensions for Unity itself.

# install
Insert this repository directly to Unity.

### Dependencies
* New Input System
* https://github.com/KleinerHacker/unity-animation
* https://github.com/KleinerHacker/unity-editor-ex

# usage

### Extension Methods
* Instantiate or Destroy all game objects of a list
* Async Operation extensions to check state easier

### Components
* `CurorSystem` for using multiple cursors (see `CursorPreset` Asset)
* Single behaviors for singleton pattern in two ways:
  * `ObserverSingleton[UI]Behavior` store its instance on `OnEnabled`
  * `SearchingSingleton[UI]Behavior` get its instance directly from scene
* `SceneQuality` to handle quality in an easy way
* `UnityDispatcher` to run code on Unity render thread

### Utilities
* `GameTimeController` to control game time in an easier way
  * Can differ between "Game Pause" and play time of 0
