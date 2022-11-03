# Working-Memory-VR-Experiment

This project seeks to provide a flexible tool to create memory based experiments in both 2D and 3D enironments. Using this tool, an experimenter is able to test users over an extensive range of customisable variables to construct test conditions for any number of hypotheses.
A single target shape in 2D             |  A 2D trial displaying five target shapes  | The start of a 2D trial             
:-------------------------:|:-------------------------:|:--------------------------:|
![2dTarget1](https://user-images.githubusercontent.com/60265517/184759346-2fef9d61-0449-486a-8005-39db490aab79.PNG) | ![2dTarget5](https://user-images.githubusercontent.com/60265517/184766663-6ca2b8ef-3c3f-4661-950f-1674306804a1.PNG) | ![2dDisplay](https://user-images.githubusercontent.com/60265517/184759385-5adaa5fe-683a-4113-9443-827253134b32.PNG)

A 2D trial with a single selected shape |  The middle of a 2D trial with five shapes selected
:-------------------------:|:-------------------------:|
![2dDisplaySelected](https://user-images.githubusercontent.com/60265517/184766608-1d4d2aeb-05d0-4e8f-9de4-2a192149283e.PNG) | ![2dDisplaySelected5](https://user-images.githubusercontent.com/60265517/184759376-b5a1a269-85ad-4980-be6b-4edbef0cc905.PNG)

<br>

Five target shapes in 3D | Three target shapes in 3D   | 
 :-------------------------:|:-------------------------:|
![3dTarget5](https://user-images.githubusercontent.com/60265517/184759947-96a59f60-41e6-4191-ba0f-974dcd7e1062.PNG) | ![3dDisplayStand](https://user-images.githubusercontent.com/60265517/184766799-20e7136d-b7e0-46b5-bc50-7ec1f36ede50.PNG)
Nine shapes in 3d with grid layout | Twenty-five shapes in 3d circular layout   | 
![3dGridDisplay](https://user-images.githubusercontent.com/60265517/184766847-5ca3721f-edbb-457c-97c8-ec9be1be0dee.PNG) | ![3dDisplayCircular25](https://user-images.githubusercontent.com/60265517/184766712-5d2b7717-b812-4b1e-86f4-760c2cf7ed8a.PNG)



## System Requiremnts

<p>
  <ul>
    <li>The system was created and tested in Unity version 2020.3.0f1, and therefore this version shall be required to access this project throguh the Unity    Editor.</li>
    <li>VR capabilities within this project were tested on a system with access to SteamVR version 1.22.13f; this version or greater is required to manage and detect connected VR devices.</li>
  </ul>
<p>

<p align="center">
  <img width="300" height="300" src= "https://user-images.githubusercontent.com/60265517/184690226-68fa930b-4cad-461b-896f-e15aed7ca23c.jpg" alt="HTC Vive Headset">
  <br>
  <em>The HTC Vive VR headset.</em>
</p>

### Note on System Testing

System was created using a Windows 10 system therefore any subsequent builds that were tested were done so using the Windows 10 platform. However, steps were taken to ensure that the system should be compatible with other platforms, despite being untested.

Accessing the project within the Unity Editor (outlined futher in <a href="https://github.com/ElliotMillington/Working-Memory-VR-Experiment/blob/main/README.md#how-to-use">How to Use</a>) will work across any platform.

## Variables
<p>
  For the purposes of the next section, a trial is an instance of a block, and a block is a collection of trials which all share the same test conditions.
</p>

<p>
  The following are a list of variables which may be manipulated by the user for a given experiment:
</p>
<p>
  <table>
    <tr>
      <th>Variable</th>
      <th>Description</th>
    </tr>
    <tr>
      <td>Dimension</td>
      <td>Indicated by the badge of each panel, this varibable determines whether the block shall be in 3D, in virtual reality, or 2D, on the application monitor. Can be inverted by clicking the badge, when VR is detected(Discuessed further in section <a href="https://github.com/ElliotMillington/Working-Memory-VR-Experiment/blob/main/README.md#design-limiations">Design Limiations</a>).</td>
    </tr>
    <tr>
      <td>Number of trials</td>
      <td>The number of trials determines the number of times a single test condtion will be repeated by the user.</td>
    </tr>
    <tr>
      <tr>
      <td>Layout</td>
      <td>This variable only affects blocks which are in 3 dimensions. By default, this can either be 'grid', which arranges shape objects in a grid pattern in front of the user, or 'circular', which arranges the shape objects around the user.</td>
    </tr>
    <tr>
      <td>Number of Target Shapes</td>
      <td>This is the number of shapes the user will need to memorise to later select within the given number of display shapes. By default, values can be 1, 3, and 5.</td>
    </tr>
    <tr>
      <td>Number of Display Shapes</td>
      <td>This is the total number of the shapes from which the user must select the target shapes they were previously shown. By default, values can be 9, 16, 25. </td>
    </tr>
    <tr>
      <td>Colours</td>
      <td>This determines the range of colours that all objects in a block can be. There are total of 8 different colours.</td>
    </tr>
    <tr>
      <td>Shapes</td>
      <td>This determines the range of shapes that all objects in a block can be. There are total of 25 different shapes.</td>
    </tr>
    <tr>
      <td>Target Display Time</td>
      <td>This is the time in which users will be able to memorise the target shapes. By default, can be between 1 and 15 seconds. </td>
    </tr>
    <tr>
      <td>Time until Display</td>
      <td>This is the time between target shapes disappearing and for the display shapes, from which the user must select, to appear. By default, can be between 0 and 15 seconds. </td>
    </tr>
    <tr>
      <td>Start Confirmation</td>
      <td>A boolean value indicating if the first trial of each new block should wait for confirmation that the user is ready to begin the next block.</td>
    </tr>
    <tr>
      <td>Random Target Shape Rotation</td>
      <td>A boolean value indicating if the target shapes should be shown to the user in a random configuarion.</td>
    </tr>
    <tr>
      <td>Random Display Shape Rotation</td>
      <td>A boolean value indicating if the display shapes should be shown to the user in a random configuarion.</td>
    </tr>
  </table>
</p>
  
Note there is also a 3 second delay between trails to ensure each trial is set up correctly.

#### List Of Available Shapes and Colours

<table>
 <tr>
  <th>
   Shapes
  </th>
  <th>
   Colours
  </th>
 </tr>
 <tr>
  <td>
   <ul>
    <li>Black</li>
    <li>Blue</li>
    <li>Green</li>
    <li>Orange</li>
    <li>Red</li>
    <li>Violet</li>
    <li>White</li>
    <li>Yellow</li>
   </ul>
  </td>
  <td>
   <ul>
    <li>Arrow</li>
    <li>Circle</li>
    <li>Crescent</li>
    <li>Cross</li>
    <li>Heart</li>
    <li>Hexagon</li>
    <li>Kite</li>
    <li>Octogon</li>
    <li>Parallelogram</li>
    <li>Pentagon</li>
    <li>Pic</li>
    <li>Polygon</li>
    <li>Qautrefoil</li>
    <li>Rectangle</li>
    <li>Rhombus</li>
    <li>Right Angle Triangle</li>
    <li>Ring</li>
    <li>Scalene Triangle</li>
    <li>Semi-circle</li>
    <li>Square</li>
    <li>Star</li>
    <li>Trapeze</li>
    <li>Triangle</li>
    <li>Trifoil</li>
   </ul>
  </td>
 </tr>
 
  

</table>

### Panel Validity
There are a number of contraints placed upon a trial for it to be valid. Each panel has an indicator denoting its current validity status (see image below). When a panel is invalid this indicator can be clicked to open a panel holding the reasons that make the panel invalid and how to resolve them. These constraints include:
<ul>
  <li> The different possible permutations of selected shapes and colours must be greater than or equal to the number of display shapes. This ensures that there are enough unique shapes to choose from.</li>
  <li> The number of trials must be an integer value greater than 0.</li>
  <li> A block which is in 3 dimensions will be invalid if VR equipment has not been detected.</li>
</ul>

<p align="center">
  <img width="500" height="300" src= "https://user-images.githubusercontent.com/60265517/184996681-736a6723-de31-4a80-8be1-4cd47c41c0ed.PNG" alt="Example of blocks being customised.">
  <br>
  <em>Example of blocks being customised.</em>
</p>

## How to Use

This project can either be used in the Unity editor, or built in the Unity editor and run as an executable. The first step is to clone the project into the desired directory by opening a command line in that directory and running command:

```bash
git clone https://github.com/ElliotMillington/Working-Memory-VR-Experiment.git
```

### Unity Editor
Once the project has been opened in the Unity editor, ensure that you have loaded the 'EntryScene', which can be found in 'Assets/Scenes' folder in the Unity inspector. Navigate to the 'Game' tab in the Unity Inspector, ensuring that the aspect ratio of the display is set to 'Full HD (1920x1080)' if not already and lower scale so that the scene fills the game window; by default aspect ratio is set to 'Free Aspect'. This should prepare the GUI for use in the editor and should appear as shown below: 

<p align="center">
  <img width="500" height="300" src= "https://user-images.githubusercontent.com/60265517/184932117-1f21d239-bbb1-4bf3-81e7-751cbbf9576b.PNG" alt="EntryScence experiment formulation panel">
  <br>
  <em>EntryScence experiment formulation panel</em>
</p>

The begin the experiment in the editor click the play button, generally found at the top of the editor.

### How to build and run the project as an executable application
Open the project in the Unity editor and navigate to File > Build Settings, this should open up the 'Build Settings' tab. Select the platform that the build is intended for and click the 'Build' button. Select the destination folder in which the build is to be created. After building is completed open the build destination folder and click the 'Working_Memory_VR_Experiment' executable file to begin the application.

#### Building Note
As stated previously in the <a href="https://github.com/ElliotMillington/Working-Memory-VR-Experiment/blob/main/README.md#system-requiremnts"> System Requirments</a> section, building has been tested using the 'PC, Mac & Linux standalone' platform, but should be compatible compatible with other platforms. 


## Output

Before a user session begins, a location to store the output data must be defined as well as a Particiapnt ID. Within this folder, session folders will be created with unique file names. These themselves will contain folders named 'participantdetails', containing Particiapnt ID, and 'othersessiondata' which will hold the following information from each trial of a given session:<br>
<p>
  <table>
    <tr>
      <th>Result Collected</th>
      <th>Description</th?
    </tr>
    <tr>
      <td>Block Number</td>
      <td>Denotes the block number that the trial occured in to be used to identify the test conditions applied(i.e. other test conditions not in this table are not recorded and should be recorded by the experimenter).</td>
    </tr>
    <tr>
      <td>Trial Number</td>
      <td>Denotes the block number that the trial occured in to be used to identify the test conditions applied. </td>
    </tr>
    <tr>
      <td>Dimension</td>
      <td>This feild indicates whether this block was in either 2D or 3D, holding values '2' and '3' respectively.</td>
    </tr>
    <tr>
      <td>Layout</td>
      <td>This value is only applicable in 3 dimensional blocks either denoting the 'grid' or 'circular' layout of the block. For 2 dimensional blocks the value is 'N/A'.</td>
    </tr>
    <tr>
      <td>Total User Time</td>
      <td>This is the time in milliseconds between when the display shape objects, the ones in which the user is to select from, become visable until when the user confirms their input.</td>
    </tr>
    <tr>
      <td>Target Shape</td>
      <td> This is a list of the shape objects, both colour and shape names, that the user should have attempted to select.</td>
    </tr>
    <tr>
      <td>Partipant Selected Shapes</td>
      <td> This is a list of the shape objects, both colour and shape names, that the user did select from the display shape objects.</td>
    </tr>
    <tr>
      <td>Correctly Selected Shapes</td>
      <td>This is a list of the shape objects, both colour and shape names, that the user did select from the display shape objects and were also target shapes.</td>
    </tr>
    <tr>
      <td>Incorrectly Selected Shapes</td>
      <td>This is a list of the shape objects, both colour and shape names, that the user did select from the display shape objects and were not target shapes.</td>
    </tr>
    <tr>
      <td>All Correct?</td>
      <td>This feild is a boolean value of 'TRUE' or 'FALSE' indicating if all partipant selected values are the same as the target values.</td>
    </tr>
  </table>
</p>


## Design Decisions

<ul>
  <li>For a user to confirm their input in any trial the number of user selected shapes <i>must</i> match the number of target shapes. </li>
  <li>For a user to confirm their input within any virtual reality space, they must perform the confirmation gesture. This is to point their controller towards the ceiling and click. This area will change colour to indicate that confirmation is valid at that time.</li>
  <li>Pressing the 'esc' button will end any running experiment while also saving any data collected from finished trials in the session.</li>
</ul>

### Design Limiations
Ensure that VR devices have been connected, and are active, at the time of application startup. If this is not the case functions which require access to VR equipment will be diabled. This will not prevent the system from running any other function.

## Citations for Publication
This is a project that anyone may use freely, however we ask in that in the instance that you do use this project in any form of publication that you should appropriately accredit this page and the following people for this tool's creation:

#### Acknowledgements

Unity Developer: <br/>
<a href="https://github.com/AndrewParker770">Andrew Parker</a> <br/>
<a href="https://github.com/ElliotMillington">Elliot Millington</a> <br/>

Project Researcher: <br/>
Sarune Savickaite
