# Eden Engine
Eden is a new small game engine built ontop of raylib. It is based on Monocle which was an open source MIT licensed engine using FNA that I've used and modified over the years. Eden is something of a port, though using the conventions and features of raylib.

The entry point to Eden is the `Eden.Engine` class. You will want to extend it to implement your startup logic. To start the game simply create an instance of your engine and call `engine.Run()`.

To make the engine do stuff you need to create scenes by extending the `Scene` class. It manages `Entity` objects which themself are groups of `Component` objects. 
1. Use components to implement reusable bits of logic for your entities to use
2. Use entities to group components and keep data to be used by components
3. Use scenes to assemble you world with its entities. Scenes also manage rendering logic.
4. Lastly the Engine is responsible for running and switching out the scene.