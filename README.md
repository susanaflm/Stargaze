# Stargaze (Source Repo)

## Index

- [Description](#Description)
- [Requirements](#Requirements)
- [Project Structure](#Project-Structure)
- [Preferred File Types](#Preferred-File-Types)
    - [3D Models](#3D-Models)
    - [Textures/Sprites](#Textures/Sprites)
    - [Sounds](#Sounds)
- [Naming Conventions](#Naming-Conventions)

## Description

- This repo contains only the necessary files to compile the game, so no Blender or Substance files.

## Requirements

- Unity 2021.3.1f1

## Project Structure

```
|- Input                                InputSystem action files
|- RendererSettings                     Renderer related files
|- Resources                            Every game asset (3D models, fonts, UI, ...)
    |- Cool asset
        |- Models                       Exported models (.fbx, .obj, ...)
        |- Textures                     Exported textures (.png, ...)
        |- Materials                    Unity material files
        CoolHouse.prefab                Unity prefab for this asset
    |- ...
    |- Vendor                           Every third-party asset pack (like 'Sci-Fi Styled Modular Pack')
|- Scenes                               Every scene and scene templates
    |- Templates                        Template scenes (May or may not be used)
    |- VeryNiceScene.scene
    |- ...
|- ScriptableObjects                    Every ScriptableObject
|- Scripts
    |- Input                            InputSystem generated classes
    |- Mono                             Every MonoBehaviour script
    |- ScriptableObjects                Every ScriptableObject classes
    |- Tools                            Custom editors, automations, ...
    |- Shaders                          Shaders source code
|- Vendor                               Every third-party libraries (like Mirror)
    |- Mirror
    |- ...
```

## Preferred File Types

### 3D Models

```.fbx``` is the ideal.

### Textures/Sprites

```.tiff``` is the best but ```.png``` is fine too.

### Sounds

```.wav``` is the best when available but ```.mp3``` is ok when ```.wav``` is unavailable.

## Naming Conventions

Try to follow a CamelCase naming convention as much as possible for folders files and objects.
Using dashes, underscores or dots for separating separating words or suffixes is allowed.

Do:
```
CoolAsset.fbx
Super Cool Asset Folder
CoolAsset-Extra.fbx
CoolAsset.L.fbx
CoolAsset_Normal.png
```

Don't Do:
```
coolAsset.fbx
coolasset.fbx
cool_asset.fbx
cool asset folder
```