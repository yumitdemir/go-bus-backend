
## Short Description
Go Bus is a bus booking website that I developed as a personal project. The main functionality of the website is to provide a platform for users to book bus tickets. Users can select their departure and arrival bus stops, choose the departure date, view the available buses, and then proceed to book their tickets. The website is designed to be fully responsive, providing a seamless user experience across different devices.

## Technology Stack
This project is a full-stack application that uses ASP.NET Core Web API for the backend and React for the front end.
### Backend
The backend of the application is built using ASP.NET Core Web API. It handles all the server-side operations of the application. It uses JWT authentication to secure the endpoints and ensure that only authorized users can access certain routes. Also when the user selects one trip and when he/she is in the booking process the seats are blocked for 10 minutes and if the user doesn't make the booking during that time the blocked seats get released.
### Frontend
The front end of the application is built using React. It consists of two main parts: an admin dashboard and guest pages. The admin dashboard provides an interface for administrators to manage the application, including viewing and managing bookings. It features server-side pagination to efficiently handle large amounts of data. The guest pages are where users can select their bus stops, view available buses, and book their tickets.
## Website Video
Please click the image to watch the youtube video
[![youtbe](https://img.youtube.com/vi/9jXndwC4Ar4/0.jpg)](https://www.youtube.com/watch?v=9jXndwC4Ar4)

## Screenshots of The Website
### Guest
![home](https://github.com/yumitdemir/go-bus-frontend/assets/108368506/d7f35215-452f-4e4b-bb44-e43d5c75f2ad)
<pre>                               </pre>
![screencapture-localhost-5173-2023-09-25-01_25_04](https://github.com/yumitdemir/go-bus-frontend/assets/108368506/99e1125a-3839-4fd1-aa10-16a86d3c2aa6)
<pre>                               </pre>
![search](https://github.com/yumitdemir/go-bus-frontend/assets/108368506/79bcbcf5-e807-4f89-9605-846cf5fb7d13)
<pre>                               </pre>
![screencapture-localhost-5173-search-2023-09-25-01_25_30](https://github.com/yumitdemir/go-bus-frontend/assets/108368506/1819956c-9f88-4965-a178-fcad7eb1d5d1)

<pre>                               </pre>
![checkout](https://github.com/yumitdemir/go-bus-frontend/assets/108368506/e5710194-55cd-455a-aa4b-0ffdc2b1a0d4)
<pre>                               </pre>
![managebooking](https://github.com/yumitdemir/go-bus-frontend/assets/108368506/ae7121ac-f20a-47f5-874a-f58fdef6c224)
<pre>                               </pre>
![managebooking card](https://github.com/yumitdemir/go-bus-frontend/assets/108368506/a731e127-a6d4-4e01-b624-edb5cae40754)
<pre>                               </pre>
![cancledbooking](https://github.com/yumitdemir/go-bus-frontend/assets/108368506/15ffa745-f427-42d6-95d1-7b05eafcc04c)

### Admin
![screencapture-localhost-5173-admin-2023-09-25-01_26_00](https://github.com/yumitdemir/go-bus-frontend/assets/108368506/68936781-926c-452a-9d6d-7d22a2da7481)
<pre>                               </pre>

![screencapture-localhost-5173-admin-trips-2023-09-25-01_26_16](https://github.com/yumitdemir/go-bus-frontend/assets/108368506/349295b0-9c6d-4f8f-a530-b43e2d49d0d3)

<pre>                               </pre>

![screencapture-localhost-5173-admin-add-trip-2023-09-25-01_26_57](https://github.com/yumitdemir/go-bus-frontend/assets/108368506/5d545c0f-8acd-4f47-ba67-e25f6a0fa762)
