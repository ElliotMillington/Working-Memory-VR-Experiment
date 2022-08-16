# Working-Memory-VR-Experiment

This project seeks to provide a flexible tool to create memory based experiments in both 2D and 3D. Using this tool an experimenter is able to test users over an extensive range of customisable varibales to construct test conditions for any number of hypotheses.
A single target shape in 2D             |   The Start of a 2D Trial     |  The middle of a 2D trial with five shapes selected                       |
:-------------------------:|:-------------------------:|:--------------------------:|
![2dTarget1](https://user-images.githubusercontent.com/60265517/184759346-2fef9d61-0449-486a-8005-39db490aab79.PNG) | ![2dDisplay](https://user-images.githubusercontent.com/60265517/184759385-5adaa5fe-683a-4113-9443-827253134b32.PNG) | ![2dDisplaySelected5](https://user-images.githubusercontent.com/60265517/184759376-b5a1a269-85ad-4980-be6b-4edbef0cc905.PNG)

A 2D trial with a single selected shape |  A 2D trial displaying five target shapes  |   Five target shapes in 3D
 :-------------------------:|:-------------------------:|:--------------------------:|
![2dDisplaySelected](https://user-images.githubusercontent.com/60265517/184766608-1d4d2aeb-05d0-4e8f-9de4-2a192149283e.PNG) | ![2dTarget5](https://user-images.githubusercontent.com/60265517/184766663-6ca2b8ef-3c3f-4661-950f-1674306804a1.PNG) |    ![3dTarget5](https://user-images.githubusercontent.com/60265517/184759947-96a59f60-41e6-4191-ba0f-974dcd7e1062.PNG)

A 2D trial with a single selected shape |  A 2D trial displaying five target shapes  |
 :-------------------------:|:-------------------------:|
![2dDisplaySelected](https://user-images.githubusercontent.com/60265517/184766608-1d4d2aeb-05d0-4e8f-9de4-2a192149283e.PNG) | ![2dTarget5](https://user-images.githubusercontent.com/60265517/184766663-6ca2b8ef-3c3f-4661-950f-1674306804a1.PNG)

Five target shapes in 3D | Three target shapes in 3D   | 
 :-------------------------:|:-------------------------:|
![3dTarget5](https://user-images.githubusercontent.com/60265517/184759947-96a59f60-41e6-4191-ba0f-974dcd7e1062.PNG) | ![3dDisplayStand](https://user-images.githubusercontent.com/60265517/184766799-20e7136d-b7e0-46b5-bc50-7ec1f36ede50.PNG) |
:----------------------------:|:-------------------------------:|
Nine shapes in 3d with grid layout | Twenty-five shapes in 3d circular layout   | 
![3dGridDisplay](https://user-images.githubusercontent.com/60265517/184766847-5ca3721f-edbb-457c-97c8-ec9be1be0dee.PNG) | ![3dDisplayCircular25](https://user-images.githubusercontent.com/60265517/184766712-5d2b7717-b812-4b1e-86f4-760c2cf7ed8a.PNG) |



## System Requiremnts

<p>
  <ul>
    <li>The system was created and tested in Unity version 2020.3.0f1, and therefore this version shall be required to access this project throguh the Unity    Editor</li>
    <li>VR capabilities within this project were tested on a system with access to SteamVR version 1.22.13f, which is required to manage and detect connected VR devices</li>
  </ul>
<p>


<p align="center">
  <img width="300" height="300" src= "https://user-images.githubusercontent.com/60265517/184690226-68fa930b-4cad-461b-896f-e15aed7ca23c.jpg" alt="HTC Vive Headset">
  <br>
  <em>The HTC Vive VR headset.</em>
</p>

## Variables
<p>
  For the purposes of the next section, a trial is an instance of a blocka, and a block is a collection of trials which all share the same test conditions.
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
      <td>A boolean value indicating if the target shapes should be shown to the user in a random configuarion. This option is only avaible for blocks in 3 dimension.</td>
    </tr>
    <tr>
      <td>Random Display Shape Rotation</td>
      <td>A boolean value indicating if the display shapes should be shown to the user in a random configuarion. This option is only available for blocks in 3 dimension.</td>
    </tr>
  </table>
</p>
  
Note there is also a 3 second delay between trails to ensure each trial is set up correctly.


### Panel Validity
There are a number of contraints placed upon a trial for it to be valid. Each panel has an indicator denoting its current validity status. When a panel is invalid this indicator can be clicked to open a panel holding the reasons that make the panel invalid and how to resolve them. These constraints include:
<ul>
  <li> The different possible permutations of selected shapes and colours must be greater than or equal to the number of display shapes. This ensures that there are enough unique shapes to choose from.</li>
  <li> The number of trials must be an integer value greater than 0.</li>
  <li> A block which is in 3 dimensions will be invalid if VR equipment has not been detected.</li>
</ul>

## Output

Before a user session begins, a location to store the output data must be defined as well as a Particiapnt ID. Within this folder session folders will be created with unique file names. These themselves will contain folders named 'participantdetails', containing Particiapnt ID, and 'othersessiondata' which will hold the following information for each trial of a given session:<br>
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
      <td>This feild indicates whether this block was in either 2D('2') or 3D('3').</td>
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
  <li>For a user to confirm their input within any virtual reality space, they must perform the confirmation gesture. This is to point their controller towards the ceiling and click. This area will change colour to indicate that confirmation is valid.</li>
</ul>

### Design Limiations
Ensure that VR devices have been connected, and are active, at the time of application startup. If this is not the case, functions which require access to VR equipment will be diabled. This will not prevent the system from running any other function.

## Acknowledgements

Unity Development: <br/>
<a href="https://github.com/AndrewParker770">Andrew Parker</a> <br/>
<a href="https://github.com/ElliotMillington">Elliot Millington</a> <br/>

Project Researcher: <br/>
Sarune Savickaite
