# NASA Photos (Watchguard test) - Jerry Williams
## Introduction
The initial commit for this application includes a simple class library which reads the given dates.txt file and consumes the web request outlined on NASA's website. It then downloads the files to the local hard drive (currently, the C:\Temp directory, more on this later).

### Models
Only one model exists for this project, the `Photo` class. I'm using the `id` field as the filename, and the Model includes the `img_src` field to download the picture.

### Services
Most of the logic is in the PhotoService.cs (which implements the IPhotoService interface). There are some helper methods (in-code documentation) to make things cleaner.

### Testing
There is a testing project (separate repository, linked here https://github.com/williagr/WatchGuardNASAPhotos.Test).

### To Do
-Combine these two repositories (Unit Test project was added after the fact)
-Create MVC web application to view local pictures and display in web browser
-Add to Docker containers
