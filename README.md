# Eco Design and Safety Architectural Suite

## Project Description
Interactive 3D Unity-based software for designing houses with furniture placement, furniture material choices, wall material choices and floor material choices. The software provides fire-safety and sustainability score evaluation based on design choices, feedback is also available for the scores.


## Folder structure

The source code folder is called "Eco Design Fire Safety." Inside the source code folder all the assets and packages used for the project are available. 
- The main executable file is located in the folder called 'Executable' which is outside of the main source code folder in the main directory.
- The C# scripts are located in the source code folder in file path 'Assets/Scripts'. 
- Only two scripts for player controller are located in a different file path which is 'Assets/First Person and Cursor/First Person Player'. 
- The furniture scriptable objects for inventory menu are located in 'Assets/Resources/Furniture' file path.
- The material data scriptable objects are located in their own respective folder corresponding either furniture, floor or wall material in 'Assets/Resources/Furniture&House Materials'
- The main menu scene and loading screen scene are located in 'Assets/Scenes' file path in source code folder.


## Running The Software
In order to run the software, the executable folder must be downloaded to local computer. The executable file is called "EcoDesignandFireSafety.exe". Simply running the executable file should open the software.
Alternatively the source code folder can be download and opened in unity editor. In order to do that the zipped folder downloaded from gitlab must be unzipped to locate the source code folder which is "Eco Design Fire Safety". The unity hub need to be open and from unity hub, option for adding project from local disk should be selected. When the window file explorer pop up menu is open the source code folder must be located and selected to load in unity editor.

## External Resources 
- The main scene is located in the source code folder in file path 'Assets/Modern Homes'. The main scene is part of an asset from Unity asset store called "Modern Homes Pack" [1].
- The player controller located in source code folder in file path 'Assets/First Person and Cursor' is part of an unity asset called "Apartment Kit" [2]. The scripts for player controller were adapted to include jump functionality for the player. 
- The furniture prefabs located in the file path 'Assets/Modern Homes/Furniture and Props Simple' are also part of the same unity asset called "Apartment Kit" [2]. 
- The DontDestroy script located in 'Assets/Scripts' is directly adapted from a tutorial video on youtube [3].
- The 'WindTurbine' 3D object loacted in 'Assets/Material and Miscellaneous' is a 3D object downloaded from TurboSquid [4].


## References
[1] Unity Asset Store. “Modern Homes Pack.” assetstore.unity.com. [https://assetstore.unity.com/packages/3d/environments/urban/modern-homes-pack-68912](https://assetstore.unity.com/packages/3d/environments/urban/modern-homes-pack-68912). (accessed Jan. 2, 2024).
[2]  Brick Project Studio. “Apartment Kit.” assetstore.unity.com. https://assetstore.unity.com/packages/3d/environments/apartment-kit-124055 (accessed Jan. 2, 2024). 
[3] JTAGames, How To Make a Don't Destroy on Load (One Script for Every Object) (Unity Tutorial). (Mar. 26, 2021). Accessed Mar. 2, 2024. [Online Video]. Available: https://www.youtube.com/watch?v=HXaFLm3gQws
[4] Scheissegalo. "Wind Turbine." TurboSquid.com https://www.turbosquid.com/3d-models/free-3ds-model-environmentally-friendly/964716 (accessed May. 3, 2024).