# How To Run

Should be as easy as opening this in Unity and running it in the Editor for the most part - certain packages will likely need to be installed, but the setup of these can be found in the repo https://github.com/JacobJEdwards/UnityGaussianSplatting, which is a fork of the unity gaussian splatting repo which adds cleverer resorting (only if the head has been moved enough) and frustrum culling (dont render splats behind the camera).
Follow the readme in that repo and running this should be simple enough.
There are a few gotchas, particulary around the type of renderer so please follow the instuctions to the letter, otherwise you can ask me questions (can always leave comments on this repo).

Then to get the project to actually build and run on the headset:
- Plug in the vr headset
- May need to allow developer access or something in the quests settings (there will be lots online about this, just google or ask ai)
- Then in the project settings in unity choose the build device as the quest (it should show up as a device, if it doesnt then just google it)
also note that it must be an android or meta build device.
