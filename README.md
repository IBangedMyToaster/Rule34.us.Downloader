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
After downloading the latest Version the tool is ready to run, upon starting it will look like this:

![Pasted image 20230514204436](https://github.com/IBangedMyToaster/Rule34.us.Downloader/assets/43365830/ccc1130b-918a-4460-9b20-c55c50399073) 

# Setup
On startup the file "~\\AppData\\Roaming\\ScrewsTools\\Rule34.us\\rule34.json" (if not already existend) will be created.

Within this File there are some Properties that may be modified:
   1. **SavePath**: Sets location where the library will be created. (by Default the Path is ...\\Pictures\\rule34.us)
   2. **ShadowTags**: ShadowTags are Tags that will be considered behind the scenes and apply to everything you download in order to keep foldernames simple.

# Usage
When it comes to simply dowloading Content all you need to do is to enter the appropriates Tags. The tool follows the same [conventions](https://rule34.us/index.php?r=help/search) as that of [Rule34.us](https://rule34.us/).

Usage:
```
Enter Tags: [Command] [Tags]
```

To see all Commands you may use:
```
Enter Tags: --help
```

## Download Content
In Order to download Content all you need to do is enter the Tags. There is a `--download` Command, however it is complete optional.
The Following Examples result in the same Outcome.

Example1:
```
Enter Tags: stable_diffusion nude
```

Example2:
```
Enter Tags: --download stable_diffusion nude
```

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

### Automation
Since it is possible to run the Tool with Arguments it is possible to create a script to automatically update the Library on Startup of your machine.

If you simply want to update the whole Library your .bat File would look similiar to this:
```
"rule34.us Downloader.exe" --update
```

Move the .bat File into `~\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup` and it will keep everything Up-To-Date.

# Commands
There are only a handful of commands, and they are quite simple, but they can come in handy for some extra quality of life. You can only use one command at a time. Commands are preceded by the prefix '--'.

It is worth mentioning that the currently supported size of a **single content folder within the library** is approximately **10,030 files (or 238 pages)** due to a limitation of rule34.us. Technically, the site supports more by making some small changes, but since it has never been necessary, that is the current maximum. If you need more than that, I encourage you to open an issue, and I will implement it.

## Download
The Download Command really only exists for Completion-Reasons. It is completey optional and does not effect the Result.

Usage:
```
Enter Tags: stable_diffusion nude
```
Alternative :
```
Enter Tags: --downlaod stable_diffusion nude
```

As the Name already indicates, Content with the **given Tags + Shadow Tags** will be downloaded and saved.

## Help
Display all Commands, thats it..
![image](https://github.com/IBangedMyToaster/Rule34.us.Downloader/assets/43365830/4ce32f19-127e-420b-87fb-9214dd1dcf49)

## Update
Probably the biggest one for me on this List when it comes to Life-Quality.
As shown in [Maintain Content Libraries](https://github.com/IBangedMyToaster/Rule34.us.Downloader#maintain-content-libraries) the Tags following the Command are optional since they refer to your already Present Library.
You can update your Entire Library or just a specific Folder within this Library. In Order to just do one Folder you can follow up the Command with the Tags of the Folder or just copy the Foldername.

Update entire Library:
```
Enter Tags: --update
```

Update Specific Folder (by entering Tags):
```
Enter Tags: --update stable_diffusion nude
```
Update Specific Folder (by copying Foldername):
```
Enter Tags: --update stable_diffusion & nude
```

To keep your Library Up-To-Date, see [Automation](https://github.com/IBangedMyToaster/Rule34.us.Downloader#automation).

## Complete
Complete is very similiar to the Update Command, the main Difference is that instead of just looking at the last File in the Folder and Downloading new ones, Complete compares all IDs from the Site and what is currently present in the Folder and fills in the blanks. The Complete Command **does only add** to the collection, it does not delete Content.
Supports specific Folders aswell.

Complete entire Library:
```
Enter Tags: --complete
```

Complete Specific Folder (by entering Tags):
```
Enter Tags: --complete stable_diffusion nude
```
Complete Specific Folder (by copying Foldername):
```
Enter Tags: --complete stable_diffusion & nude
```

## Clean
Cleaning your Library is a good idea after Tags changed (Shadow Tags got added or Foldernames changed).
Supports specific Folders aswell.

Clean entire Library:
```
Enter Tags: --clean
```

Clean Specific Folder (by entering Tags):
```
Enter Tags: --clean stable_diffusion nude
```
Clean Specific Folder (by copying Foldername):
```
Enter Tags: --clean stable_diffusion & nude
```

## Show
As shown in [Download Content](https://github.com/IBangedMyToaster/Rule34.us.Downloader#download-content) it is recommended by me to make a little check to know how much Content you are looking forward to. I mostly just enjoy watching Tags strip away unwanted Content :)

```
Enter Tags: --show stable_diffusion nude
```

# Unwanted Content in your Library
Became a Victim of a Tag you do not want to see content of anymore? Or did you just change Foldernames or Shadow Tags? Heres how i personally deal with those situations:

## While browsing
I like to copy the Filename which is also the ID and go to [rule34.us](https://rule34.us/), click on **"View all Images"**, select any image and change the ID in the Adressbar with the one i copied.

![image](https://github.com/IBangedMyToaster/Rule34.us.Downloader/assets/43365830/77851a71-8766-42f0-bc48-d8fa7f482b7d)

After pressing Enter with the right ID in place, i try to find out which Tag i need to Filter out.
It is more of a Trial and Error type of Problem.

Can not find the right Tag to Filter or the File barely has any Tags? See [Issue that can not be fixed](https://github.com/IBangedMyToaster/Rule34.us.Downloader#issue-that-can-not-be-fixed).

Add the Tag to the Shadow Tags, reopen the App

![image](https://github.com/IBangedMyToaster/Rule34.us.Downloader/assets/43365830/d13d8cb0-a86b-4a6e-8a86-ad4adf474e69)

and follow it up with a 
```
Enter Tags: --clean
```

# Troubleshooting
In case you run into a bug or have Problems with any of the Features i encourage you to open an issue. I will gladly take a look at the problem and solve it.
You can also reach out to me on Discord *Nesto#9561*

## Issue that can not be fixed
At the end of the day, the app has a very delicate and unreliable dependency. The artist/uploader decides what tags a file has and becomes the biggest wildcard when it comes to consistent results. As someone with roughly 50k files, I can say that around 90% of them are tagged quite accurately; it's the remaining 10% that can cause issues. Personally, I assign shadow tags to artists with poor tagging skills.

# Difference between Rule34.us and Rule34.xxx
Rule34.us and Rule34.xxx are both websites dedicated to the sharing and discovery of explicit adult content, specifically related to Rule 34, which states that "if it exists, there is porn of it." While both platforms serve similar purposes, they differ in terms of user experience, content moderation, and community engagement. Here are the pros and cons of Rule34.us compared to Rule34.xxx:

**Pros of Rule34.us:**

1.  User-Friendly Interface: Rule34.us is known for its user-friendly and intuitive interface, making it easy to navigate and search for specific content. The website's design and layout are optimized for a smooth user experience.
2.  Wide Variety of Content: Rule34.us boasts a vast collection of adult content related to various fandoms, including cartoons, video games, and movies. Users can find a diverse range of material to suit their preferences.
3.  Active Community: The website has an active and engaged community, with users frequently interacting through comments and discussions. This fosters a sense of community and allows users to connect with like-minded individuals.

**Cons of Rule34.us:**

1.  Lack of Content Moderation: Rule34.us has been criticized for its limited content moderation. As a result, some explicit and potentially offensive material may be present on the website, making it unsuitable for individuals seeking more curated or moderated content.
2.  Reliability and Stability: Rule34.us has faced occasional issues with reliability and stability, including server downtime and slow loading times. This can be frustrating for users trying to access the website consistently.
3.  Questionable Legality: Due to the explicit nature of the content, there may be legal concerns associated with accessing and sharing adult material on Rule34.us. Users should exercise caution and ensure compliance with local laws and regulations.

In comparison, Rule34.xxx is another popular platform with its own set of advantages and disadvantages. It offers a different user experience, content moderation approach, and community atmosphere. Users should consider their preferences and priorities when choosing between the two websites.


### This Project was inspired by [RaulS963's Downloader](https://github.com/RaulS963/Rule34-Downloader) for [rule34.xxx](https://rule34.xxx/)
