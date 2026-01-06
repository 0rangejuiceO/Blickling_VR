How To Run as of 01/2026

- Open this in unity (currently using editor version 6000.2.13f1)
- Go to File - Build Profiles 
- Make sure Windows is Active then select Build.
- Once it is built connect the HMD while in developer mode using meta quest link app and cable (Only tested with Meta Quest 3)
- Run the exe and it should display both on your computer and in the HMD.

Please let me know if this successfully runs with and other hardware as it has only been tested with:
- HMD: Meta Quest 3
- Connection Method: Meta Quest Link App with Syntech USB A to USB C Cable
- Operating System: Windows 11 Home 64-bit
- CPU: AMD Ryzen 7, Granite Ridge 4nm Technology
- RAM: 32.0GB Dual-Channel Unknown @ 2998MHz (36-44-44-96)
- Graphics: 4034MB NVIDIA GeForce RTX 5070 (ASUStek Computer Inc), 512MB ATI AMD Radeon Graphics (ASUStek Computer Inc), SLI Disabled, CrossFire Disabled

\
\
\
\
#Original How To Run

Should be as easy as opening this in Unity and running it in the Editor for the most part - certain packages will likely need to be installed, but the setup of these can be found in the repo https://github.com/JacobJEdwards/UnityGaussianSplatting, which is a fork of the unity gaussian splatting repo which adds cleverer resorting (only if the head has been moved enough) and frustrum culling (dont render splats behind the camera).
Follow the readme in that repo and running this should be simple enough.
There are a few gotchas, particulary around the type of renderer so please follow the instuctions to the letter, otherwise you can ask me questions (can always leave comments on this repo).

Then to get the project to actually build and run on the headset:
- Plug in the vr headset
- May need to allow developer access or something in the quests settings (there will be lots online about this, just google or ask ai)
- Then in the project settings in unity choose the build device as the quest (it should show up as a device, if it doesnt then just google it)
also note that it must be an android or meta build device.
