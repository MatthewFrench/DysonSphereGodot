# godot-next

Godot Node Extensions, AKA Godot NExt, is a repository dedicated to collecting basic node extensions that are currently unavailable in vanilla Godot.

As you might have noticed, Godot Engine's initial node offerings are general purpose and are intentionally not oriented towards particular types of games.

The intent is to create nodes that fulfill a particular function and work out-of-the-box. Users should be able to use your node immediately after adding it to their project.
Ideally, these are single nodes rather than whole systems of nodes (for that, you might be better off building a separate repository for that system).

[Jump to Node List](https://github.com/willnationsdev/godot-next#nodes)

## How to Use

1. If godot-next is on the Asset Library already, simply plugin the asset from the Asset Library browser. Otherwise, download the repository as a .zip from GitHub. Unzip the archive and copy the files into your project directory (you just need to put the `godot-next` folder into the `res://addons/` directory for your project).

2. Create or open a project.

3. Open Project Settings and go to the Plugins tab.

4. Find the `godot-next` plugin and click the Active button on the right-hand side.

5. Restart the engine. You should now be able to create each new type of node in your project!

Note that when you add a custom type node to your node hierarchy, it may already have a script attached to it (the script associated with the custom type). This **is not** a bug. Any script, even one defined by `add_custom_type`, still needs to be actually attached to the node in order for the node to still be custom. All the EditorPlugin does is change the icon, give it a default script, and add it to the "Create New Node" window (the last part only works if you use an in-engine node as a base type though).

You'll also see, at least in 2.1.4, that when you remove this script, the Add A Script button in the top-right will still show a trash-can icon. This probably **is** a bug, albeit an understandable one. Thankfully, you can still assign a new script by going to the bottom of the Inspector and directly assigning a script property.

Also note that when extending custom node types with scripts, those you cannot use `extends NodeType`, even if it was added via an EditorPlugin. You will need to use the path directly:

    extends "res://addons/godot-next/path/to/MyClass/MyClass.gd"

## How to Contribute

### Ideas
If you have an idea for a node that you would like to have added to the repository, create a new Issue.

### In Progress
Work-in-progress nodes should be kept in forked repositories until completed. Feel free to edit the README file's Work In Progress section with a link to your repository's content.

### Completed

If you would like to add your own node to the repository, do the following:

1. Fork the repository to your GitHub.

2. Clone the repository and switch to the branch (major.minor engine version) you are adding a node for.

3. Identify a place in the repository folder hierarchy that seems appropriate for your node. Folders should be named in association with a related topic. If you must create new directories, please follow a snake_case naming scheme when creating topical directories.

4. Once you've found a suitable location, create a new directory, this time using UpperCamelCase.

5. Inside the new folder, attach the content necessary for your node. For GDScript, this would be a `.gd` file. For CSharp (when it's available), this would be a `.cs` file (as far as I know). For GDNative scripts, the folder should contain the `.gdns` file, a GDNativeLibrary `.tres`, and one dynamic library file for each major desktop platform (Windows, Mac OS, Linux) along with any and all custom source code (not part of the bindings) that was used to generate the dynamic libraries. **For GDNative scripts, please attempt to acquire as many platforms as possible so that all nodes will be available to every user.**

- Be sure to include in your node a comment at the top saying "Contributors" along with a subsequent comment detailing your GitHub username after a hyphen. For example:
```
    # Contributors
    # - willnationsdev
```
- Please keep the content of your script in the appropriate style for the type of script you are making to maintain consistency. `.gd` files should use snake_case for non-script-type variable names. `.cs` files likewise should use UpperCamelCase for typenames and lowerCamelCase for variable and function names. Finally, any and all GDNative scripts should use the style conventions of the bound language. The exception to this would be GDNative C++, in which case, please use the style preferred in Godot Engine source code (snake\_case for variables/functions, UpperCamelCase for typenames). For example:
```
    # GDScript
    var my_var = null
    var ScriptType = preload("res://script.gd")

    // CSharp
    public int myVar = null;
    public class MyClass {};

    // GDNative C++ (if a different language, then you should use whatever convention is appropriate for that language)
    int my_var = null;
    class MyClass {};
```

- Scripts will have varying degrees of efficiency / compatibility based on language, but because logistically-speaking it would be difficult to maintain multiple scripts per node, please only use one scripting language per node. If you would like to port from one language to another, make an Issue to discuss and update the community first. Priority should favor things that won't break compatibility, but should favor efficiency within that constraint. So that would mean a hierarchy of VisualScript < GDScript < C# < GDNative (with desktop/mobile/web platforms all setup). For GDNative, always preserve a non-GDNative implementation (if there is one) as a backup. GDNative also shouldn't completely replace other implementations in `godot_next_plugin.gd` until all platforms have a dynamic library prepared for use.

6. Also inside the new folder, you should have a 16x16 .png image file that appears as consistent as possible with the editor's pre-existing node images. The name of the file should follow the pattern: icon\_(snake\_case\_node\_name).png. For example, for the type MyNode, the proper name would be `icon_my_node.png`.

7. Once you have your script and image file handy, go to the `godot_next_plugin.gd` file and add/remove the custom type using the `add_custom_type` and `remove_custom_type` methods, passing in the preloaded paths to your script and image files. Note that the second parameter, the base type of the custom node, WILL need to be the name of an in-engine node (it's a bug for now - unfortunately it makes it messier to work with nodes derived from existing custom nodes).

8. Go to the README.md file and add the name of any added nodes to the list of included nodes along with any hashtags you would like to attach (please keep it to 3 or less). The name of the node should be a relative link to its location in the repository. If possible, try to find a space nearby other nodes of a similar type. For GDNative scripts, the description should specify what platforms have been made available.

9. Commit and push all of your changes

    1. the new directory with an UpperCamelCase name.
    2. the script-related file(s) with an UpperCamelCase name (with contributor credits).
    3. the .png file with an icon\_prefixed\_snake\_case name.
    4. the modified `godot_next_plugin.gd` file to add and remove your node from the editor.
    5. the modified README.md file to add your node to the description of the repository's content. (GDNative scripts should specify what platforms have been made available)

10. Submit a pull request to the original repository

If you would like to make edits to an existing node, don't forget to add your name to the list of contributors within that node's script.

If possible, please try to create working scripts for each version of the engine supported by the branches. One way that you as a community member can help is by porting existing nodes from one engine version to another.

If you find any problems with a particular plugin or have suggestions for improving it and would like to launch a discussion, please create an Issue and mention one of the active developers associated with it (you can click on the file and then click on the History button to see a list of commits that have edited the file. Common usernames will give you an idea of who to mention).

That's it! I hope you've got ideas of what you'd like to share with others.

# Nodes

|Linkable Node Name|Description|Language|Tags
|-|-|-|-|
|[HoverContainer](addons/godot-next/gui/containers/HoverContainer/HoverContainer.gd)|A container that emits hover events periodically along with signaling mouse clicks.|GDScript|\#gui \#Control \#hover
|[BaseSwitcher](addons/godot-next/node_manipulation/BaseSwitcher/BaseSwitcher.gd)|A generic node used to apply a state to one of a set of nodes, cycling between as needed.|GDScript|\#switch \#Node \#cycle
|[ControlSwitcher](addons/godot-next/gui/navigation/ControlSwitcher/ControlSwitcher.gd)|A node that sets only one of its Control children to be visible or in focus.|GDScript|\#navigation \#Node \#cycle

# Work In Progress Nodes

These are nodes that others are actively working to contribute to the repository. Clicking the link should take you to the new node's code so-far-implemented in their forked repository.

|Linkable Node Name|Description|Language|Tags
|-|-|-|-|

