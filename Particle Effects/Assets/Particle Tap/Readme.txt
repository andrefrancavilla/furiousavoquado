The TapAndDrag component can be placed on any GameObject within the current scene.
All you have to do is assign your particle effects to the script as prefabs and the script will do the rest.

Important notes about layers and particle system configuration:

	>To make the particles appear over the UI, you must assign the UI to a sorting layer of its own and then
		assign the particles to another sorting order created after the previous one. In the example, 
		this is already configured.

	> The *tap* particle system must have the following stop action: Disable.

	> The *drag* particle system must have its emission's rate over time set to 0 and rate over distance
		must be greater than 0.