<p align="center">
 <img alt="GitHub " src="https://img.shields.io/github/last-commit/IBangedMyToaster/Rule34.us.Downloader">
 <img alt="GitHub " src="https://img.shields.io/github/repo-size/IBangedMyToaster/Rule34.us.Downloader"> 
 <img alt="GitHub " src="https://img.shields.io/github/downloads/IBangedMyToaster/Rule34.us.Downloader/total">
 <img alt="GitHub " src="https://img.shields.io/github/license/IBangedMyToaster/Rule34.us.Downloader">
 <img alt="GitHub " src="https://img.shields.io/github/issues-raw/IBangedMyToaster/Rule34.us.Downloader">
 <img alt="GitHub " src="https://img.shields.io/github/issues-closed-raw/IBangedMyToaster/Rule34.us.Downloader">
</p>

# What is Rule34.us Downloader
Rule34.us Downloader allows you to download and maintain content present in [Rule34.us](https://rule34.us/) and is basically an rule34.us version of [RaulS963's Downloader](https://github.com/RaulS963/Rule34-Downloader) for [Rule34.xxx](https://rule34.xxx/).
For more Information about my choice of website see [Difference between Rule34.us and Rule34.xxx](https://github.com/IBangedMyToaster/Rule34.us.Downloader#difference-between-rule34us-and-rule34xxx)

# Installation
After downloading the [latest Version](https://github.com/IBangedMyToaster/Rule34.us.Downloader/releases/latest/download/rule34.us.downloader.exe), the tool is ready to run, upon starting it will look like this:

![Pasted image 20230514204436](https://github.com/IBangedMyToaster/Rule34.us.Downloader/assets/43365830/ccc1130b-918a-4460-9b20-c55c50399073)

# Setup
On startup the file "~\\AppData\\Roaming\\ScrewsTools\\Rule34.us\\rule34.json" (if not already existend) will be created.

Within this File there are some Properties that may be modified:
   1. **SavePath**: Sets location where the library will be created. (by Default the Path is ...\\Pictures\\rule34.us)
   2. **ShadowTags**: ShadowTags are Tags that will be considered behind the scenes and apply to everything you download in order to keep foldernames simple.

# Usage
When it comes to simply dowloading Content all you need to do is to enter the appropriates Tags. The tool follows the same [conventions](https://rule34.us/index.php?r=help/search) as that of [Rule34.us](https://rule34.us/).
To see all Commands you may use `Enter Tags: --help`.

Usage:
`Enter Tags: [Command] [Tags]`

## Download Content
In Order to download Content all you need to do is enter the Tags. There is a `--download` Command, however it is complete optional.
The Following Examples result in the same Outcome.

Example1:
`Enter Tags: stable_diffusion nude`

Example2:
`Enter Tags: --download stable_diffusion nude`

![2023-05-15 13-43-32_1](https://github.com/IBangedMyToaster/Rule34.us.Downloader/assets/43365830/1d3f8aa0-35ff-4730-afcd-7019e1bac787)

![Pasted image 20230515145101](https://github.com/IBangedMyToaster/Rule34.us.Downloader/assets/43365830/8e6912ea-1c3d-4be7-a394-aa89d2f3d8ad)

It is recommended to use `Enter Tags: --show [Tags]` in Order to know how many Files you will be downloading.
In this Example also shows how exactly ShadowTags work, in green we have the input by the user (this will be the foldername), blue are ShadowTags, which are applyed to every download in the background and finally red shows the total amount of Elements found with green + blue together.

![Screenshot 2023-05-15 133003](https://github.com/IBangedMyToaster/Rule34.us.Downloader/assets/43365830/5fd91467-41eb-403f-b32a-bb0ce1b9afe4)

## Maintain Content Libraries
Following Commands help you maintain the Library:

| Command      | Tags     | Description |
|--------------|----------|-------------|
| `--update`   | Optional | Download the newest Content. |
| `--complete` | Optional | Download all missing Ids.    |
| `--clean`    | Optional | Deletes all Files that don't Match the Foldertags + Shadowtags |

# Troubleshooting
I case you run into a bug or have Problems with any of the Features i encourage you to open an issue. I will gladly take a look at the problem and solve it.

# Difference between Rule34.us and Rule34.xxx
[Rule34.us](https://rule34.us/) has a user-friendly interface, a wide variety of content, and an active community. However, it lacks content moderation, experiences occasional reliability issues, and raises legal concerns due to its explicit nature. Users should consider these factors when comparing it to [Rule34.xxx](https://rule34.xxx/).


### This Project was inspired by [RaulS963's Downloader](https://github.com/RaulS963/Rule34-Downloader) for [rule34.xxx](https://rule34.xxx/)
