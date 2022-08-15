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



## Design Decisions

<ul>
  <li>For a user to confirm their input in any trial the number of user selected shapes <i>must</i> match the number of target shapes. </li>
  <li>For a user to confirm their input within any virtual reality space, they must perform the confirmation gesture. This is simply to point their controller towards the ceiling and click their controller. This area will chnage colour to indicate that confirmation is valid.</li>
</ul>

### Design Limiations
Ensure that VR devices have been connected, and are active, at the time of application startup. If this is not the case functions which require access to VR equipment will be diabled. This will not prevent the system from running any other function other.

## Acknowledgements

Unity Development: <br/>
<a href="https://github.com/AndrewParker770">Andrew Parker</a> <br/>
<a href="https://github.com/ElliotMillington">Elliot Millington</a> <br/>

Project Researcher: <br/>
Sarune Savickaite
