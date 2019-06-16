# ALGroups
ALGroups is a platform for dicovering and managing groups of friends.

This project was developed for the *Web Application Developent* course at the University of Bucharest.

## Features
* Group discovery - See the latest groups and search by name
* Membership management - Anybody can request membership, but only the group moderators can accept. Users can also be kicked out.
* Content - The users can post messages, upload files and set events in the calendar. The moderators can filter and delete the content.
* Admin - a user that can see every group, delete contemt, kick users and revoke their access to the platform.

## Technologies
* ASP MVC with Razer templates.

## Development
With this project I experimented with a *Clean Architecture* inspired structure. Every piece of logic in the app is coded in it's use case (UseCases folder).
