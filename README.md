# Working-Memory-VR-Experiment

This project 

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
  For the purposes of the next section, a trial is an instance of a block and a block is a collection of trials which all share the same test conditions.
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
      <td>Indicated by the badge of each panel, this varibable determines whether the block shall be in 3D, in virtual reality, or 2D, on the application monitor. Can be inverted by clicking the badge, when VR is detected(Discuessed further in section <a href="https://github.com/ElliotMillington/Working-Memory-VR-Experiment/blob/main/README.md#design-limiations">Design Decisions</a>).</td>
    </tr>
    <tr>
      <td>Number of trials</td>
      <td>The number of trials determines the number of times a single test condtion will be repeated by the user.</td>
    </tr>
    <tr>
      <tr>
      <td>Layout</td>
      <td></td>
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
      <td></td>
    </tr>
    <tr>
      <td>Target Shape Rotation</td>
      <td></td>
    </tr>
    <tr>
      <td>Display Shape Rotation</td>
      <td></td>
    </tr>
  </table>
</p>
  
Note there is also a 3 second delay between trails to ensure each trial is set up correctly.


### Panel Validity
There are a number of contraints placed upon a trial for it to be valid.

## Output

## Design Decisions

<ul>
  <li>For a user to confirm their input in any trial the number of user selected shapes <i>must</i> match the number of target shapes. </li>
  <li>For a user to confirm their input within any virtual reality space, they must perform the confirmation gesture. This is simply to point their controller towards the ceiling and click their controller. This area will chnage colour to indicate that confirmation is valid.</li>
</ul>

### Design Limiations
Ensure that VR devices have been connected, and are active, at the time of application startup. If this is not the case, functions which require access to VR equipment will be diabled. This will not prevent the system from running any other function other.

## Acknowledgements

Unity Development: <br/>
<a href="https://github.com/AndrewParker770">Andrew Parker</a> <br/>
<a href="https://github.com/ElliotMillington">Elliot Millington</a> <br/>

Project Researcher: <br/>
Sarune Savickaite
