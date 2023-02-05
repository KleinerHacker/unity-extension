# unity-extension
Extensions for Unity itself.

# install
Insert this repository directly to Unity.

### Dependencies
* New Input System
* https://github.com/KleinerHacker/unity-animation
* https://github.com/KleinerHacker/unity-editor-ex

### Open UPM
URL: https://package.openupm.com

Scope: org.pcsoft

# usage

### Extension Methods
* Instantiate or Destroy all game objects of a list
* Async Operation extensions to check state easier
* Effect Item extension methods to run effects directly

### Components
* `CurorSystem` for using multiple cursors (see Project Settings)
* Single behaviors for singleton pattern, see below
* `SceneQuality` to handle quality in an easy way
* `UnityDispatcher` to run code on Unity render thread
* `Raycaster` to insert a simple raycast system for pointer
  * Add `Raycaster` to the main camera as component
  * Use `Raycaster` class to add or remove listener
  * Setup raycasts in project settings (Project/Physics/Raycast)
* `RuntimeEnvironment` to check current environment for specific conditions, see project settings 'Project/Player/Environment Detection'
  * For example to detect specific input on specific plattforms (if platform is Android and TV and a keyboard exists, handle keyboard input)
  * Now with optional Steam support (requires Steam API)

### Singleton
There are two singleton pattern classes: 
* For `MonoBehavior`
* For `UIBehavior`

There are multiple variants to use this singleton. So you can use different scopes and different creation or usage strategies.

Example:

```CSharp
[Singleton(Scope = SingletonScope.Application, Instance = SingletonInstance.RequiresNewInstance, CreationTime = SingletonCreationTime.Loading)]
public class MyBehavior : SingletonBehavior<MyBehavior> {

  //Optional condition to create singlgeton or not
  [SingletonCondition]
  public static bool IsSingletonAlive() {
    //...
  }  

  //Optional initializer for created singleton instance
  [SingletonInitializer] 
  public static void InitializeSingleton(MyBehavior instance) {
    //...
  }

  //...

}
```

### Utilities
* `GameTimeController` to control game time in an easier way
  * Can differ between "Game Pause" and play time of 0

### Basics for assets
* `IdentifiableAsset` base class with auto-GUID
* `EffectItem` for effect setup in assets
* `PrefabItem` for prefab setup in assets

### Debug System (Development Build only)
* Framerate Displayer, see project settings
